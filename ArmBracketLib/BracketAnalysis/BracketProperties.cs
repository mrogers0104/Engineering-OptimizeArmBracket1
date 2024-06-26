using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using ArmBracketDesignLibrary.StructureComponents.Data;
using MaterialsLibrary;
using NLog;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    
    public enum BracketStressType
    {
        Unknown,
        //BracketThickness,
        BracketLegStress,
        BracketShearRupture,
        BracketBearing,
        BracketBolts
    }

    /// <summary>
    /// Contains saddle bracket properties: I, C, Moment, stress, etc.
    /// </summary>
    public class BracketProperties
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private BracketBoltGroup _bracketBoltGroup = null;
        private List<BracketBolt> _bktBolts = null;


        private TubularArm _arm = null;
        private SaddleBracket _bracket = null;
        private ThruPlateConnection _connection = null;

        private TubeOrientation tubeOrientation = TubeOrientation.FlatToZero;

        private double _Dff = 0;        // arm flat-flat, in.
        private double _Dpp = 0;        // arm point-point, in.
        private double _fw = 0;         // flat width, in

        private double _Fyb = 0;        // yield strength of the bracket material, ksi
        private double _Fub = 0;        // ultimate strength of bracket material, ksi
        private double _Fut = 0;        // ultimate strength of thru plate material, ksi

        private double _Tb = 0;         // bracket thickness, in.
        private double _W = 0;          // thru plate opening, in.
        private double _H = 0;          // thru plate (bracket) height, in.
        private double _Tt = 0;         // thru plate thickness, in.
        private double _Ts = 0;         // stiffener thickness, in.
        private double _boltDia = 0;    // bracket bolt diameter, in.
        private double _boltHole = 0;   // bracket bolt hole diameter, in.
        private int _boltQty = 0;       // bracket bolt quantity
        private double _B = 0;          // bracket leg length from bracket face to tip of leg, in
        private double _e = 0;          // bracket edge distance, in.
        private double _Hh = 0;         // ??


        #region Constructors

        /// <summary>
        /// Define the bracket properties in order to compute stress in bracket.
        /// </summary>
        /// <param name="arm">TubularDavitArm object the bracket is attached. </param>
        /// <param name="bracket">The SaddleBracket object being analyzed</param>
        /// <param name="boltGroup">The bolt group for the bracket being analyzed</param>
        /// <param name="nonCoped">Is this arm non-coped? (Default = true)</param>
        public BracketProperties(TubularArm arm, SaddleBracket bracket, BracketBoltGroup boltGroup, bool nonCoped = true)
        {
            //_arm = (TubularDavitArm)arm;
            _arm = arm;
            _bracket = bracket;
            _connection = (ThruPlateConnection)_bracket.Parent;
            _bracketBoltGroup = boltGroup;
            _bktBolts = _bracketBoltGroup.GetBracketBolts();

            //_project = _arm.Parent.Parent;

            MyStiffenerGroup = new StiffenerGroup(_bracket, _bracketBoltGroup);
            NonCopedArm = nonCoped;

            BracketResults = new Dictionary<BracketStressType, BracketResult>();
            //WarningMessages = new StringBuilder();
            WarningMessages = new List<Message>();

            // ** define fields used for calculations
            int nSides = _arm.Shape.SideCount;
            double angle = Math.PI / nSides;
            tubeOrientation = _arm.Shape.TubeOrientation;
            _Dff = _arm.BaseFlatWidthInches;
            _fw = _Dff * Math.Tan(angle);
            _Dpp = _Dff / Math.Cos(angle) - (_fw / 2.0) * Math.Sin(angle);

            _W = _connection.ThruPlateOpening;
            _Tt = _connection.Thickness;

            _Fyb = _bracket.MaterialStrength;
            _Tb = _bracket.BracketThick;
            _H = _bracket.Height;
            _Ts = _bracket.StiffenerThick;
            _boltDia = _bracket.BoltDiameter;
            _boltQty = _bracket.BoltQty;
            _boltHole = _boltDia + 0.125;

            _e = _bracket.BoltSideEdgeDistance;
            _B = _bracket.BracketSideWidth + _e;

            _Hh = (_boltQty == 1 ? Math.Min(_H, 24.0 * _Tb) : _H / 2.0);

            _Fub = GetUltimateStrength();
            _Fut = _Fub;                    // TODO: thru plate material strength is not defined in the ThruPlateConnection object

            SetupBolts();  

            ComputeBracketResults();

            CheckBracketResults();
        }

        #endregion

        #region Properties

        public StiffenerGroup MyStiffenerGroup { get; set; }    

        /// <summary>
        /// This bracket is for a non-coped arm if true.
        /// </summary>
        public bool NonCopedArm { get; set; }

        #region Bracket Leg Properties

        /// <summary>
        /// Bracket inside face to bolt plus edge distance, in.
        /// (B - Tb)
        /// </summary>
        public double Leg { get { return _B - _Tb; } }

        /// <summary>
        /// Half the arm height, in.
        /// </summary>
        public double  HalfArmHt
        {
            get
            {
                double armHt = (tubeOrientation == TubeOrientation.FlatToZero ? _Dff : _Dpp) / 2.0;
                return armHt;
            }
        }


        /// <summary>
        /// Distance to the centroid of the bracket leg, in.
        /// </summary>
        public double Centroid { get { return ComputeCentroid(); } }

        /// <summary>
        /// Distance to outer fiber of bracket leg
        /// from the center of the bracket, in.
        /// </summary>
        public double DistOuterFiber { get { return ComputeDistOutFiber(); } }

        /// <summary>
        /// Moment of inertia about the bracket leg about the center line, in^4
        /// </summary>
        public double I { get { return ComputeMomentOfInertia(); } }

        /// <summary>
        /// The sum of the bracket leg moment from the bolt group
        /// </summary>
        public double SumBracketMoment
        {
            get
            {
                double moment = _bktBolts.Sum(b => b.BracketMomentFtKips);

                return moment;
            }
        }

        #endregion  // Bracket Leg Properties

        #region Bracket Results

        /// <summary>
        /// The minimum bracket height, in.
        /// </summary>
        public double BracketMinHeight { get { return ComputeMinimumBracketHeight(); } }

        /// <summary>
        /// The minimum edge distance, in.
        /// </summary>
        public double BracketMinEdgeDistance { get { return GetMinEdgeDistance(); } }

        /// <summary>
        /// The minimum bracket plate thickness, in.
        /// </summary>
        public double BracketMinThickness { get; set; }

        /// <summary>
        /// The stress for the actual bracket thickness used, ksi.
        /// </summary>
        public double BracketThicknessCapacityKsi { get; set; }

        /// <summary>
        /// The minimum bracket opening considering whether coped or non-coped, in.
        /// </summary>
        public double BracketMinOpening
        {
            get
            {
                double opening = ComputeMinBracketOpening(_arm, _bracket.BracketThick, nonCoped: true);
                return opening;
            }
        }

        public Dictionary<BracketStressType, BracketResult> BracketResults { get; set; }

        /// <summary>
        /// bracket bolt stress results.
        /// </summary>
        /// <remarks>
        /// A look at the bolt stress in the BracketResults dictionary.
        /// </remarks>
        public BracketResult BracketBoltStressKsi { get { return BracketResults[BracketStressType.BracketBolts]; } }

        ///// <summary>
        ///// bracket stress results.
        ///// </summary>
        ///// <remarks>
        ///// A look at the bracket plate stress in the BracketResults dictionary.
        ///// </remarks>
        //public BracketResult BracketThicknessStressKsi { get { return BracketResults[BracketStressType.BracketThickness]; } }

        /// <summary>
        /// Bracket leg stress results
        /// </summary>
        /// <remarks>
        /// A look at the bracket leg stress in the BracketResults dictionary.
        /// </remarks>
        public BracketResult BracketLegStressKsi { get { return BracketResults[BracketStressType.BracketLegStress]; } }

        /// <summary>
        /// Bracket shear rupture results
        /// </summary>
        /// <remarks>
        /// A look at the bracket shear rupture in the BracketResults dictionary.
        /// </remarks>
        public BracketResult BracketShearRuptureKsi { get { return BracketResults[BracketStressType.BracketShearRupture]; } }

        /// <summary>
        /// Bracket bearing results
        /// </summary>
        /// <remarks>
        /// A look at the bracket bearing stress in the BracketResults dictionary.
        /// </remarks>
        public BracketResult BracketBearingKsi { get { return BracketResults[BracketStressType.BracketBearing]; } }

        /// <summary>
        /// Check the bracket to see if it will work with the given loads and
        /// defined dimensions of the arm, thru plate, and bracket.
        /// </summary>
        public bool IsBracketAdequate { get { return CheckBracketResults(); } }

        /// <summary>
        /// Warning messages if the bracket is not adequate.
        /// </summary>
        public List<Message> WarningMessages { get; set; }
        //public StringBuilder WarningMessages { get; set; }

        #endregion  // Bracket Results


        #endregion  // Properties


        #region Methods

        /// <summary>
        /// Compute the bracket leg moment arm and moment for each bolt in the bolt group
        /// </summary>
        private void SetupBolts()
        {
            List<BracketBolt> bktBolts = _bracketBoltGroup.GetBracketBolts();

            foreach (var bolt in bktBolts)
            {
                double d = bolt.DistFromCenter;
                double Marm = (d > HalfArmHt ? d - HalfArmHt : 0);
                double fv = bolt.ShearForceKips;
                double M = Marm * fv;

                bolt.BracketMomArm = Marm;
                bolt.BracketMomentFtKips = M;
            }
        }

        /// <summary>
        /// Get the ultimate strength of the bracket and/or thru plate material, ksi
        /// </summary>
        /// <returns>Return ultimate strength in ksi</returns>
        private double GetUltimateStrength()
        {
            double Fu = 0;
            // ** Get ultimate strength of plate material.
            List<SteelPlateMaterial> plates = MaterialsLibraryBO.GetPlates(includeInactive: false);
            string plateSpec = _bracket.BracketMaterialSpecification.ToString();
            plateSpec = plateSpec.Replace("_", "-");
            SteelPlateMaterial plate = plates.Find(p => p.MaterialSpecAndYield == plateSpec);
            if (plate == null)
            {
                Dictionary<double, double> strength = new Dictionary<double, double>()
                {
                    {36.0, 50.0},
                    {42.0, 60.0},
                    {46.0, 65.0},
                    {50.0, 75.0},
                    {65.0, 80.0},
                };
                double fy = _bracket.MaterialStrength;
                Fu = strength[fy];
            } else
            {
                Fu = (double) plate.FuKsi;
            }

            return Fu;
        }

        /// <summary>
        /// Get the minimum edge distance for this bracket
        /// </summary>
        /// <returns></returns>
        private double GetMinEdgeDistance()
        {
            if (_boltDia <= 0)
            {
                logger.Error($"BracketProperties.GetMinEdgeDistance: bolt diameter NOT defined: {_boltDia}\" dia for spec {_bracket.BoltSpec}");
                return 0.0;
            }
            string boltSpec = _bracket.BoltSpec.ToString();
            boltSpec = (boltSpec.Contains("354BC") ? boltSpec.Replace("BC", "-BC") : boltSpec);

            StructuralBolt_ML bolt = MaterialsLibraryBO.GetStructuralBolt((decimal)_boltDia, boltSpec, includeInactive: false);
            double emin1 = (bolt == null ? 0 : bolt.BktBoltEdgeDistance);
            double emin2 = _Tb + _boltDia / 2.0;
            double emin3 = _Ts + _boltDia / 2.0;

            double e = Math.Max(emin1, emin2);
            e = Math.Max(e, emin3);

            return e;
        }

        /// <summary>
        /// Compute the centroid of the stiffener group.
        /// </summary>
        /// <returns></returns>
        private double ComputeCentroid()
        {
            double num = (_W * _Tb) / 4.0 + (Leg * _B) / 2.0;
            double den = _W / 2.0 + Leg;

            if (den == 0)
            {
                return 0;
            }

            double y = num / den;

            return y;
        }

        /// <summary>
        /// Compute the distance to the outer fiber of the stiffener group, in.
        /// </summary>
        /// <returns></returns>
        private double ComputeDistOutFiber()
        {
            double y = Centroid;
            double C = _B - y;

            return C;
        }

        /// <summary>
        /// Compute the moment of inertia of the stiffener group, in^4.
        /// </summary>
        /// <returns></returns>
        private double ComputeMomentOfInertia()
        {
            double y = Centroid;
            double p1 = y - _Tb / 2.0;
            double p2 = _B / 2.0 - y;

            double I = _W * _Tb * _Tb * _Tb / 24.0;
            I += _Tb * Leg * Leg * Leg / 12;
            I += (_W * _Tb / 2.0) * p1 * p1;
            I += (_Tb * Leg) * p2 * p2;

            return I;
        }


        private void ComputeBracketResults()
        {
            ComputeBracketThickness();

            ComputeBracketLegStress();

            ComputeBracketShearRupture();

            ComputeBracketBearing();

            ComputeBracketBolts();

        }

        private void ComputeBracketThickness()
        {
            double sumMoment = _bracketBoltGroup.SumMomentFtKip;
            double T = 6.0 * sumMoment / (_Hh * _Fyb);
            T = Math.Sqrt(T);

            BracketMinThickness = T;

            double F = 6.0 * sumMoment / (_Hh * _Tb * _Tb);

            BracketThicknessCapacityKsi = F;

            // ** Note: we want to see stress ratio > 1.0 for over stress.
            //BracketResult bracketResult = new BracketResult(_Fyb, F, "Bracket Thickness Stress");
            //BracketResult bracketResult = new BracketResult(F, _Fyb, "Bracket Thickness Stress");

            //BracketResults.Add(BracketStressType.BracketThickness, bracketResult);
        }

        /// <summary>
        /// Compute the bracket leg stress, ksi
        /// </summary>
        private void ComputeBracketLegStress()
        {
            double stifStress = MyStiffenerGroup.StressKsi;

            double Cmax = DistOuterFiber;
            double sumBktMoment = Math.Abs(_bracketBoltGroup.SumBracketMomentFtKip);
            double bktStress = sumBktMoment * Cmax / I;

            double maxStress = Math.Max(stifStress, bktStress);

            //BracketLegStressKsi = new BracketResult(maxStress, _Fyb, "Bracket Leg Stress");
            BracketResult bracketResult = new BracketResult(maxStress, _Fyb, "Bracket Leg Stress");

            BracketResults.Add(BracketStressType.BracketLegStress, bracketResult);
        }

        /// <summary>
        /// Compute the shear rupture in the bracket.
        /// </summary>
        private void ComputeBracketShearRupture()
        {
            double maxBoltShear = _bracketBoltGroup.MaxShearForceKips;
            double T = Math.Min(_Tb, _Tt);

            double stress = maxBoltShear / (2.0 * (_e - _boltHole / 2.0) * T);
            double Fu = Math.Min(_Fub, _Fut);

            double F = 0.45 * Fu;

            //BracketShearRuptureKsi = new BracketResult(stress, F, "Bracket Shear Rupture");
            BracketResult bracketResult = new BracketResult(stress, F, "Bracket Shear Rupture");

            BracketResults.Add(BracketStressType.BracketShearRupture, bracketResult);
        }

        /// <summary>
        /// Compute the bracket bearing stress and capacity.
        /// </summary>
        private void ComputeBracketBearing()
        {
            double maxBoltShear = _bracketBoltGroup.MaxShearForceKips;
            double T = Math.Min(_Tb, _Tt);

            double stress = maxBoltShear / (_boltDia * T);
            double Fu = Math.Min(_Fub, _Fut);

            double F = 1.90 * Fu;

            //BracketBearingKsi = new BracketResult(stress, F, "Bracket Bearing Stress");
            BracketResult bracketResult = new BracketResult(stress, F, "Bracket Bearing Stress");

            BracketResults.Add(BracketStressType.BracketBearing, bracketResult);
        }

        /// <summary>
        /// Set the bracket bolt results.
        /// </summary>
        private void ComputeBracketBolts()
        {
            double maxShearStress = _bracketBoltGroup.MaxCombinedShearKsi;
            double boltCapacity = _bracketBoltGroup.ShearCapacity;

            BracketResult bracketResult = new BracketResult(maxShearStress, boltCapacity, "Bracket Bolt Shear Stress");
            BracketResults.Add(BracketStressType.BracketBolts, bracketResult);
        }



        /// <summary>
        /// Compute the maximum bracket width for a given arm butt diameter
        /// This uses Hocks calculations for a non-coped arm (3/3/17)
        /// </summary>
        /// <remarks>
        /// This method is also located in SaddleBracketCalculators.ComputeBracketWidthNonCoped.  Moved here to put as 
        /// many related methods in one place as possible.
        /// </remarks>
        /// <returns>
        /// Return the computed non-coped bracket width rounded up to next inch.  
        /// </returns>
        public static double ComputeMinBracketOpening(TubularArm arm, double bracketThickness, bool nonCoped) // , BendRadiusType bendRadiusType, bool nonCoped)
        {
            //BendRadiusType radiusType = _project.BendRadiusType;
            //int nSides = _arm.Shape.SideCount;
            //double Dpp = _Dff / Math.Cos(angle);
            //BendRadiusType radiusType = bendRadiusType;
            TubeOrientation tubeOrientation = arm.Shape.TubeOrientation;
            int nSides = arm.Shape.SideCount;
            double Dff = arm.BaseFlatWidthInches;
            double angle = Math.PI / nSides;
            double Dpp = Dff / Math.Cos(angle);


            double maxGap = 0.125;
            double OD = 0;
            double ts = arm.MaterialThicknessInches;
            double tb = bracketThickness; // _bracket.BracketThick; // bracketThickness;
            double inR;
            double outR;
            //double factor = (radiusType == BendRadiusType._2T ? 2.0 : 3.0); // (tb < 0.75 ? 1.5 : 3);
            double factor = BracketBendRadii.GetBendRadiusFactor(tb);

            if (nSides == 6)
            {
                OD = (tubeOrientation == TubeOrientation.FlatToZero ? Dpp : Dff);
            }
            else
            {
                OD = (tubeOrientation == TubeOrientation.FlatToZero ? Dff : Dpp);
            }

            // ** If coped arm, add clearance and done!
            if (!nonCoped)
            {
                return OD + 0.125;
            }

            // ** Compute bend radius
            //inR = Rounding.RoundUp(tb * factor, 0.125);
            inR = (tb * factor).RoundUp(0.125);
            outR = inR + tb;

            // ** compute h
            double d = outR - maxGap;
            double h = outR * outR - d * d; // distance along bend radius to obtain a gap = maxGap
            h = Math.Sqrt(h);

            double width = OD + 2 * (inR - h - ts);
            //width = Rounding.RoundUp(width, 1.0);

            return width;

        }

        /// <summary>
        /// Compute the minimum height required for the bracket.
        /// Irregular polygon shapes are considered (ie FWT hex arms)
        /// </summary>
        /// <returns>Return minimum bracket height in inches.</returns>
        private double ComputeMinimumBracketHeight()
        {
            double minHt1 = 2.0 * (_bracketBoltGroup.MaxDistToCenterLine + BracketMinEdgeDistance);
            double minHt2 = 0;

            // ** Check the width vs height for an irregular polygon (ie hex arm)
            if (!_arm.Shape.IsRegularPolygon)
            {
                if (GetPolygonWidthAndHeight(_arm, out double minBktWidth, out double minBktHt))
                {
                    minHt2 = minBktWidth + BracketMinEdgeDistance;
                }
            }

            double minHt = Math.Max(minHt1, minHt2);

            return minHt;

        }

        /// <summary>
        /// Get the width and height of an irregular polygon (FWT shape).
        /// Returns TRUE if the height factor was found, otherwise, FALSE.
        /// Height factor = the hex height factor. EG. hex1: factor = 2,
        /// total height for hex1 = Dff * factor.
        /// </summary>
        /// <param name="arm">The davit arm</param>
        /// <param name="minBktWidth">The minimum bracket width in inches.</param>
        /// <param name="minBktHt">The minimum bracket height in inches.</param>
        /// <returns>
        /// Returns TRUE if the height factor was found, otherwise, FALSE
        /// </returns>
        /// <remarks>
        /// Compute the angle of the arm to get the mitered height required.
        /// NOTE: this method also appears in SaddleBracketData.GetPolygonWidthAndHeight
        /// at ~line 365.  Placed here to put all related methods in one place.
        /// </remarks>
        private static bool GetPolygonWidthAndHeight(TubularArm arm, out double minBktWidth, out double minBktHt)
        {
            int nSides = arm.Shape.SideCount;
            double angle = Math.PI / nSides;
            double cosAng = Math.Cos(angle);

            double armAngle = Math.Atan(arm.TipVerticalOffset / (arm.PLSLengthFt * 12.0));

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
            //minBktHt = Rounding.RoundUp(minBktHt, 0.5);

            return true;
        }

        #endregion  // Methods

        #region Methods - check bracket results

        /// <summary>
        /// Is the bracket adequate?
        /// That is, are the bracket stresses and W/T within the proper limits.
        /// </summary>
        /// <returns>
        /// Returns TRUE if no warnings were found, otherwise FALSE
        /// </returns>
        private bool CheckBracketResults()
        {
            CheckAnalysisResults();

            //CheckWoverT();

            // ** Are there any warnings??
            //int L = WarningMessages.Length;
            int L = WarningMessages.Count;
            bool noWarnings = (L < 6);

            //if (!noWarnings) 
            //{
            //    _bracket.AnalysisWarningMessages = WarningMessages.ToString();
            //} 

            _arm.Parent.AnalysisWarningMessages.AddRange(WarningMessages);

            return noWarnings;

        }


        /// <summary>
        /// Check for over stresses.  Returns 
        /// </summary>
        /// <returns></returns>
        private void CheckAnalysisResults()
        {

            // ** Check W/t
            double WoverT = _bracket.BracketOpening / _bracket.BracketThick;
            double factor = (_bracket.StiffenerQty == 0 ? 24.0 : 28.0);

            bool wtDoesntwork = (WoverT > factor);

            // ** Check for over stresses.
            //bool chkThickness = BracketThicknessStressKsi.IsOverstressed;
            bool chkLegStress = BracketLegStressKsi.IsOverstressed;
            bool chkShearRupture = BracketShearRuptureKsi.IsOverstressed;
            bool chkBearingStress = BracketBearingKsi.IsOverstressed;

            // ** No over stresses.  Nothing else to do.
            if (!chkLegStress && !chkShearRupture && !chkBearingStress && !wtDoesntwork)
            {
                return;
            }

            //if (chkThickness)
            //{
            //WarningMessages.AppendLine($"\t{BracketThicknessStressKsi.ResultsMsg}");
            //}

            //if (chkLegStress)
            //{
            WarningMessages.Add(new Message(MessageCategory.Warn, $"\t{BracketLegStressKsi.ResultsMsg}"));
            //WarningMessages.AppendLine($"\t{BracketLegStressKsi.ResultsMsg}");
            //}

            //if (chkShearRupture)
            //{
            WarningMessages.Add(new Message(MessageCategory.Warn, $"\t{BracketShearRuptureKsi.ResultsMsg}"));
            //WarningMessages.AppendLine($"\t{BracketShearRuptureKsi.ResultsMsg}");
            //}

            //if (chkBearingStress)
            //{
            WarningMessages.Add(new Message(MessageCategory.Warn, $"\t{BracketBearingKsi.ResultsMsg}"));
            //WarningMessages.AppendLine($"\t{BracketBearingKsi.ResultsMsg}");
            //}

            //if (itDoesntwork)
            //{
            if(wtDoesntwork)
            {
                WarningMessages.Add(new Message(MessageCategory.Warn, $"\tBracket W/t is too high: W/t = {WoverT:f3} should be <= {factor}."));
            }
            
            //WarningMessages.AppendLine($"\tBracket W/t {msg} : W/t = {WoverT:f3} should be <= {factor}.");
            //}
        }

        //    /// <summary>
        //    /// Check the W / t of the bracket.
        //    /// W/t is ok if:
        //    ///     With stiffeners:    W/t &le; 30
        //    ///     Without stiffeners: W/t &le; 24 
        //    ///     
        //    /// </summary>
        //    private void CheckWoverT()
        //{
        //    double WoverT = _bracket.BracketOpening / _bracket.BracketThick;
        //    double factor = (_bracket.StiffenerQty == 0 ? 24.0 : 30.0);
        //    //bool withStiff = (WoverT > 30.0) && (_bracket.StiffenerQty > 0);
        //    //bool withoutStiff = (WoverT > 24.0) && (_bracket.StiffenerQty == 0);

        //    //bool itDoesntwork = withStiff || withoutStiff;
        //    bool itDoesntwork = (WoverT > factor);

        //    if (itDoesntwork)
        //    {
        //        WarningMessages.AppendLine($"\tBracket W/T is too high!!: W/t = {WoverT:f3} should be < {factor}.");
        //    }

        //}

        #endregion

    }
}
