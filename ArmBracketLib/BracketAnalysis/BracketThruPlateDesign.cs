using System;
using System.Collections.Generic;
using System.Linq;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using ArmBracketDesignLibrary.StructureComponents.Data;
using MaterialsLibrary;
using NLog;

//using System.Windows.Forms;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    /// <summary>
    /// Arm bracket and thru plate design.  The method used here
    /// is defined in the Arm Bracket Design spreadsheet written by Hock Lim.
    /// </summary>
    public static class BracketThruPlateDesign
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static ArmProject _project;

        /// <summary>
        /// Analyze the arm bracket to determine if it is adequate to support the loads for each load case.
        /// </summary>
        /// <param name="bracket"></param>
        /// <param name="arm"></param>
        /// <remarks>
        /// Outline of bracket analysis:
        ///     *)BracketLoads
        ///         -) Get BracketLoad for each load case
        ///         -) Set up BracketBolts for each load
        ///             ~) Define BracketBoltGroup: location, shear, moment
        ///             ~) Define BracketProperties: stresses - thickness, leg, shear rupture, bearing
        ///         -) Determine controlling load case for this bracket
        /// </remarks>
        /// <returns>
        /// Return TRUE if bracket design worked, otherwise FALSE.
        /// </returns>
        public static bool BracketAnalysis(SaddleBracket bracket, TubularArm arm)
        {
            // ** BracketLoads will get all the Arm Connection Design loads for this arm
            // ** Each load case will be considered in the analysis.
            BracketLoadCalcs bracketLoads = new BracketLoadCalcs(arm, bracket);

            if (bracketLoads.BracketLoadItems.Count == 0)
            {
                return false;
            }

            bracketLoads.DoBracketAnalysis();

            if (bracketLoads.ControllingLoadcaseList == null || bracketLoads.ControllingLoadcaseList.Count <= 0)
            {
                string bktId = (string.IsNullOrEmpty(bracket.StdBracketID) ? "" : $" / ID: {bracket.StdBracketID}");
                string msg = $"No loads found for arm bracket {arm.Label} @ Dft = {arm.Dft} {bktId}";
                logger.Warn(msg);
            }


            return bracketLoads.DesignWorked;
        }

        /// <summary>
        /// Is the connection adequate to resist the arm loads?
        /// This is standard or designed bracket / thru plate
        /// </summary>
        /// <param name="arm"></param>
        /// <param name="bracket"></param>
        /// <returns>Return TRUE if the bracket works, otherwise FALSE.</returns>
        public static bool IsArmConnectionAdequate(TubularArm arm, SaddleBracket bracket)
        {

            // ** If this is a standard bracket, make sure it will work with the arm loads
            bool stdBkt = bracket.IsStandardBracket;

            // ** I don't care what the DesignMethodSpecified is.  Analyze the bracket.
            if (bracket != null)  // && bracket.DesignMethodSpecified == BracketDesignMethod.STND)
            {
                string n = (stdBkt ? "standard" : "design");

                bool designWorked = BracketAnalysis(bracket, arm);

                if (designWorked)
                {
                    return true;   // design worked!!
                }
            }

            // TODO bracket failed - design method is unknown until design procedure is in place.
            bracket.DesignMethodUsed = BracketDesignMethod.UNKN; // BracketDesignMethod.DSGN;
            return false;   // Std bracket design failed!! Use STS connection design for this connection
        }

        ///// <summary>
        ///// Design an arm bracket.
        ///// !!!!NOTE!!!!!  this is FAR from being done!!
        ///// </summary>
        ///// <param name="arm"></param>
        ///// <param name="connection"></param>
        ///// <returns>
        ///// Return true if design worked, otherwise false.
        ///// </returns>
        //public static bool BracketDesign(TubularArm arm, ArmConnection connection)
        //{
        //    //ThruPlateConnection thruPlate = (ThruPlateConnection)connection;
        //    //SaddleBracket bracket = (SaddleBracket)thruPlate.GetAttachmentPointByAzimuth(arm.Azimuth);

        //    BendRadiusType bendRadiusType = _project.BendRadiusType;
        //    double initialBracketThickness = 0.625;
        //    bool coped = _project.AllowArmCoping;

        //    // ********************************************************************************************
        //    // ** The initialization process resulted in UNKN state of bracket, values were not initialized
        //    // ** Set them to a starting point so DesignConnection does not fail.
        //    bracket.BracketThick = (bracket.BracketThick <= 0 ? initialBracketThickness: bracket.BracketThick);
        //    bracket.BoltDiameter = (bracket.BoltDiameter <= 0 ? 0.625 : bracket.BoltDiameter);
        //    bracket.BoltSpec = (bracket.BoltSpec == StructuralBolts.StructuralBoltGrades.None ?
        //                                            StructuralBolts.StructuralBoltGrades.A325 : bracket.BoltSpec);


        //    //// ** BracketLoads will get all the Arm Connection Design loads for this arm
        //    //// ** Each load case will be considered in the design/analysis.
        //    //BracketLoads bracketLoads = new BracketLoads(arm, bracket);

        //    //if (bracketLoads.BracketLoadItems.Count == 0)
        //    //{
        //    //    return false;
        //    //}


        //    //Dictionary<string, List<BracketLoad>> bracketLoad = bracketLoads.GetBracketLoads();
        //    //foreach (var kvp in bracketLoad)
        //    //{
        //    //    string lCase = kvp.Key;
        //    //    List<BracketLoad> bktLoads = kvp.Value;

        //    //    foreach (var load in bktLoads)
        //    //    {

        //    //    }

        //    //}

        //    List<SaddleBracket> goodBracket = new List<SaddleBracket>();

        //    double minBracketHeight = GetArmHeight(arm);
        //    List<StructuralBolt_ML> bktBolts = GetBracketBolts();
        //    PlateFinish plateFinish = arm.Finish;
        //    List<PlateMaterialDataItem> bktThicknesses = SaddleBracketData.GetAvailableThicknesses(plateFinish);

        //    foreach (var thick in bktThicknesses)
        //    {
        //        double bktThick = (double) thick.MaterialThickness;

        //        foreach (var bolt in bktBolts)
        //        {

        //            double boltDia = bolt.Diameter;

        //            SaddleBracket newBracket = new SaddleBracket(bracket.Parent);

        //            double minBracketOpening = BracketProperties.ComputeMinBracketOpening(arm, bktThick, bendRadiusType, nonCoped: !coped);
        //            //minBracketOpening = Rounding.RoundUp(minBracketOpening, 1.0);
        //            minBracketOpening = minBracketOpening.RoundUp(1.0);

        //            newBracket.BracketOpening = minBracketOpening;
        //            newBracket.Height = minBracketHeight;
        //            newBracket.BoltDiameter = boltDia;

        //            if (IsArmConnectionAdequate(arm, newBracket))
        //            {
        //                goodBracket.Add(newBracket);
        //            }

        //        }

        //    }


        //    return true;
        //}

        //private static void DefineNewBracketGeometry(TubularArm arm, SaddleBracket bracket, double bktThick, double boltDia)
        //{
        //    double minBracketOpening = BracketProperties.ComputeMinBracketOpening(arm, bktThick, bendRadiusType, nonCoped: !coped);
        //    minBracketOpening = Rounding.RoundUp(minBracketOpening, 1.0);

        //    newBracket.BracketOpening = minBracketOpening;
        //    newBracket.Height = minBracketHeight;
        //    newBracket.BoltDiameter = boltDia;
        //}

        //private static void DefineNewBracketStiffeners(ArmConnection connection, SaddleBracket bracket)
        //{
        //    ThruPlateConnection thruPlate = (ThruPlateConnection)connection;
        //    double W = thruPlate.

        //}

        /// <summary>
        /// Get a list of bolt to use for bracket design.
        /// </summary>
        /// <returns></returns>
        private static List<StructuralBolt_ML> GetBracketBolts()
        {
            List<StructuralBolt_ML> bktBolts = MaterialsLibraryBO.GetStructuralBolts(includeInactive: false);
            List<StructuralBolt_ML> a325Bolts = (from b in bktBolts
                                                 where b.QtGradeName.Equals("A325", StringComparison.CurrentCultureIgnoreCase) &&
                                                      (b.Diameter >= 0.625 && b.Diameter <= 1.50)
                                                 select b).ToList();

            List<StructuralBolt_ML> a354Bolts = (from b in bktBolts
                                                 where b.QtGradeName.Equals("A354BC", StringComparison.CurrentCultureIgnoreCase) &&
                                                      (b.Diameter >= 1.50 && b.Diameter <= 3.00)
                                                 select b).ToList();
            bktBolts.Clear();
            bktBolts.AddRange(a325Bolts);
            if (_project.AllowA354BCBolts)
            {
                bktBolts.AddRange(a354Bolts);
            }

            return bktBolts;
        }

        /// <summary>
        /// Get the height of the arm in inches.
        /// </summary>
        /// <param name="arm"></param>
        /// <returns></returns>
        private static double GetArmHeight(TubularArm arm)
        {
            int nSides = arm.Shape.SideCount;
            TubeOrientation tubeOrientation = arm.Shape.TubeOrientation;

            double angle = Math.PI / nSides;
            double Dff = arm.BaseFlatWidthInches;
            double Dpp = Dff / Math.Cos(angle);

            double ht = (tubeOrientation == TubeOrientation.FlatToZero ? Dff : Dpp);

            return ht;
        }

        //private static Dictionary<int, SaddleBracketData.AvailableBolt> EstimateBoltQuantity(SaddleBracket bracket, BracketLoad bracketLoad)
        //{
        //    List<SaddleBracketData.AvailableBolt> boltList = SaddleBracketData.GetAvailableBolts(_project.AllowA354BCBolts);

        //    foreach (var bolt in boltList)
        //    {

        //        double initQty = ComputeBoltQuantity(bracket, bolt);

        //    }


        //}

        //private static double ComputeBoltQuantity(SaddleBracket bracket, SaddleBracketData.AvailableBolt bolt)
        //{
        //    double boltDia = bolt.Diameter;
        //    string boltSpec = bolt.Grade.ToString();
        //    boltSpec = (boltSpec.Contains("354BC") ? boltSpec.Replace("BC", "-BC") : boltSpec);
        //    ICommonBolt myBolt = MaterialsLibraryBO.GetStructuralBolt((decimal)boltDia, boltSpec, includeInactive: false);
        //    double Ag = myBolt.GrossArea;
        //    double Fy = myBolt.FyKsi;

        //    double W = bracket.BracketOpening;
        //    double H = bracket.Height;

        //    double p1 =
        //}

        #region Helpers



        #endregion


        #region Saddle Bracket Extensions

        /// <summary>
        /// Get the number of bolt holes in a bracket quadrant.
        /// </summary>
        /// <param name="bkt"></param>
        /// <returns></returns>
        public static int GetNumberBktQuadBolts(this SaddleBracket bkt)
        {
            double dJ = Math.Truncate(bkt.BoltQty / 2.0);

            return (int)dJ;
        }

        /// <summary>
        /// Compute the distance from the center of the bracket to the first bolt, in.
        /// If an odd number of bolts, c = 0.
        /// </summary>
        /// <param name="bkt"></param>
        /// <returns></returns>
        public static double GetBktBoltCenterSpace(this SaddleBracket bkt)
        {
            int j = bkt.GetNumberBktQuadBolts();
            double H = bkt.Height;
            double b = bkt.BoltTopEdgeDistance;
            double s = bkt.BoltSpacing;

            double c = (j == 0 ? 0: H/2.0 - b - ((j - 1) * s));

            return c;
        }

        #endregion
    }
}
