using System.Collections.Generic;
using ArmBracketAnalysisLib.BracketAnalysis;

namespace ArmBracketAnalysisLib.Reports
{
    public class ArmBracketContainer
    {

        private ArmsProject _project;

        public ArmBracketContainer() { }

        public ArmBracketContainer(ArmsProject project, TubularArm arm)
        {
            _project = project;

            MyTubularArm = arm;

            // ** Get the associated arm bracket
            ArmConnection connection = _project.GetArmConnection(arm.Id);

            TubularArmAttachmentPoint attach = connection.Attachments.Find(a => a.Azimuth == arm.Azimuth);

            MySaddleBracket = (SaddleBracket)attach;

        }

        public TubularArm MyTubularArm { get; set; }

        public SaddleBracket MySaddleBracket { get; set; }

        #region Bracket Results

        public Dictionary<BracketStressType, BracketControllingResult> BracketResults
        {
            get
            {
                BracketLoads bktLoads = MySaddleBracket.BracketAnalysisLoads;
                if (MySaddleBracket.BracketAnalysisLoads == null || MySaddleBracket.BracketAnalysisLoads.BracketAnalysisResults == null)
                {
                    return null;
                }
                else
                {
                    return MySaddleBracket.BracketAnalysisLoads.BracketAnalysisResults;
                }
            }
        }


        ///// <summary>
        ///// Dictionary containing the bracket stress results.
        ///// Used in Bracket Analysis Report
        ///// </summary>
        //public BracketResult BracketThicknessStressKsi { get { return BracketResults[BracketStressType.BracketThickness]; } }

        /// <summary>
        /// Dictionary containing the bracket leg stress results.
        /// Used in Bracket Analysis Report
        /// </summary>
        public BracketResult BracketLegStressKsi { get { return GetBracketResults(BracketStressType.BracketLegStress); } }

        ///// <summary>
        ///// The controlling bracket stress, that is, the maximum of 
        ///// bracket thickness and bracket leg stress
        ///// </summary>
        //public BracketResult BracketStressKsi
        //{
        //    get
        //    {
        //        double bkt = BracketThicknessStressKsi.Value;
        //        double bktLeg = BracketLegStressKsi.Value;

        //        BracketResult controllingStress = (bkt > bktLeg ? BracketThicknessStressKsi : BracketLegStressKsi);

        //        return controllingStress;
        //    }
        //}

        public BracketResult BracketShearRuptureKsi { get { return GetBracketResults(BracketStressType.BracketShearRupture); } }

        public BracketResult BracketBearingKsi { get { return GetBracketResults(BracketStressType.BracketBearing); } }

        public BracketResult BracketBoltsKsi { get { return GetBracketResults(BracketStressType.BracketBolts); } }

        /// <summary>
        /// The arm orientation: Flat-Flat or Point-Point.
        /// </summary>
        public string ArmOrientation
        {
            get
            {
                TubeOrientation tubeOrientation = MyTubularArm.Shape.TubeOrientation;
                string tubeOrient = (tubeOrientation == TubeOrientation.FlatToZero ? "Flat-Flat" : "Point-Point");

                return tubeOrient;
            }
        }

        /// <summary>
        /// List of controlling load cases for this bracket.
        /// Used in the Bracket Analysis Report.
        /// </summary>
        public List<BracketLoad> ControllingResult
        {
            get
            {
                List<BracketLoad> lCase = new List<BracketLoad>();
                if (MySaddleBracket.BracketAnalysisLoads == null || MySaddleBracket.BracketAnalysisLoads.ControllingLoadcaseList == null)
                {
                    return lCase;
                }
                else
                {
                    return MySaddleBracket.BracketAnalysisLoads.ControllingLoadcaseList;
                }
            }
        }

        public double BracketBoltCenterSpace { get { return MySaddleBracket.GetBktBoltCenterSpace(); } }


        #endregion


        private BracketResult GetBracketResults(BracketStressType stressType)
        {
            return (BracketResults == null ? null : BracketResults[stressType]);

        }
    }
}
