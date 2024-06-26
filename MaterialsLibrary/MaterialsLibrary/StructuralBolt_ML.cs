using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace MaterialsLibrary
{
    /// <summary>
    /// Class defining the structural bolt properties.
    /// </summary>
    [Serializable]
    public class StructuralBolt_ML : ICommonBolt
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static List<StructuralBolt_ML> _allStructuralBolts = null;
        private static List<StructuralBolt_ML> _allActiveStructuralBolts = null;
        public static int callCount = 0;

        #region Constructors

        #endregion  // Constructors

        #region Properties

        public string MaterialSpecification { get; set; }
        public double Diameter { get; set; }
        public double NutHeight { get; set; }
        public double NutPointWidth { get; set; }

        public double GrossArea { get; set; }
        public double TensileArea { get; set; }
        public double RootArea { get; set; }
        public double ThreadCountPerInch { get; set; }
        public double MinimumBoltSpacing { get; set; }
        public double MinimumEdgeDistance { get; set; }
        public double FlatWasherDiameter { get; set; }
        public double FlatWasherThickness { get; set; }
        public double BoltHeadAndNutWeight { get; set; }
        public double BoltShaftWeightPerInch { get; set; }
        public double FuKsi { get; set; }
        public double FyKsi { get; set; }
        public double CenterLineBoltToPoleFace { get; set; }
        public bool Inactive { get; set; }

        #region Bracket bolt properties

        /// <summary>
        /// Arm bracket bolt edge distance. 
        /// </summary>
        public double BktBoltEdgeDistance { get; set; }

        /// <summary>
        /// Arm bracket bolt spacing, in.
        /// </summary>
        public double BktBoltSpacing { get; set; }

        public double ThreadLength { get; set; }

        /// <summary>
        /// Distance from arm bracket bolt to face of pole, in.
        /// </summary>
        public double BktBoltToPoleFace { get; set; }

        #endregion

        public double WasherWeight
        {
            get
            {
                double wWt = 180.34 * Diameter * Diameter - 199.74 * Diameter + 95.708;  // wt per 1000 washers
                wWt /= 1000;
                return wWt;
            }
        }


        public double FuPsi
        {
            get
            {
                return FuKsi * 1000;
            }
        }

        public double FyPsi
        {
            get
            {
                return FyKsi * 1000;
            }
        }

        public double MinLengthForFlange()
        {
            // * Compute minimum bolt length using Portland Bolt
            // * see URL: http://www.portlandbolt.com/products/bolts/heavy-hex/
            double boltLeng = Diameter * 2 + 0.25d;
            return (boltLeng > 6 ? boltLeng + 0.25d : boltLeng);
        }

        public double MinLengthNonFlange { get; set; }

        /// <summary>
        /// The minimum bolt length in inches for a structural bolt (flange).
        /// </summary>
        public double MinimumBoltLength
        {
            get
            {
                return MinLengthForFlange();
            }
        }


        public double HoleDiameter
        {
            get
            {
                double ovrSize = (Diameter < 2.25 ? 0.125d : 0.1875d);

                return Diameter + ovrSize;
            }

        }

        private double _nutWeight = 0;
        public double NutWeight
        {
            get
            {
                AnchorBolt_ML ab = MaterialsLibraryBO.GetAnchorBolts(true).Find(abt => (decimal)abt.Diameter == (decimal)Diameter);
                if (ab != null && _nutWeight <= 0)
                {
                    return ab.NutWeight;
                }
                //return 0d;
                return _nutWeight;
            }
            set
            {
                _nutWeight = value;
            }
        }

        private double _nutWeightHeavyHex = 0;
        public double NutWeightHeavyHex
        {
            get
            {
                AnchorBolt_ML ab = MaterialsLibraryBO.GetAnchorBolts(true).Find(abt => (decimal)abt.Diameter == (decimal)Diameter);
                if (ab != null)
                {
                    return ab.NutWeightHeavyHex;
                } else if(NutWeight > 0)
                {
                    return NutWeight;
                }

                return 0d;
            }

            set
            {
                _nutWeightHeavyHex = value;
                if (_nutWeightHeavyHex <= 0 && NutWeight > 0)
                {
                    _nutWeightHeavyHex = NutWeight;
                }
            }
        }


        #endregion  // Properties

        #region Methods

        /// <summary>
        /// Get the Structural Bolt Properties from the JSON resource 
        /// </summary>
        public static List<StructuralBolt_ML> GetStructuralBolts(bool includeInactive)
        {
            callCount++;

            logger.Trace(" <~><~><~>STRUCTURAL BOLTS<~><~><~> callCount:{0}", callCount);

            try
            {
                if (_allStructuralBolts == null || _allActiveStructuralBolts == null)
                {

                    logger.Debug("\tPopulating Structural bolts cache from JSON");
                    StructuralBolt_ML[] s = JsonConvert.DeserializeObject<StructuralBolt_ML[]>(Properties.Resources.StructuralBolts_json);

                    _allStructuralBolts = s.ToList();
                    _allActiveStructuralBolts = s.ToList();
                    _allActiveStructuralBolts.RemoveAll(sp => sp.Inactive == true);
                    logger.Debug(" <~><~> Setup lists: _allStructuralBolts={0} && _allActiveStructrualBolts={1}",
                        _allStructuralBolts.Count, _allActiveStructuralBolts.Count);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Could not read Structural Bolt json data from resources. ");
                throw;
            }

            logger.Trace(" <~><~> list counts: _allStructuralBolts={0} && _allActiveStructrualBolts={1}",
                _allStructuralBolts.Count, _allActiveStructuralBolts.Count);

            return includeInactive ? _allStructuralBolts : _allActiveStructuralBolts;
        }

        public BoltShaftType ShaftType
        {
            get
            {
                if (MaterialSpecification != null)
                {
                    return MaterialSpecification.ToUpper().Contains("A615") ? BoltShaftType.Deformed : BoltShaftType.Smooth;
                }

                return BoltShaftType.None;
            }
        }
        
        double ICommonBolt.NutWeight
        {
            get
            {
                AnchorBolt_ML ab = MaterialsLibraryBO.GetAnchorBolts(true).Find(abt => (decimal)abt.Diameter == (decimal)Diameter);
                if (ab != null)
                {
                    return ab.NutWeight;
                }
                return 0d;
            }

            set { value = 0; }//HACK: to comply with Interface because of the screwy way we have to get Nut Weight
        }
        
        public double Radius
        {
            get
            {
                return Diameter / 2;
            }
        }

        //public double YoungsModulus
        //{
        //    get
        //    {
        //        return Constants.YOUNGS_MODULUS;

        //    }
        //}

        /// <summary>
        /// The section modulus, S,  for the bolt based on the tensile area, cuin.
        /// </summary>
        public double SectionModulus
        {
            get
            {
                double at = TensileArea / Math.PI;
                double r = Math.Sqrt((double)at);

                double myS = Math.PI * (r * r * r) / 4;
                return myS;
            }
        }

        public string QtGradeName { get; set; }
        public override string ToString()
        {
            string matSpec = "?????";
            if (MaterialSpecification != null)
            {
                matSpec = MaterialSpecification;
            }

            string msg = string.Format("{0} x {1:f3} in. dia. ", matSpec, Diameter);
            //return base.ToString();
            return msg;
        }

        public StructuralBolt_ML Clone()
        {
            StructuralBolt_ML b = new StructuralBolt_ML();
            b.BoltHeadAndNutWeight = BoltHeadAndNutWeight;
            b.BoltShaftWeightPerInch = BoltShaftWeightPerInch;
            b.CenterLineBoltToPoleFace = CenterLineBoltToPoleFace;
            b.Diameter = Diameter;
            b.FlatWasherDiameter = FlatWasherDiameter;
            b.FlatWasherThickness = FlatWasherThickness;
            b.FuKsi = FuKsi;
            b.FyKsi = FyKsi;
            b.GrossArea = GrossArea;
            b.Inactive = Inactive;
            b.MaterialSpecification = MaterialSpecification;
            b.MinimumBoltSpacing = MinimumBoltSpacing;
            b.MinimumEdgeDistance = MinimumEdgeDistance;
            b.MinLengthNonFlange = MinLengthNonFlange;
            b.NutHeight = NutHeight;
            b.NutPointWidth = NutPointWidth;
            b.NutWeight = NutWeight;
            b.NutWeightHeavyHex = NutWeightHeavyHex;
            b.QtGradeName = QtGradeName;
            b.RootArea = RootArea;
            b.TensileArea = TensileArea;
            b.ThreadCountPerInch = ThreadCountPerInch;
            return b;
        }
     
        #endregion  // Methods

    }
}
