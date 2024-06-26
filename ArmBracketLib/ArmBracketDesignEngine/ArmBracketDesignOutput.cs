using ArmBracketDesignLibrary.BracketAnalysis;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.StructureComponents;
using MaterialsLibrary;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

//

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class ArmBracketDesignOutput
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private const double EPSILON = .000001d;

        private ArmBracketDesignOutput()
        { }

        public ArmBracketDesignOutput(Guid bundleId, string bracketId)
        {
            BundleId = bundleId;
            BracketId = bracketId;
        }

        /// <summary>
        /// The Id for this run.
        /// </summary>
        private Guid BundleId { get; set; }

        /// <summary>
        /// The id for the bracket being designed.
        /// </summary>
        public string BracketId { get; set; }

        /// <summary>
        /// The messages generated during the bracket design.
        /// </summary>
        ///
        public List<Message> Messages { get; set; } = new List<Message>();

        /// <summary>
        /// The design for this bracket worked.
        /// </summary>
        public bool DesignWorked { get; set; }

        ///// <summary>
        ///// The controlling load case by BracketStressType.
        ///// </summary>
        //public BracketLoads BracketAnalysisLoads { get; set; }

        public List<BracketResultsDTO> BracketControllingResults { get; set; }

        public List<BracketLoadcaseDTO> ControllingLoadcases { get; set; }

        public SaddleBracketDTO BracketDTO { get; set; }

        public TubularArmDTO Arm { get; set; }

        /// <summary>
        /// Assign the bracket design results to the output object DTOs.
        /// </summary>
        public void ProcessBracketOutput(ArmProject armProject)
        {
            Messages.AddRange(armProject.AnalysisWarningMessages);

            try
            {
                SaddleBracket bracket = armProject.SaddleBracket;
                if (bracket != null)
                {
                    BracketLoadCalcs bracketLoads = bracket.BracketAnalysisLoads;

                    DesignWorked = bracketLoads.DesignWorked;

                    LoadControllingResults(bracketLoads.BracketAnalysisResults);

                    LoadControllingLoadcase(bracketLoads.ControllingLoadcaseList);
                }

                BracketDTO = new SaddleBracketDTO(bracket);

                Arm = new TubularArmDTO(armProject.TubularArm);

                Arm.LengthAdjustedForBracketFt = AdjLength();
                Arm.WeightAdjustedForBracket = AdjWeight();

                double AdjWeight()
                {
                    double sideWidth = RegularPolygon.GetSideLength(Arm.Shape.SideCount, AverageCircumRadius());
                    double volume = sideWidth.InchesToFeet() * Arm.LengthAdjustedForBracketFt * Arm.Shape.SideCount *
                                    Arm.MaterialThicknessInches.InchesToFeet();
                    return Math.Round(volume * 490, 2);
                }

                double AdjLength()
                {
                    try
                    {
                        string spec = BracketDTO?.BoltSpec;
                        if (spec != null && BracketDTO.BoltSpec.ToUpper().Equals("A354BC"))
                        {
                            spec = "A354-BC";
                        }

                        if (spec != null && BracketDTO != null)
                        {
                            StructuralBolt_ML bktBolt = MaterialsLibraryBO.GetStructuralBolt((decimal)BracketDTO.BoltDiameter, spec, includeInactive: false);
                            double multiplier = Arm.Armtype == 2 ? 2d : 1d; // 2 = crossarm

                            // *** Is the arm attached to the face or the pole center?
                            bool isAttachedToFace = IsArmAttachedToFace();
                            double poleRadiusIn = (!isAttachedToFace ? Arm.PoleDiameterIn / 2.0 : 0);

                            // *** NOTE: For standard brackets, Hocks BracketDTO.BracketSideWidth includes the bracket thickness.
                            // *** Adding the bracket thickness would be doubling up on the bracket thickness
                            double bktThick = (BracketDTO.IsStandardBracket ? 0 : BracketDTO.BracketThick);
                            double bktFullWidth = bktBolt.BktBoltToPoleFace + BracketDTO.BracketSideWidth + bktThick;
                            return Arm.LengthFt - multiplier * ((bktFullWidth + poleRadiusIn) / 12d);
                        }
                        return 0d;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Could not determine arm length for {Arm.Label}");
                        throw;
                    }
                }

                bool IsArmAttachedToFace()
                {
                    bool toFace = false;

                    // ??? Are arm joints defined for this arm.
                    if (Arm.ArmJoints.Count <= 0)
                    {
                        return toFace;
                    }

                    // *** Look for the origin ":O" joint.
                    string armLabel = $"{Arm.Label}:";
                    //var armJoint = Arm.ArmJoints.FirstOrDefault(a => a.JointLabel.EndsWith(":O", StringComparison.OrdinalIgnoreCase));
                    //var armJoint = Arm.ArmJoints.FirstOrDefault(a => a.JointPosition.Equals("Origin", StringComparison.OrdinalIgnoreCase));
                    var armJoint = Arm.ArmJoints.FirstOrDefault(a => a.JointPosition != null &&
                                                                     a.JointPosition.Equals("Origin", StringComparison.OrdinalIgnoreCase));

                    if (armJoint == null)
                    {
                        return toFace;
                    }

                    // *** If the joint starts with the arm label, the arm is face mounted.
                    // *** Otherwise, it's pole centerline mounted.
                    toFace = armJoint.JointLabel.StartsWith(armLabel, StringComparison.OrdinalIgnoreCase);
                    return toFace;
                }

                double AverageCircumRadius()
                {
                    double toNeutral = 0.445;  // k factor for neutral axis
                    int sideCount = Arm.Shape.SideCount;
                    double apothTip = (Arm.TipFlatWidthInches - 2 * (Arm.MaterialThicknessInches * (1.0 - toNeutral))) / 2d;
                    double apothBas = (Arm.BaseFlatWidthInches - 2 * (Arm.MaterialThicknessInches * (1.0 - toNeutral))) / 2d;
                    double averageApothem = (apothTip + apothBas) / 2d;
                    //double averageApothem = (Arm.BaseFlatWidthInches / 2d + Arm.TipFlatWidthInches / 2d) / 2d;
                    return RegularPolygon.GetCircumradius(sideCount, averageApothem);
                }
            }
            catch (Exception e)
            {
                Message msg = new Message(MessageCategory.Error, "Exception in ProcessBracketOutput", e);
                Messages.Add(msg);
            }
        }

        /// <summary>
        /// Define the controlling load case DTO
        /// </summary>
        /// <param name="controllingLoadcase"></param>
        private void LoadControllingLoadcase(List<BracketLoad> controllingLoadcase)
        {
            ControllingLoadcases = new List<BracketLoadcaseDTO>();

            foreach (BracketLoad bktResult in controllingLoadcase)
            {
                BracketLoadcaseDTO lCaseDto = new BracketLoadcaseDTO()
                {
                    LoadAtPoleFace = new BracketLoadDTO()
                    {
                        AxialForce = bktResult.AxialForce,
                        HorizontalMoment = bktResult.HorizontalMoment,
                        HorizontalShear = bktResult.HorizontalShear,
                        JointLabel = bktResult.JointLabel,
                        Label = bktResult.Label,
                        LoadCase = bktResult.LoadCase,
                        LongitudinalOffset = bktResult.LongitudinalOffset,
                        RelDist = bktResult.RelDist,
                        Torsion = bktResult.Torsion,
                        TransverseOffset = bktResult.TransverseOffset,
                        VerticalMoment = bktResult.VerticalMoment,
                        VerticalOffset = bktResult.VerticalOffset,
                        VerticalShear = bktResult.VerticalShear
                    },

                    ControllingStressTypeResults = new Dictionary<BracketStressType, bool>(bktResult.ControllingStressTypeResults)
                };

                ControllingLoadcases.Add(lCaseDto);
            }
        }

        /// <summary>
        /// Create the Analysis Results list
        /// </summary>
        /// <param name="bracketResults"></param>
        private void LoadControllingResults(Dictionary<BracketStressType, BracketControllingResult> bracketResults)
        {
            BracketControllingResults = new List<BracketResultsDTO>();

            foreach (var kvp in bracketResults)
            {
                BracketStressType stressType = kvp.Key;
                BracketControllingResult result = kvp.Value;

                BracketResultsDTO bktResults = new BracketResultsDTO()
                {
                    StressType = stressType,
                    BracketResult = new BracketResultDTO()
                    {
                        Allowable = result.Allowable,
                        Description = result.Description,
                        Value = result.Value,
                        InteractionRatio = Math.Abs(result.Allowable) < EPSILON ? 0d : result.Value / result.Allowable
                    }
                };

                BracketControllingResults.Add(bktResults);
            }
        }
    }
}