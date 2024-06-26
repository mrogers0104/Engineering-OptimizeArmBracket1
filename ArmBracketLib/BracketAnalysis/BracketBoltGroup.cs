using System;
using System.Collections.Generic;
using System.Linq;
using ArmBracketDesignLibrary.Helpers;
using MaterialsLibrary;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using NLog;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    /// <summary>
    /// Calculate bracket bolt parameters
    /// </summary>
    public class BracketBoltGroup
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private TubularArm _arm = null;
        private SaddleBracket _bracket = null;

        private List<BracketBolt> _bracketBolts = new List<BracketBolt>();

        private BracketLoad _bracketLoad;
        double _Ag = 0;
        double _totalAg = 0;
        int _qtyB = 0;
        double _Fu = 0;         // ultimate strength of bolt, ksi

        private double _thruPlateOpening = 0;   // W
        private double _momentLb = 0;       // MLb
        private double _momentVb = 0;       // Mvb
        private double _momentTb = 0;       // MTb


        #region Constructors

        public BracketBoltGroup(TubularArm arm, SaddleBracket bkt, BracketLoad bracketLoad)
        {
           
            _arm = arm;
            _bracket = bkt;
            _bracketLoad = bracketLoad;

            ThreadsExcluded = !_bracket.BoltThreadsInShearPlane;

            if (_bracket.BoltDiameter <= 0)
            {
                logger.Error($"BracketBolt diameter NOT defined: {_bracket.BoltDiameter}\" dia for spec {_bracket.BoltSpec}");
                return;     // may cause problems from the caller!!!
            }

            string boltSpec = _bracket.BoltSpec.ToString();
            boltSpec = boltSpec.Contains("354BC") ? boltSpec.Replace("BC", "-BC") : boltSpec;
            ICommonBolt myBolt = MaterialsLibraryBO.GetStructuralBolt((decimal)_bracket.BoltDiameter, boltSpec, includeInactive: false);

            _Fu = myBolt.FuKsi;

            _Ag = myBolt.GrossArea;
            _qtyB = _bracket.BoltQty;
            _totalAg = _Ag * _qtyB * 2; // two legs on the bracket

            ThruPlateConnection thruPlate = (ThruPlateConnection)_bracket.Parent;
            _thruPlateOpening = thruPlate.ThruPlateOpening; // W

            _momentLb = bracketLoad.HorizontalMoment;
            _momentVb = bracketLoad.VerticalMoment;
            _momentTb = bracketLoad.Torsion;

            SetupBolts();
        }

        #endregion  // Constructors


        #region Properties


        public double Ix { get { return ComputeMomentOfInertiaX(); } }

        public double Iy
        {
            get
            {
                double iy = 4.0 * _Ag * SumDistSqToCenterLine;

                return iy;
            }

        }

        public double Iz { get { return Ix; } }

        public double BoltStressTransKsi { get { return ComputeBoltStressTrans(); } }

        public double BoltStressVertKsi { get { return ComputeBoltStressVertical(); } }

        /// <summary>
        /// Maximum distance to the outer bolt from the bracket center line, in.
        /// </summary>
        public double MaxDistToCenterLine
        {
            get
            {
                double dist = _bracketBolts.Max(b => b.DistFromCenter);
                return dist;
            }
        }

        /// <summary>
        /// The sum of the square of d (distance to bolt from center line of bracket)
        /// </summary>
        public double SumDistSqToCenterLine
        {
            get
            {
                double sumDsq = _bracketBolts.Sum(b => b.DistSqFromCenter);
                return sumDsq;
            }
        }

        public double SumShearStressXKsi
        {
            get
            {
                double sum = _bracketBolts.Sum(b => b.ShearStressXksi);

                return sum;
            }
        }

        public double SumShearStressZKsi
        {
            get
            {
                double sum = _bracketBolts.Sum(b => b.ShearStressZksi);

                return sum;
            }
        }

        /// <summary>
        /// Maximum bolt shear, ksi
        /// </summary>
        public double MaxCombinedShearKsi
        {
            get
            {
                double val = _bracketBolts.Max(b => b.ShearCombinedStressKsi);

                return val;
            }
        }

        /// <summary>
        /// Sum of the shear force for all bolts, kips
        /// </summary>
        public double SumShearForceKips
        {
            get
            {
                double sum = _bracketBolts.Sum(b => b.ShearForceKips);

                return sum;
            }
        }

        /// <summary>
        /// Sum of the bolt moments for all bolts, ft-kips
        /// </summary>
        public double SumMomentFtKip
        {
            get
            {
                double sum = _bracketBolts.Sum(b => b.MomentFtKips);

                return sum;
            }
        }

        /// <summary>
        /// Sum of the bracket moment for all bolts, ft-kips
        /// </summary>
        public double SumBracketMomentFtKip
        {
            get
            {
                double sum = _bracketBolts.Sum(b => b.BracketMomentFtKips);

                return sum;
            }
        }

        /// <summary>
        /// Maximum shear force in the bolts, kips
        /// </summary>
        public double MaxShearForceKips { get { return MaxCombinedShearKsi * _Ag; } }

        /// <summary>
        /// Shear capacity of the bolt, ksi
        /// </summary>
        public double ShearCapacity
        {
            get
            {
                double cap = _Fu * (ThreadsExcluded ? 0.45 : 0.35);
                return cap;
            }
        }

        /// <summary>
        /// Are the bolt threads excluded from the shear plane?
        /// </summary>
        public bool ThreadsExcluded { get; set; }

        /// <summary>
        /// The interaction ratio for this bolt
        /// </summary>
        public double InteractionRatio { get; set; }

        /// <summary>
        /// The interaction ratio as a percentage.
        /// </summary>
        public string Ratio
        {
            get
            {
                double ratio = InteractionRatio * 100.0;
                return $"{ratio:f2}%";
            }

        }

        /// <summary>
        /// Are any of the bolts for this load case over stressed?
        /// </summary>
        public bool BoltIsOverstressed { get { return InteractionRatio > 1.0; } }

        #endregion  // Properties

        #region Methods

        private void SetupBolts()
        {
            _bracketBolts.Clear();

            int I = 0;
            int j = _bracket.GetNumberBktQuadBolts();
            int qty = _bracket.BoltQty;

            // ** First, need to compute the distance to the bot from the centerline of bracket
            if (qty % 2 != 0)   // bolt on bracket center line (odd number of bolts)
            {
                BracketBolt bracketBolt = new BracketBolt(I, _bracket);
                _bracketBolts.Add(bracketBolt);
                I = 1;
            }
            for (int i = I; i < j; i++)
            {
                BracketBolt bracketBolt = new BracketBolt(i, _bracket);
                _bracketBolts.Add(bracketBolt);
            }

            // ** Second, compute bolt force and stresses given the bolt location.
            foreach (var bolt in _bracketBolts)
            {
                ComputeBoltShearStressX(bolt.DistFromCenter, out double stress, out double force);
                bolt.ShearStressXksi = stress;
                bolt.ShearForceKips = force;

                bolt.ShearStressZksi = ComputeBoltShearStressZ();

                bolt.MomentFtKips = ComputeBoltMoment(force);
            }
        }

        /// <summary>
        /// Compute bolt shear stress Z for a bolt at 'd'
        /// </summary>
        /// <param name="d"></param>
        /// <param name="stress">Returns the stress in ksi</param>
        /// <param name="force">Returns the shear force in kips</param>
        private void ComputeBoltShearStressX(double d, out double stress, out double force)
        {
            double p1 = 12.0 * _momentLb * _thruPlateOpening / 2.0 / Iz;
            p1 += BoltStressTransKsi;

            double p2 = 12.0 * _momentVb * d / Iy;

            stress = p1 + (_qtyB == 1 ? 0 : p2);

            force = stress * _Ag;
        }

        /// <summary>
        /// Compute bolt shear stress Z for a bolt.
        /// </summary>
        /// <returns>Return the stress in ksi</returns>
        private double ComputeBoltShearStressZ()
        {
            double p1 = 12.0 * _momentTb * _thruPlateOpening / 2.0 / Ix;
            p1 += BoltStressVertKsi;

            return p1;
        }

        /// <summary>
        /// Compute the bolt moment for a bolt.
        /// </summary>
        /// <param name="shearForce"></param>
        /// <returns></returns>
        private double ComputeBoltMoment(double shearForce)
        {
            double Tarm = BktSideWidth();
            double Marm = MomentArm();
            double fvi = shearForce;

            double num = fvi * Marm * (3.0 * Marm + 2.0 * Tarm);
            double den = 2.0 * (3.0 * Marm + Tarm);
            double moment = num / den;

            return moment;
        }

        /// <summary>
        /// Bolt stress translational, ksi
        /// </summary>
        /// <returns></returns>
        private double ComputeBoltStressTrans()
        {
            double fTb = _bracketLoad.VerticalShear;

            double stress = fTb / _totalAg;

            return stress;
        }

        /// <summary>
        /// Bolt stress vertical, ksi
        /// </summary>
        /// <returns></returns>
        private double ComputeBoltStressVertical()
        {
            double fVb = _bracketLoad.AxialForce;

            double stress = fVb / _totalAg;

            return stress;
        }

        /// <summary>
        /// Compute the moment of inertia of the bolts about x
        /// Note: Iz = Ix
        /// </summary>
        /// <returns></returns>
        private double ComputeMomentOfInertiaX()
        {
            double qty = _bracket.BoltQty / 2;
            ThruPlateConnection thruPlate = (ThruPlateConnection)_bracket.Parent;
            double W = thruPlate.ThruPlateOpening;
            double Ix = _totalAg * Math.Pow(W / 2.0, 2);

            return Ix;

        }

        /// <summary>
        /// The distance from the bracket bolt to the face of the bracket (arm butt), in.
        /// [[refers to Tarm = max(A + Tb), 3.0)]]
        /// </summary>
        /// <returns></returns>
        private double BktSideWidth()
        {
            double sideWidth = _bracket.BracketSideWidth;
            return Math.Max(sideWidth / 2.0, 3.0);
        }

        /// <summary>
        /// The bolt moment arm
        /// </summary>
        /// <returns>Return bolt moment arm in inches</returns>
        private double MomentArm()
        {
            double armFlatDiameter = _arm.BaseFlatWidthInches;   // Dff
            double armThickness = _arm.MaterialThicknessInches;  // Ta
            int n = _arm.Shape.SideCount;

            ThruPlateConnection connection = (ThruPlateConnection)_bracket.Parent;
            double W = connection.ThruPlateOpening;
            double Tb = _bracket.BracketThick;

            double angle = Math.PI / n;
            double fw = armFlatDiameter * Math.Tan(angle);

            double Dff = armFlatDiameter;
            double Dpp = Dff / Math.Cos(angle);

            double Td1 = (W + Tb - Dff) / 2.0;
            double p = Dpp - fw / 2.0 * Math.Sin(angle);
            double Td2 = (W + Tb - p) / 2.0;

            if (n == 6) // Hexagonal arm
            {
                Td2 = _arm.Shape.TubeOrientation == TubeOrientation.FlatToZero ? Td2 : 99;
            }
            else
            {
                Td2 = _arm.Shape.TubeOrientation == TubeOrientation.PointToZero ? Td2 : 99;
            }
            double momentArm = Math.Min(Td1, Td2);

            return momentArm;
        }


        #endregion  // Methods

        #region Results Methods

        /// <summary>
        /// Get the bracket bolts for this load.
        /// </summary>
        /// <returns></returns>
        public List<BracketBolt> GetBracketBolts()
        {
            return _bracketBolts;
        }

        public override string ToString()
        {
            string p = _bracketBolts.Count == 1 ? string.Empty : "s";
            string msg = $"{_bracketBolts.Count} bolt{p} defined for bracket {_bracket.Label}";
            return msg;
        }

        #endregion
    }
}
