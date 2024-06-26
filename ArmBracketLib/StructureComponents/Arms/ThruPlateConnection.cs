using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using ArmBracketDesignLibrary.BracketAnalysis;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.StructureComponents.Data;
using NLog;
using System;
using System.Collections.Generic;

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{
    [Serializable]
    public class ThruPlateConnection : ArmConnection
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly object HeightLock = new object();

        //private PlateMaterialSpecification _materialSpecification;

        //private double _materialStrength;

        #region Constructors

        public ThruPlateConnection() : base(Guid.NewGuid())
        {
        }

        //public ThruPlateConnection(ThruPlateConnectionDTO dtoThruPlate) : this()
        //{
        //    ThruPlateOpening = dtoThruPlate.ThruPlateOpening;
        //}

        #endregion Constructors

        #region Properties

        public double ThruPlateOpening { get; set; }

        public override ConnectionType ConnectionType
        {
            get { return ConnectionType.ThruPlate; }
        }

        public double BracketWidth
        {
            get { return ThruPlateOpening + .125; }
        }

        public double Height
        {
            get
            {
                lock (HeightLock)
                {
                    return GetMaxBracketHeight();
                }
            }
        }

        public override string ConnectionTypeLabel
        {
            get { return "Thru Plate"; }
        }

        #endregion Properties

        public override double GetConnectionOffsetFromPoleFace(TubularArmAttachmentPoint attachment) //, BendRadiusType bendType)
        {
            SaddleBracket bracket = (SaddleBracket)attachment;

            //double offset = ThruPlateCalculator.GetPoleFaceToBracketBolt(bracket);
            double offset = Utils.GetPoleFaceToBracketBolt(bracket);

            return offset.InchesToFeet();
        }

        public double GetMaxBracketHeight()
        {
            double height = 0;

            foreach (TubularArmAttachmentPoint bracket in Attachments)
            {
                height = Math.Max(height, bracket.Height);
            }

            return height;
        }

        public override void FindWorkingStdBracket(ArmProject project)
        {
            // We get to this point only if DesignMethodSpecified is STND

            // ** Brackets are initialized separately, to allow the thru plate spacing to be completely
            // ** done for all arms, before attempting to set bracket info.
            TubularArm arm = project.TubularArm;

            SaddleBracket thisBkt = project.SaddleBracket;

            BracketArmType bktArmType = arm.Shape.SideCount == 6 ? BracketArmType.Hex : BracketArmType.Common;

            // get the possible brackets, test them, and use one if test succeeds
            SaddleBracket sb = CheckStdBrackets(arm);
            if (sb != null) // found matching standard bracket that worked
            {
                sb.Azimuth = arm.Azimuth;
                sb.DesignMethodUsed = (sb.DesignMethodUsed != BracketDesignMethod.FAIL ? BracketDesignMethod.STND : sb.DesignMethodUsed);
                arm.DesignMethodUsed = sb.DesignMethodUsed; // BracketDesignMethod.STND;
                sb.BracketMaterialSpecification = project.BracketMaterialSpecification;
                AddAttachment(sb);
            }
            else if (sb == null && bktArmType == BracketArmType.Hex)
            {   // ** Std Hex bracket could not be found
                arm.Parent.AnalysisWarningMessages.Add(new Message(MessageCategory.Warn, $"No Standard hex arm bracket found for arm {arm.Label}."));
                arm.DesignMethodUsed = BracketDesignMethod.NOHX;
            }
            else if (sb == null && bktArmType == BracketArmType.Common)
            {   // ** Std bracket failed
                arm.Parent.AnalysisWarningMessages.Add(new Message(MessageCategory.Warn, $"No Std bracket found or bracket not adequate to support loads for Arm {arm.Label}."));
                arm.DesignMethodUsed = BracketDesignMethod.UNKN;
            }
        }

        /// <summary>
        /// Check to see if the current QT3 version is &ge; the QT3 version containing
        /// functionality for Bracket and Thru Plate overrides.
        /// This will ensure that the property values for a bracket is assigned values
        /// for calculations later, such as, arm length, etc.
        /// </summary>
        /// <returns></returns>
        private SaddleBracket CheckStdBrackets(TubularArm arm)
        {
            ArmProject project = arm.Parent;
            ArmConnection connection = project.ThruPlateConnection;

            if (connection == null) return null;

            BracketArmType bktArmType = arm.Shape.SideCount == 6 ? BracketArmType.Hex : BracketArmType.Common;
            PlateFinish finish = arm.Finish;
            string finishName = (finish == PlateFinish.Galvanized ? "G" : "W");

            List<StdBracketDataItem> stdCandidates = SaddleBracketData.GetStdBracket(arm);

            // ** Use only Sabre Std Brackets (Common) -- remove this "FindAll" to include FWT std brackets
            stdCandidates = stdCandidates.FindAll(s => s.ArmType == BracketArmType.Common);

            if (stdCandidates == null || stdCandidates.Count == 0)
            {
                return null;
            }

            // ** Sort the bracket candidates by weight so we just pick the
            // ** first successful one, which will be the lightest
            // !! For smaller brackets, sometimes the "heavy" is lighter depending on finish
            // !! so we must distinguish between the finish type and sort accordingly.
            // !!
            // !! If the standard bracket is a YK08-LX or YK08-HX, DO NOT sort by wt.
            // !! YK08-HX is lighter than YK08-LX.
            bool isYK08 = stdCandidates[0].BracketID.StartsWith("YK08", StringComparison.CurrentCultureIgnoreCase) && stdCandidates.Count > 1;

            if (finish == PlateFinish.Weathering)
            {
                stdCandidates.Sort((o1, o2) => o1.WeatheringWeight.CompareTo(o2.WeatheringWeight));
            }
            else
            {
                if (!isYK08)
                {
                    stdCandidates.Sort((o1, o2) => o1.GalvWeight.CompareTo(o2.GalvWeight));
                }
            }

            // ** Std Bracket counter
            int bCnt = 0;

            foreach (StdBracketDataItem stdb in stdCandidates)
            {
                bCnt++;
                SaddleBracket sb = new SaddleBracket(connection, stdb, project);

                // ** Replace the "X" in the StdBracketID to indicate the finish.
                // ** G = galvanized & W = weathering.
                sb.StdBracketID = sb.StdBracketID.Replace("X", finishName);

                if (BracketThruPlateDesign.IsArmConnectionAdequate(arm, sb))
                {
                    return sb;
                }
                else
                {
                    if (bCnt == stdCandidates.Count)    // return bracket information for last bracket, even though it failed.
                    {
                        sb.DesignMethodUsed = BracketDesignMethod.FAIL;
                        return sb;
                    }
                }
            }

            return null;
        }
    }
}