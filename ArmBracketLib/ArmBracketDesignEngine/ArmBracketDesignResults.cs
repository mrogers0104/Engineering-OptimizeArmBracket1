using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using ArmBracketDesignLibrary.StructureComponents.Data;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class ArmBracketDesignResults
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ArmBracketDesignResults()
        { }

        public ArmBracketDesignResults(List<ArmBracketDesignInput> bracketDesignInputs)
        {
            ArmBracketDesignOutputs.AddRange(RunDesigns(bracketDesignInputs));
        }

        /// <summary>
        /// The bracket design output results for each bracket.
        /// </summary>
        public List<ArmBracketDesignOutput> ArmBracketDesignOutputs { get; set; } = new List<ArmBracketDesignOutput>();

        /// <summary>
        /// Run all the arm bracket designs.
        /// </summary>
        /// <param name="bracketDesignInputs"></param>
        /// <returns></returns>
        private List<ArmBracketDesignOutput> RunDesigns(List<ArmBracketDesignInput> bracketDesignInputs)
        {
            List<ArmBracketDesignOutput> bracketOutputs = new List<ArmBracketDesignOutput>();

            foreach (ArmBracketDesignInput input in bracketDesignInputs)
            {
                logBracketInputs(input);
                ArmBracketDesignOutput bracketOutput = new ArmBracketDesignOutput(input.BundleId, input.UserInputs.BracketID);

                try
                {
                    ArmProject armProject = new ArmProject();
                    armProject.PoleTaper = input.ValuesFromPLSData.PoleTaper;
                    armProject.PoleSideCount = input.ValuesFromPLSData.PoleSideCount;
                    armProject.PoleOuterDiameterIn = input.ValuesFromPLSData.PoleDiameterIn;
                    armProject.PoleWallThickness = input.ValuesFromPLSData.PoleWallThickness;
                    armProject.AllowA354BCBolts = input.UserInputs.AllowA354BCBolts;
                    armProject.AllowCenterBolts = input.UserInputs.AllowCenterBolts;
                    armProject.ArmsAtLocation = input.UserInputs.TwoArmsAtLocation ? 2 : 1;

                    armProject.Customer = input.UserInputs.Customer;
                    armProject.BracketFinish = (PlateFinish)Enum
                        .Parse(typeof(PlateFinish), input.UserInputs.BracketFinish);

                    armProject.BracketMaterialSpecification
                        = (PlateMaterialSpecification)Enum
                        .Parse(typeof(PlateMaterialSpecification), input.UserInputs.BracketMaterialSpecification.Replace('-', '_'));

                    // set to true always and remove from user input
                    armProject.UseArmPoleOffsets = true;

                    armProject.ArmPlateFinish = (PlateFinish)Enum.Parse(typeof(PlateFinish), input.UserInputs.ArmFinish);

                    int dIdx = input.ValuesFromPLSData.ArmType.IndexOf("Davit", StringComparison.OrdinalIgnoreCase);
                    if (dIdx >= 0)
                    {
                        armProject.TubularDavitArm = new TubularDavitArm(input, armProject);
                    }
                    else
                    {
                        armProject.TubularXArm = new TubularXArm(input, armProject);
                    }

                    if (!input.UserInputs.CustomBracketInput.IsValid())
                    {
                        throw new Exception("CustomBracketInput properties must either all be null or all be populated.");
                    }

                    if (input.UserInputs.CustomBracketInput.AllNull())
                    {
                        armProject.SaddleBracket = armProject.RunDesign();
                    }
                    else
                    {
                        armProject.ThruPlateConnection = new ThruPlateConnection
                        {
                            ThruPlateOpening = (double)input.UserInputs.CustomBracketInput.ThruPlateWidth
                        };

                        bool isGalv = (armProject.ArmPlateFinish == PlateFinish.Galvanized);

                        TubularArm arm = armProject.TubularArm;

                        var customBkt = CustomBracketData.GetCustomBracket(arm);
                        string customBktId = customBkt?.PartNumber ?? string.Empty;

                        SaddleBracketDTO sb = new SaddleBracketDTO()
                        {
                            BracketThick = (double)input.UserInputs.CustomBracketInput.BracketThick.Value,
                            BracketOpening = (double)input.UserInputs.CustomBracketInput.BracketOpening.Value,
                            IsStandardBracket = false,
                            StdBracketID = customBktId,
                            BoltQty = input.UserInputs.CustomBracketInput.TotalBoltQty.Value / 2,  // use half total bolt qty : added mar 6/8/20
                            BoltDiameter = (double)input.UserInputs.CustomBracketInput.BoltDiameter.Value,
                            BoltLength = (double)input.UserInputs.CustomBracketInput.BoltLength.Value,
                            BracketHeight = (double)input.UserInputs.CustomBracketInput.BracketHeight.Value,
                            StiffenerQty = input.UserInputs.CustomBracketInput.StiffenerQty.Value,
                            StiffenerThick = (double)input.UserInputs.CustomBracketInput.StiffenerThick.Value,
                            //StiffenerWidth = (double)input.UserInputs.CustomBracketInput.StiffenerWidth.Value,
                            ThruPlateWidth = (double)input.UserInputs.CustomBracketInput.ThruPlateWidth.Value,
                            ThruPlateThick = (double)input.UserInputs.CustomBracketInput.ThruPlateThick.Value,
                            BracketMaterialSpecification = input.UserInputs.BracketMaterialSpecification,
                            BracketFinish = input.UserInputs.BracketFinish,
                            ThruPlateSpec = input.UserInputs.BracketMaterialSpecification
                        };

                        armProject.SaddleBracket = new SaddleBracket(armProject.ThruPlateConnection, sb, armProject);
                        armProject.RunAnalysis();
                    }

                    bracketOutput.ProcessBracketOutput(armProject);
                }
                catch (Exception e)
                {
                    Message msg = new Message(MessageCategory.Error, "Exception running design", e);
                    bracketOutput.Messages.Add(msg);
                }
                finally
                {
                    bracketOutputs.Add(bracketOutput);
                    logBracketOutput(bracketOutput);
                }
            }

            return bracketOutputs;

            void logBracketInputs(ArmBracketDesignInput input)
            {
                try
                {
                    string j = JsonConvert.SerializeObject(input);
                    ArmBracketDesignInput in2 = JsonConvert.DeserializeObject<ArmBracketDesignInput>(j);
                    //in2.ValuesFromPLSData.ArmJoints = null;
                    //in2.ValuesFromPLSData.ArmLoads = null;
                    logger.Info("{@i}", in2);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error trying to log BRACKET WEBAPI INPUT");
                }
            }

            void logBracketOutput(ArmBracketDesignOutput output)
            {
                try
                {
                    string j = JsonConvert.SerializeObject(output);
                    ArmBracketDesignOutput r2 = JsonConvert.DeserializeObject<ArmBracketDesignOutput>(j);
                    //r2.Arm.ArmJoints = null;
                    //r2.Arm.ArmLoads = null;
                    logger.Info("{@i}", r2);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error trying to log ARM BRACKET WEBAPI OUTPUT");
                }
            }
        }
    }
}