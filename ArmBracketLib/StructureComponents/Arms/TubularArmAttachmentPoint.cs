using System;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;
using Newtonsoft.Json;

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{

    /// <summary>
    /// Represents a single attachment to the pole at a specific height and azimuth
    /// </summary>
    public abstract class TubularArmAttachmentPoint
    {
        //        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected double _boltMidSpacing;
        protected double _boltSideEdgeDistance;
        protected double _boltSpacing;
        protected StructuralBolts.StructuralBoltGrades _boltSpec = StructuralBolts.StructuralBoltGrades.None;
        protected PlateMaterialSpecification _bracketMaterialSpec = PlateMaterialSpecification.None;

        protected PlateFinish _finish = PlateFinish.None;

        protected double _materialStrength;
        private double _boltLength;

        #region Constructors

        private TubularArmAttachmentPoint() { }

        public TubularArmAttachmentPoint(ArmConnection parent, Guid id)
        {
            Id = id;
            Parent = parent;
        }

        #endregion  // Constructors

        #region Properties

        //public Guid ArmId { get; set; }

        public double Azimuth { get; set; }

        public virtual double BoltDiameter { get; set; }

        public double BoltLength
        {
            get
            {
                if (_boltLength.IsZero())
                {
                    return calcMinBoltLength();

                    double calcMinBoltLength()
                    {
                        if ((ThruPlateConnection)Parent != null)
                        {
                            double minLength = BracketThick;
                            minLength += ((ThruPlateConnection)Parent).Thickness;
                            minLength += BoltDiameter;
                            if (BoltDiameter > 0)
                            {
                                minLength += StructuralBolts.GetFlatWasherHeight(BoltSpec, BoltDiameter) * 2;
                            }
                            minLength += .5; // lengths are 1/4" too short according to Hocks Standard Bracket Design spreadsheet (11.07.17)
                                             // minLength += .25; //Minimum extra length past nut.
                                             //NOTE: Hardcode length rounded to .25 inch. Doubtful that we will ever use a big enough bolt that .25 inch increments aren't available
                            double nearest = minLength < 6.0 ? 0.25 : 0.50;
                            minLength = minLength.RoundUp(nearest);

                            return minLength;
                        }
                        return _boltLength;
                    }
                }
                return _boltLength;
            }

            set
            {
                _boltLength = value;
            }
        }

        public virtual double BoltMidSpacing
        {
            get { return _boltMidSpacing > 0 ? _boltMidSpacing : StructuralBolts.GetMinimumBoltSpacing(BoltDiameter); }
            set { _boltMidSpacing = value; }
        }

        public BracketBoltPattern BoltPattern { get; set; }

        /// <summary>
        /// Bolt quantity in one leg of the bracket. (!!! NOT the total number of bolts !!!)
        /// </summary>
        public virtual int BoltQty { get; set; }

        public virtual double BoltSideEdgeDistance
        {
            get
            {
                //if (DoubleUtil.isZero(Math.Abs(_boltSideEdgeDistance)))
                if (Math.Abs(_boltSideEdgeDistance).IsZero())
                {
                    return StandardBoltEdgeDistance;

                }

                return _boltSideEdgeDistance;
            }

            set => _boltSideEdgeDistance = value;
        }

        public virtual double BoltSpacing
        {
            get => _boltSpacing.IsZero() ? StructuralBolts.GetMinimumBoltSpacing(BoltDiameter) : _boltSpacing;
            set => _boltSpacing = value;
        }

        public double BoltSpacingFromFrontPlate => Utils.GetBoltSpacingFromFrontPlate(BracketThick);

        public StructuralBolts.StructuralBoltGrades BoltSpec
        {
            get
            {
                if (_boltSpec != StructuralBolts.StructuralBoltGrades.None)
                {
                    return _boltSpec;
                }

                if (BoltDiameter > 1.5)
                {
                    return StructuralBolts.StructuralBoltGrades.A354BC;
                }

                return StructuralBolts.StructuralBoltGrades.A325;
            }
            set { _boltSpec = value; }
        }

        public abstract double BoltWeight { get; set; }

        public PlateMaterialSpecification BracketMaterialSpecification
        {
            get
            {
                if (_bracketMaterialSpec == PlateMaterialSpecification.None)
                {
                    return PlateMaterial.GetStandardPlateMaterialSpecification(Finish, BracketThick);
                }

                return _bracketMaterialSpec;
            }
            set { _bracketMaterialSpec = value; }
        }

        public virtual double BracketSideWidth { get; set; }
        public virtual double BracketThick { get; set; }
        public BracketType BracketType
        {
            get
            {
                if (BracketThick > 1.5)
                {
                    return BracketType.ThreePiece;
                }

                return BracketType.Bent;
            }
        }

        public abstract double BlackWeight { get; }

        public virtual ArmConnectionMethod ConnectionType
        {
            get
            {
                return ArmConnectionMethod.Fixed;
            }
        }

        public BracketDesignMethod DesignMethodSpecified { get; set; } = BracketDesignMethod.STND;

        public BracketDesignMethod DesignMethodUsed { get; set; } = BracketDesignMethod.UNKN;

        public PlateFinish Finish { get; set; }

        public virtual double Height { get; set; }

        public Guid Id { get; set; }

        public string Label { get; set; }

        public virtual double MaterialStrength
        {
            get
            {
                if (_materialStrength > 0)
                {
                    return _materialStrength;
                }

                return PlateMaterial.GetMaterialFyKsi(BracketMaterialSpecification);
            }
            set => _materialStrength = value;
        }

        public virtual double MinimumSideWidth { get; set; }

        //public BracketOrientation Orientation { get; set; }

        [JsonIgnore]
        public ArmConnection Parent { get; set; }

        public virtual string StdBracketID { get; set; }

        public virtual double BracketOpening { get; set; }

        public virtual int StiffenerQty { get; set; }

        public virtual double StiffenerThick { get; set; }

        public virtual double StiffenerVertSpacing { get; set; }

        public virtual double StiffenerLength { get; set; }

        public virtual double StiffenerWidth { get; set; }

        /// <summary>
        /// The computed stiffener black weight in lbs.
        /// </summary>
        public virtual double StiffenerWeightLbs { get; set; }

        public virtual PlateMaterialSpecification ThruPlateSpec { get; set; }

        internal double ThruPlateThick
        {
            get
            {
                return ((ThruPlateConnection)Parent)?.Thickness ?? 0;
            }
        }

        public double ThruPlateWidth { get; set; }

        public double ThruPlateBlackWeight { get; set; }

        private double MinimumSideWidthNewRadius => Utils.GetMinimumSideWidthNewRadius(BracketThick);

        private double StandardBoltEdgeDistance
        {
            get
            {
                //                logger.Trace("Bolt Diam: {0}", BoltDiameter);

                if (BoltDiameter.IsZero())
                {
                    return 0;
                }

                int boltRowCount = 1;
                switch (BoltPattern)
                {
                    case BracketBoltPattern.SingleRow:
                        return StructuralBolts.GetBoltMinimumEdgeDistance(BoltDiameter);
                }

                double steelManualMinimumEdgeDistance = StructuralBolts.GetBoltMinimumEdgeDistance(BoltDiameter);
                double spacingFromFrontPlate = BoltSpacingFromFrontPlate;
                double minimumSideWidthStdRadius = spacingFromFrontPlate + 2 * steelManualMinimumEdgeDistance +
                                                   (boltRowCount > 0
                                                        ? StructuralBolts.GetMinimumBoltSpacing(BoltDiameter) *
                                                          boltRowCount - 1
                                                        : 0);
                double minimumSideWidthNewRadius = MinimumSideWidthNewRadius;

                return Math.Max(steelManualMinimumEdgeDistance,
                                steelManualMinimumEdgeDistance +
                                (minimumSideWidthNewRadius - minimumSideWidthStdRadius) / 2);
            }
        }

        protected double SideWidth { get; set; }

        #endregion  // Properties

        //public abstract void InitializeDimensions(TubularArm arm);
    }
}