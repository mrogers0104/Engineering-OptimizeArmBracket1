using System;
using System.Collections.Generic;
using System.Linq;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using Newtonsoft.Json;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    enum BktWeldType
    {
        Unknown,
        PJP_1,
        PJP_2,
        CJP
    }

    public class SaddleBracketData
    {
        //private static readonly object SaddleBracketBendingCriteriaLock = new object();

        private static readonly object SaddleBracketThicknessLock = new object();

        private static readonly object StdBracketLock = new object();

        private static readonly object BracketBoltGradeLock = new object();

        private static readonly object BracketToArmWeldSizeLock = new object();

        private static List<PlateMaterialDataItem> _saddleBracketAllowableThicknesses;

        //private static List<SaddleBracketBendingCriteriaDataItem> _saddleBracketBendingCriteria;

        private static List<StdBracketDataItem> _stdBrackets;

        private static List<BracketBoltGrade> _bracketBoltGrade;

        private static List<BracketToArmWeldSize> _bracketToArmWeldSize;

        private static BktWeldType _defaultBktWeldType = BktWeldType.PJP_1;

        //        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<PlateMaterialDataItem> SaddleBracketAllowableThicknesses
        {
            get
            {
                lock (SaddleBracketThicknessLock)
                {
                    if (_saddleBracketAllowableThicknesses == null)
                    {
                        InitializeSaddleBracketThicknessData();
                    }

                    return _saddleBracketAllowableThicknesses;
                }
            }
        }

        //private static List<SaddleBracketBendingCriteriaDataItem> SaddleBracketBendingCriteria
        //{
        //    get
        //    {
        //        lock (SaddleBracketBendingCriteriaLock)
        //        {
        //            if (_saddleBracketBendingCriteria == null)
        //            {
        //                InitializeSaddleBracketBendingCriteria();
        //            }

        //            return _saddleBracketBendingCriteria;
        //        }
        //    }
        //}

        private static List<StdBracketDataItem> StdBrackets
        {
            get
            {
                lock (StdBracketLock)
                {
                    if (_stdBrackets == null)
                    {
                        InitializeStdBrackets();
                    }

                    return _stdBrackets;
                }
            }

        }

        private static List<BracketBoltGrade> BracketBoltGrades
        {
            get
            {
                lock (BracketBoltGradeLock)
                {
                    if (_bracketBoltGrade == null)
                    {
                        InitializeBracketBoltGrade();
                    }

                    return _bracketBoltGrade;
                }
            }

        }

        private static List<BracketToArmWeldSize> BracketToArmWeldSizes
        {
            get
            {
                lock (BracketToArmWeldSizeLock)
                {
                    if (_bracketToArmWeldSize == null)
                    {
                        InitializeBracketWeldSize();
                    }

                    return _bracketToArmWeldSize;
                }
            }

        }

        //public static List<AvailableBolt> GetAvailableBolts(bool allowA354BCBolts)
        //{
        //    List<AvailableBolt> bolts = new List<AvailableBolt>();

        //    bolts.Add(new AvailableBolt(.625, StructuralBolts.StructuralBoltGrades.A325));
        //    bolts.Add(new AvailableBolt(.75, StructuralBolts.StructuralBoltGrades.A325));
        //    bolts.Add(new AvailableBolt(1, StructuralBolts.StructuralBoltGrades.A325));
        //    bolts.Add(new AvailableBolt(1.25, StructuralBolts.StructuralBoltGrades.A325));
        //    bolts.Add(new AvailableBolt(1.5, StructuralBolts.StructuralBoltGrades.A325));

        //    if (!allowA354BCBolts) return bolts;

        //    bolts.Add(new AvailableBolt(1.5, StructuralBolts.StructuralBoltGrades.A354BC));
        //    bolts.Add(new AvailableBolt(1.75, StructuralBolts.StructuralBoltGrades.A354BC));
        //    bolts.Add(new AvailableBolt(2, StructuralBolts.StructuralBoltGrades.A354BC));
        //    bolts.Add(new AvailableBolt(2.25, StructuralBolts.StructuralBoltGrades.A354BC));
        //    bolts.Add(new AvailableBolt(2.5, StructuralBolts.StructuralBoltGrades.A354BC));
        //    bolts.Add(new AvailableBolt(2.75, StructuralBolts.StructuralBoltGrades.A354BC));
        //    bolts.Add(new AvailableBolt(3, StructuralBolts.StructuralBoltGrades.A354BC));

        //    return bolts;
        //}

        //public static List<PlateMaterialDataItem> GetAvailableThicknesses(PlateFinish finish)
        //{
        //    return SaddleBracketAllowableThicknesses.FindAll(o => o.FinishEnum == finish);
        //}

        //public static double GetMaximumAllowableThickness(PlateFinish finish)
        //{
        //    try
        //    {
        //        List<PlateMaterialDataItem> descList = SaddleBracketAllowableThicknesses.OrderByDescending(o => o).ToList();
        //        PlateMaterialDataItem item = descList.Find(o => o.FinishEnum == finish);

        //        if (item == null)
        //        {
        //            string msg = string.Format("Thickness for finish {0} not found.", finish);
        //            Exception ex = new Exception(msg);
        //            throw ex;
        //        }

        //        return (double)item.MaterialThickness;

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "Error determining maximum allowable Saddle Bracket thickness.");
        //        throw;
        //    }
        //}

        //public static double GetMaximumBoltDiameterForBracketThickness(double thickness)
        //{
        //    if (DoubleUtil.areEqual(thickness, .5)) return 1.0;
        //    if (DoubleUtil.areEqual(thickness, .625)) return 1.0;
        //    if (DoubleUtil.areEqual(thickness, .75)) return 1.5;
        //    if (DoubleUtil.areEqual(thickness, 1.0)) return 1.5;
        //    if (DoubleUtil.areEqual(thickness, 1.25)) return 2.0;
        //    if (DoubleUtil.areEqual(thickness, 1.5)) return 2.0;
        //    if (DoubleUtil.areEqual(thickness, 1.75)) return 2.75;
        //    if (DoubleUtil.areEqual(thickness, 2)) return 3.0;

        //    throw new Exception(string.Format("Unable to find maximum bolt diameter for a bracket thickness of {0}", thickness));
        //}


        public static double GetMinimumAllowableThickness(PlateFinish finish)
        {
            try
            {
                PlateMaterialDataItem item = SaddleBracketAllowableThicknesses.Find(o => o.FinishEnum == finish);

                if (item == null)
                {
                    string msg = string.Format("Thickness for finish {0} not found.", finish);
                    Exception ex = new Exception(msg);
                    throw ex;
                }

                return (double)item.MaterialThickness;
            }
            catch (Exception ex)
            {
                //                logger.Fatal(ex, "Error determining minimum allowable Saddle Bracket thickness.");
                throw;
            }
        }

        //public static double GetMinimumBendRadius(BendRadiusType bendType, double plateThickness)
        //{
        //    try
        //    {
        //        SaddleBracketBendingCriteriaDataItem item = SaddleBracketBendingCriteria.Find(o => o.BendRadiusType == bendType && o.PlateThickness == (decimal)plateThickness);

        //        if (item == null)
        //        {
        //            string msg = string.Format("Bend radius for bend type {0} and plate thickness {1} not found.", bendType, plateThickness);
        //            Exception ex = new Exception(msg);
        //            throw ex;
        //        }

        //        return (double)item.MinimumBendRadius;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "Error determining Saddle Bracket minimum bend radius.");
        //        throw;
        //    }

        //}

        //public static double GetMinimumBracketBoltDistance(BendRadiusType bendType, double plateThickness)
        //{
        //    try
        //    {
        //        SaddleBracketBendingCriteriaDataItem item = SaddleBracketBendingCriteria.Find(o => o.BendRadiusType == bendType && o.PlateThickness == (decimal)plateThickness);

        //        if (item == null)
        //        {
        //            string msg = string.Format("Bracket bolt distance for bend type {0} and plate thickness {1} not found.", bendType, plateThickness);
        //            Exception ex = new Exception(msg);
        //            throw ex;
        //        }

        //        return (double)item.MinimumBoltDistance;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "Error determining Saddle Bracket minimum bracket bolt distance.");
        //        throw;
        //    }
        //}

        //public static double GetNextLargerAllowableThickness(PlateFinish finish, double thickness)
        //{
        //    try
        //    {
        //        PlateMaterialDataItem item = SaddleBracketAllowableThicknesses.Find(o => o.FinishEnum == finish && (double)o.MaterialThickness > thickness);

        //        if (item == null)
        //        {
        //            string msg = string.Format("Thickness for finish {0} and greater than {1} not found.", finish, thickness);
        //            Exception ex = new Exception(msg);
        //            throw ex;
        //        }

        //        return (double)item.MaterialThickness;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "Error determining next larger allowable Saddle Bracket thickness.");
        //        throw;
        //    }
        //}

        public static List<StdBracketDataItem> GetStdBracket(TubularArm arm)
        {
            List<StdBracketDataItem> stdBktList = new List<StdBracketDataItem>();

            List<StdBracketDataItem> FWTBktList = new List<StdBracketDataItem>();
            List<StdBracketDataItem> SabreBktList = new List<StdBracketDataItem>();

            BracketArmType bktArmType = (arm.Shape.SideCount == 6 ? BracketArmType.Hex : BracketArmType.Common);

            double armBaseWidth = arm.BaseFlatWidthInches;
            double armThickness = arm.MaterialThicknessInches;

            // ** Round armBaseWidth down to nearest 1/8".
            // ** QT rounds up to two decimal places, for example, armBaseWidth = 10.875 becomes
            // ** 10.88, which is not in the Standard Bracket table.
            double rndDnArmBaseWidth = armBaseWidth.Round(0.125);
            //double rndDnArmBaseWidth = Rounding.Round(armBaseWidth, 0.125);

            // ** Get the standard Sabre brackets.
            if (bktArmType == BracketArmType.Common)
            {
                SabreBktList = StdBrackets.FindAll(o => rndDnArmBaseWidth <= o.STDMaxArmBaseDiam
                                                    && armBaseWidth >= o.STDMinArmBaseDiam
                                                    && armThickness <= o.STDMaxArmThick
                                                    && o.ArmType == BracketArmType.Common);

                FWTBktList = StdBrackets.FindAll(o => rndDnArmBaseWidth <= o.STDMaxArmBaseDiam
                                                    && armBaseWidth >= o.STDMinArmBaseDiam
                                                    && o.ArmType == BracketArmType.FWTall);
            }
            else
            {
                SabreBktList = StdBrackets.FindAll(o => rndDnArmBaseWidth <= o.HEXMaxArmBaseDiam
                                                    && armBaseWidth >= o.HEXMinArmBaseDiam
                                                    && armThickness <= o.STDMaxArmThick
                                                    && o.ArmType == BracketArmType.Common);

                FWTBktList = StdBrackets.FindAll(o => rndDnArmBaseWidth <= o.HEXMaxArmBaseDiam
                                                    && armBaseWidth >= o.HEXMinArmBaseDiam
                                                    && o.ArmType == BracketArmType.FWTall);
            }

            // ** Combine the two standard bracket lists
            stdBktList.AddRange(SabreBktList);
            stdBktList.AddRange(FWTBktList);

            // ** Check the width vs height for an irregular polygon (ie hex arm)
            if (!arm.Shape.IsRegularPolygon)
            {
                if (GetPolygonWidthAndHeight(arm, out double minBktWidth, out double minBktHt))
                {
                    stdBktList = stdBktList.FindAll(s => s.ThruPlateWidth >= minBktWidth && s.Height >= minBktHt);
                }
            }

            return stdBktList;
        }

        /// <summary>
        /// Get the width and height of an irregular polygon (FWT shape)
        /// </summary>
        /// <param name="arm">The davit arm</param>
        /// <param name="minBktWidth">The minimum bracket width in inches.</param>
        /// <param name="minBktHt">The minimum bracket height in inches.</param>
        /// <returns>
        /// Returns TRUE if the height factor was found, otherwise, FALSE
        /// </returns>
        /// <remarks>
        /// Compute the angle of the arm to get the mitered height required.
        /// </remarks>
        public static bool GetPolygonWidthAndHeight(TubularArm arm, out double minBktWidth, out double minBktHt)
        {
            int nSides = arm.Shape.SideCount;
            double angle = Math.PI / nSides;
            double cosAng = Math.Cos(angle);

            double armAngle = 0;
            if (arm.PLSLengthFt > 0)
            {
                armAngle = Math.Atan(arm.TipVerticalOffset / (arm.PLSLengthFt.FeetToInches()));
            }

            minBktWidth = arm.BaseFlatWidthInches;
            minBktHt = minBktWidth;

            string shapeDescr = arm.Shape.Description.ToLower();
            string[] d = shapeDescr.Split(' ');
            if (!arm.Shape.Description.Contains(" to ") && d.Length < 3)
            {
                return false;
            }

            double fact;
            double.TryParse(d[1], out fact);

            if (fact <= 0)
            {
                return false;
            }

            double w = arm.BaseFlatWidthInches;
            double p = w / cosAng;

            if (nSides % 4 == 0)    // everything but Hex
            {
                if (arm.Shape.TubeOrientation == TubeOrientation.FlatToZero)
                {
                    minBktWidth = w;
                    minBktHt = minBktWidth;
                }
                else  // Point
                {
                    minBktWidth = p * fact;
                    minBktHt = minBktWidth;
                }
            }
            else    // Hex
            {
                if (arm.Shape.TubeOrientation == TubeOrientation.FlatToZero)
                {
                    minBktWidth = w * fact;
                    minBktHt = w;
                }
                else  // Point
                {
                    minBktWidth = w;
                    minBktHt = w * fact;
                }
            }

            minBktHt /= Math.Abs(Math.Cos(armAngle));

            return true;
        }

        //private static void InitializeSaddleBracketBendingCriteria()
        //{
        //    try
        //    {
        //        _saddleBracketBendingCriteria = JsonConvert.DeserializeObject<List<SaddleBracketBendingCriteriaDataItem>>(Properties.Resources.SaddleBracketBendingCriteria_json);
        //        _saddleBracketBendingCriteria.Sort();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "Error trying to initialize Saddle Bracket Bending Criteria");
        //        throw;
        //    }

        //}

        private static void InitializeSaddleBracketThicknessData()
        {
            try
            {
                _saddleBracketAllowableThicknesses = JsonConvert.DeserializeObject<List<PlateMaterialDataItem>>(Properties.Resources.SaddleBracketMaterial_json);
                _saddleBracketAllowableThicknesses.Sort();
            }
            catch (Exception ex)
            {
                //                logger.Fatal(ex, "Error trying to initialize Saddle Bracket Thickness data");
                throw;
            }
        }

        private static void InitializeStdBrackets()
        {
            try
            {
                _stdBrackets = JsonConvert.DeserializeObject<List<StdBracketDataItem>>(Properties.Resources.SabreFWTStdBrackets_json);
                _stdBrackets.Sort();
            }
            catch (Exception ex)
            {
                //                logger.Fatal(ex, "Error trying to initialize Std Brackets");
                throw;
            }
        }

        public class AvailableBolt
        {
            public AvailableBolt(double diameter, StructuralBolts.StructuralBoltGrades grade)
            {
                Diameter = diameter;
                Grade = grade;
            }

            public double Diameter { get; set; }
            public StructuralBolts.StructuralBoltGrades Grade { get; set; }
        }

        private static void InitializeBracketBoltGrade()
        {
            try
            {
                _bracketBoltGrade = JsonConvert.DeserializeObject<List<BracketBoltGrade>>(Properties.Resources.BracketBoltGrade_json);
                //_bracketBoltGrade.Sort();
            }
            catch (Exception ex)
            {
                //                logger.Fatal(ex, "Error trying to initialize Bracket Bolt Grades");
                throw;
            }
        }

        private static void InitializeBracketWeldSize()
        {
            try
            {
                _bracketToArmWeldSize = JsonConvert.DeserializeObject<List<BracketToArmWeldSize>>(Properties.Resources.BracketToArmWeldSize_json);
                //_bracketToArmWeldSize.Sort();
            }
            catch (Exception ex)
            {
                //                logger.Fatal(ex, "Error trying to initialize Bracket to Arm Weld Size");
                throw;
            }
        }

        ///// <summary>
        ///// Get the bolt grade given the bracket bolt spec and bolt diameter.
        ///// If bolt grade is not found, Fy and Fu = 0
        ///// </summary>
        ///// <param name="boltSpec"></param>
        ///// <param name="boltDia"></param>
        ///// <param name="Fy"></param>
        ///// <param name="Fu"></param>
        //public static void GetBracketBoltGrade(string boltSpec, double boltDia, out double Fy, out double Fu)
        //{
        //    Fy = 0;
        //    Fu = 0;

        //    // ** Get the bracket bolt grade given the bolt spec and bolt dia.
        //    BracketBoltGrade bktBoltGradeList = BracketBoltGrades.FindAll(o => o.BoltSpec.Equals(boltSpec)).FirstOrDefault();

        //    if (bktBoltGradeList == null)
        //    {
        //        return;
        //    }

        //    if (boltDia <= 1.0)
        //    {
        //        Fy = bktBoltGradeList.Fy_1;
        //        Fu = bktBoltGradeList.Fu_1;
        //    }
        //    else if (boltDia > 1.5)
        //    {
        //        Fy = bktBoltGradeList.Fy_3;
        //        Fu = bktBoltGradeList.Fu_3;
        //    } else
        //    {
        //        Fy = bktBoltGradeList.Fy_2;
        //        Fu = bktBoltGradeList.Fu_2;
        //    }
        //}

        ///// <summary>
        ///// Get the bracket to arm weld size given the arm wall thickness in inches.
        ///// </summary>
        ///// <param name="armWallThick">Arm wall thickness, in.</param>
        ///// <returns>Return the weld size for the default weld type: PJP_1, PJP_2, or CJP</returns>
        //public static double GetBracketToArmWeldSize(double armWallThick)
        //{
        //    double weldSize = 0;

        //    BracketToArmWeldSize bktWeldSize = BracketToArmWeldSizes.Find(o => o.ArmWallThick == armWallThick);

        //    if (bktWeldSize == null)
        //    {
        //        return weldSize;
        //    }

        //    switch (_defaultBktWeldType)
        //    {
        //        case BktWeldType.PJP_1:
        //            weldSize = bktWeldSize.PJP_1;
        //            break;
        //        case BktWeldType.PJP_2:
        //            weldSize = bktWeldSize.PJP_2;
        //            break;
        //        case BktWeldType.CJP:
        //            weldSize = bktWeldSize.CJP;
        //            break;
        //        default:
        //            weldSize = 0;
        //            break;
        //    }
        //    return weldSize;
        //}

        ///// <summary>
        ///// Get the bracket to arm weld threshold given the arm wall thickness in inches.
        ///// </summary>
        ///// <param name="armWallThick">Arm wall thickness, in.</param>
        ///// <returns></returns>
        //public static double GetBracketToArmWeldThreshold(double armWallThick)
        //{
        //    double weldSize = 0;

        //    BracketToArmWeldSize bktWeldSize = BracketToArmWeldSizes.Find(o => o.ArmWallThick == armWallThick);

        //    if (bktWeldSize == null)
        //    {
        //        return weldSize;
        //    }

        //    return bktWeldSize.Threshold;
        //}

    }
}