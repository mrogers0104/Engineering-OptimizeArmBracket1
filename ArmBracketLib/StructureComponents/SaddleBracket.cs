using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using ArmBracketDesignLibrary.BracketAnalysis;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using ArmBracketDesignLibrary.StructureComponents.Data;
using ArmBracketDesignLibrary.Vectors;
using MaterialsLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmBracketDesignLibrary.StructureComponents
{
    [Serializable]
    public class SaddleBracket : TubularArmAttachmentPoint
    {
        //        private static Logger logger = LogManager.GetCurrentClassLogger();

        private double _boltWeight;

        private double _maximumBoltOffset;

        public SaddleBracket(ArmConnection parent, Guid guid) : base(parent, guid)
        {
        }

        //public SaddleBracket(ArmConnection parent) : this(parent, Guid.NewGuid())
        //{
        //}

        private double stdBracketBlackWt;

        public SaddleBracket(ArmConnection parent, StdBracketDataItem stdBracket, ArmProject project) : this(parent, Guid.NewGuid())
        {
            IsStandardBracket = true;
            stdBracketBlackWt = stdBracket.BlackWeight;
            DesignMethodSpecified = BracketDesignMethod.STND;

            BracketMaterialSpecification = project.BracketMaterialSpecification;
            Finish = project.BracketFinish;

            ThruPlateSpec = BracketMaterialSpecification;
            BracketThick = stdBracket.BracketThick;
            BracketOpening = stdBracket.BracketOpening;

            ((ThruPlateConnection)parent).ThruPlateOpening = stdBracket.BracketOpening - 0.125;
            ((ThruPlateConnection)parent).Thickness = stdBracket.ThruPlateThick;

            Height = stdBracket.Height;

            StiffenerQty = stdBracket.StiffenerQty;
            StiffenerThick = stdBracket.StiffenerThick;
            StiffenerVertSpacing = stdBracket.StiffenerVertSpacing;
            StiffenerWidth = stdBracket.StiffenerWidth;
            BoltQty = stdBracket.BoltQty / 2;
            BoltSpec = stdBracket.BoltSpec;
            BoltDiameter = stdBracket.BoltDiameter;
            BoltLength = stdBracket.BoltLength;
            StdBracketID = stdBracket.BracketID;

            BoltSpacing = stdBracket.BoltSpacing;

            // !! Important to set min. edge distance for std brackets
            StandardBracketMinEdgeDist = stdBracket.MinEdgeDist;

            BracketSideWidth = stdBracket.BracketSideWidth;
            BracketSideWidth = (BracketSideWidth <= 0 ? ComputeBracketSideWidth() : BracketSideWidth);

            ThruPlateWidth = calcThruPlateWidth();

            ThruPlateBlackWeight = (double)MyExtensions.ThruPlateBlackWt(stdBracket.ThruPlateThick, ThruPlateWidth, Height);

            double calcThruPlateWidth()
            {
                // these magic number calcs came from the old backend, used here to match QT for UA testing
                double poleFlatWidth = (Height / (2 * 12)) * (double)project.PoleTaper + (double)project.PoleOuterDiameterIn;
                double poleWidth = BracketsOnFlats()
                                  ? poleFlatWidth
                                  : RegularPolygon.GetPointDiameterAdjustedForRoundCorners(project.PoleSideCount, poleFlatWidth, (double)project.PoleWallThickness);

                double thruPlateWidth = Math.Truncate((poleWidth / 2) + 0.51 + 0.99) * 2.0 + (BoltSideEdgeDistance * 2d * project.ArmsAtLocation);
                return thruPlateWidth;
            }

            bool BracketsOnFlats()
            {
                // *** On a thru plate, the arms must be 180 degrees from each other, so we can figure out
                // *** if it is on a flat from just one azimuth
                TubularArm arm = project.TubularArm;
                //TubularArm arm = (project.TubularDavitArm == null ? project.TubularXArm : project.TubularDavitArm);

                double az = arm.Azimuth; //  project.TubularDavitArm.Azimuth;
                double currentAngle = 0;
                int sideCount = project.PoleSideCount;

                while (currentAngle < 360)
                {
                    if ((decimal)az == (decimal)Math.Round(currentAngle, 4))
                    {
                        return true;
                    }

                    currentAngle += 360 / (double)sideCount;
                }

                return false;
            }
        }

        public SaddleBracket(ThruPlateConnection parent, SaddleBracketDTO dtoBracket, ArmProject project) : this(parent, Guid.NewGuid())
        {
            IsStandardBracket = false;
            DesignMethodSpecified = BracketDesignMethod.OVRD;
            DesignMethodUsed = BracketDesignMethod.OVRD;
            Finish = project.BracketFinish;
            BracketMaterialSpecification = project.BracketMaterialSpecification;

            ThruPlateSpec = BracketMaterialSpecification;
            BracketThick = dtoBracket.BracketThick;
            BracketOpening = dtoBracket.BracketOpening;

            ((ThruPlateConnection)parent).ThruPlateOpening = dtoBracket.BracketOpening - 0.125;
            ((ThruPlateConnection)parent).Thickness = dtoBracket.ThruPlateThick;

            ThruPlateWidth = dtoBracket.ThruPlateWidth;
            Height = dtoBracket.BracketHeight;

            ThruPlateBlackWeight = (double)MyExtensions.ThruPlateBlackWt(dtoBracket.ThruPlateThick, ThruPlateWidth, Height);

            StiffenerQty = dtoBracket.StiffenerQty;
            StiffenerThick = dtoBracket.StiffenerThick;
            StiffenerVertSpacing = dtoBracket.StiffenerVertSpacing;
            StiffenerWidth = dtoBracket.StiffenerWidth;

            BoltQty = dtoBracket.BoltQty; // / 2;

            StructuralBolts.StructuralBoltGrades boltGrade = StructuralBolts.StructuralBoltGrades.None;
            Enum.TryParse(dtoBracket.BoltSpec, true, out boltGrade);
            BoltSpec = boltGrade;
            BoltDiameter = dtoBracket.BoltDiameter;
            BoltLength = dtoBracket.BoltLength;

            StdBracketID = dtoBracket.StdBracketID;
            //StdBracketID = string.Empty; // dtoBracket.BracketID;

            BoltSpacing = dtoBracket.BoltSpacing;

            // !! Important to set min. edge distance for std brackets
            BoltTopEdgeDistance = dtoBracket.BoltTopEdgeDistanceInch;
            StandardBracketMinEdgeDist = 0;

            // *** Set Bracket side width.  If it's 0, calculate the side width.
            // *** The side width must have a value > 0 before continuing.
            BracketSideWidth = dtoBracket.BracketSideWidth;
            BracketSideWidth = (BracketSideWidth <= 0 ? ComputeBracketSideWidth() : BracketSideWidth);
        }

        /// <summary>
        /// Returns a list of the locations of all the bolts, in X and Y coordinates, with the origin at the middle of the end of the bracket facing the pole.
        ///
        ///     ________________________________
        ///    |                               |
        ///    |                               |   ^
        ///    |                               |   |
        ///    |                    O          | Y pos
        ///    |                               |
        ///    |                               |
        ///    |                               |
        ///    |                               |
        ///    |                               |
        ///    |                      <-Xneg   |<- origin  Xpos->
        ///    |                               |
        ///    |                               |
        ///    |                               |
        ///    |                               |
        ///    |                               |
        ///    |                    O          | Y Neg
        ///    |                               |   |
        ///    |                               |   V
        ///    |_______________________________|
        ///
        ///                         Pole ->
        ///
        /// </summary>
        ///
        [JsonIgnore]
        public List<Vector2> BoltLocations
        {
            get
            {
                List<Vector2> boltLocations = ArmBracketDesignBoltLocations();

                return boltLocations;
            }
        }

        /// <summary>
        /// Compute the bolt locations for the Arm Bracket Design (Hock's spreadsheet)
        /// </summary>
        /// <returns>Return list of bolt locations</returns>
        private List<Vector2> ArmBracketDesignBoltLocations()
        {
            List<Vector2> boltLocations = new List<Vector2>();

            int j = this.GetNumberBktQuadBolts();
            double c = this.GetBktBoltCenterSpace(); // c
            double s = this.BoltSpacing; // s
            double e = this.BoltSideEdgeDistance;

            double x = -e;
            double y = c;
            for (int i = 0; i < j; i++)
            {
                boltLocations.Add(new Vector2(x, y));
                boltLocations.Add(new Vector2(x, -y));

                y += s;
            }

            if (BoltQty % 2 != 0)
            {
                boltLocations.Add(new Vector2());   // add bolt at origin for odd number of bolts
            }

            return boltLocations;
        }

        public bool BoltThreadsInShearPlane { get; set; }

        private double _boltTopEdgeDistance = 0;

        /// <summary>
        /// The edge distance to the first bolt from the top or bottom of the bracket, in.
        /// </summary>
        public double BoltTopEdgeDistance
        {
            get
            {
                string boltSpec = BoltSpec.ToString();
                boltSpec = boltSpec.Contains("354BC") ? boltSpec.Replace("BC", "-BC") : boltSpec;
                StructuralBolt_ML myBolt = MaterialsLibraryBO.GetStructuralBolt((decimal)BoltDiameter, boltSpec.ToString(), includeInactive: false);

                double minEdge = myBolt.BktBoltEdgeDistance;

                if (!StandardBracketMinEdgeDist.IsZero()) //   DoubleUtil.isZero(StandardBracketMinEdgeDist))
                {
                    return StandardBracketMinEdgeDist;
                }
                else if (!_boltTopEdgeDistance.IsZero()) //  !DoubleUtil.isZero(_boltTopEdgeDistance))
                {
                    return _boltTopEdgeDistance;
                }

                return minEdge;     // minimum top and bottom edge distance by bolt size
            }

            set => _boltTopEdgeDistance = value;
        }

        public bool IsStandardBracket { get; set; }

        public override double BoltWeight
        {
            get
            {
                //if (DoubleUtil.isZero(_boltWeight))
                if (_boltWeight.IsZero())
                {
                    return calcBoltWeight();
                }
                return _boltWeight;

                double calcBoltWeight()
                {
                    double weight = StructuralBolts.GetBoltHeadAndNutWeight(BoltDiameter) * BoltQty * 2;
                    weight += StructuralBolts.GetBoltShaftWeightPerInch(BoltDiameter) * BoltQty * 2 *
                                BoltLength;
                    //(ThruPlateThick + BracketThick + 1);
                    weight = weight.RoundUp(1.0);
                    return weight;
                }
            }

            set => _boltWeight = value;
        }

        /// <summary>
        /// The distance from the bracket face (Arm butt) to the center of the bolt hole, in.
        /// </summary>
        public override double BracketSideWidth
        {
            //get { return DoubleUtil.isZero(_sideWidth) ? MinimumSideWidth : _sideWidth; }
            get => SideWidth.IsZero() ? MinimumSideWidth : SideWidth;
            set => SideWidth = value;
        }

        /// <summary>
        /// The bracket weight in lbs.
        /// This weight includes galvanizing and stiffeners(gussets).
        /// </summary>
        public override double BlackWeight
        {
            get
            {
                if (IsStandardBracket)
                {
                    return stdBracketBlackWt;
                }

                return calcWeight();

                double calcWeight()
                {
                    //galvRate += Finish == PlateFinish.Galvanized ? Globals.GalvanizingRate : 0.0;

                    // ** Bracket weight
                    double holeDia = BoltDiameter + 0.125;
                    double boltHoleArea = Math.PI * holeDia * holeDia / 4.0;
                    double holeVol = boltHoleArea * BracketThick * BoltQty * 2;
                    double edge = GetBoltEdgeDist();
                    double front = BracketOpening - 2 * 3 * BracketThick + Math.PI * 3.45 * BracketThick + (4 * edge + 2 * 0.75);

                    // ** Gusset weight
                    StiffenerLength = BracketOpening;
                    StiffenerWidth = (StiffenerThick > 0 && StiffenerQty > 0 && StiffenerWidth == 0 ?
                        Math.Max(0.75, StiffenerThick) * 3.0 : StiffenerWidth);
                    StiffenerQty = StiffenerThick > 0 && StiffenerQty <= 0 ? 2 : StiffenerQty; // default to 2 if qty is not defined
                    double stifVol = BracketOpening * StiffenerWidth * StiffenerThick * StiffenerQty;

                    StiffenerWeightLbs = Math.Round(stifVol.CubicInchesToCubicFeet() * Globals.SteelDensityPerCuFt, 2);

                    double volume = BracketThick * front * Height + stifVol - holeVol;

                    volume = volume.CubicInchesToCubicFeet();

                    return Math.Round(volume * Globals.SteelDensityPerCuFt, 2);
                }
            }
        }

        /// <summary>
        /// Get the bolt edge distance. Used to calculate the bracket volume/wt.
        /// </summary>
        /// <returns></returns>
        private double GetBoltEdgeDist()
        {
            Dictionary<double, double> edgeDist = new Dictionary<double, double>()
            {
                {1, 1.25 },
                {1.25, 1.5 },
                {1.75, 1.875 },
                {2.00, 2.00 },
                {2.25, 2.375 },
                {2.50, 2.625 },
                {2.75, 2.750 },
                {3.00, 3.000 },
                {3.25, 3.250 },
                {999.0, 3.5 }
            };

            double edge = (from e in edgeDist where BoltDiameter < e.Key select e.Value).FirstOrDefault();
            return edge;
        }

        public double GalvWeight
        {
            get
            {
                return Finish == PlateFinish.Galvanized ? BlackWeight * (1d + Globals.GalvanizingRate) : 0d;
            }
        }

        public double MaximumBoltOffset
        {
            get
            {
                if (_maximumBoltOffset.IsZero())
                {
                    return calcMaxBoltOffset();
                }

                return _maximumBoltOffset;

                double calcMaxBoltOffset()
                {
                    return BoltLocations.AsParallel().Aggregate<Vector2, double>(0,
                                                                                 (current, location) =>
                                                                                 Math.Max(current, Math.Abs(location.Y)));
                }
            }

            set => _maximumBoltOffset = value;
        }

        public double StandardBracketMinEdgeDist { get; set; }

        /// <summary>
        /// The controlling load case by BracketStressType.
        /// </summary>
        public BracketLoadCalcs BracketAnalysisLoads { get; set; }

        public override PlateMaterialSpecification ThruPlateSpec
        {
            get { return BracketMaterialSpecification; }
        }

        //public override void InitializeDimensions(TubularArm arm)
        //{
        //    switch (arm.Shape.TubeOrientation)
        //    {
        //        case TubeOrientation.FlatToZero:
        //            Height = arm.BaseFlatWidthInches + 3;
        //            break;

        //        case TubeOrientation.PointToZero:
        //            Height = RegularPolygon.GetPointDiameterAdjustedForRoundCorners(arm.Shape.SideCount, arm.BaseFlatWidthInches, arm.MaterialThicknessInches * 4, arm.MaterialThicknessInches) + 3;
        //            break;
        //    }
        //    BracketThick = SaddleBracketData.GetMinimumAllowableThickness(Finish);
        //    BoltDiameter = .625;
        //}

        /// <summary>
        /// Compute the bracket side width in inches.
        /// On Hock's spreadsheet side width = A.
        /// </summary>
        /// <returns></returns>
        private double ComputeBracketSideWidth()
        {
            double Tb = BracketThick;
            double hs = StiffenerWidth;
            double boltDia = BoltDiameter;
            double magicNumber = 0.5;

            double R = BracketBendRadii.GetBendRadius(Tb);
            double e = ComputeSideEdgeDistance();

            double A1 = R + e + magicNumber;
            double A2 = hs + e + magicNumber;

            double A = Math.Max(A1, A2);
            return A;
        }

        /// <summary>
        /// Compute the side edge distance in inches.
        /// </summary>
        /// <returns></returns>
        private double ComputeSideEdgeDistance()
        {
            double Tb = BracketThick;
            double Ts = StiffenerThick;
            double boltDia = BoltDiameter;

            double e1 = BoltTopEdgeDistance;
            double e2 = Tb + boltDia / 2.0;
            double e3 = Ts + boltDia / 2.0;

            double e = Math.Max(e1, e2);
            e = Math.Max(e, e3);

            return e;
        }

        public override string ToString()
        {
            string bktId = (string.IsNullOrEmpty(StdBracketID) ? "Custom" : StdBracketID);
            return $"{bktId}: {Height}\"X{BracketThick}\" (side width: {BracketSideWidth:f3}\"";
        }
    }
}