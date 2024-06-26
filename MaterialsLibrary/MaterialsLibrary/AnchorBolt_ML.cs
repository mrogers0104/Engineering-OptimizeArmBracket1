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
    /// Class defining anchor bolt properties.
    /// </summary>
   [Serializable]
    public class AnchorBolt_ML : ICommonBolt
    {
        public static int callCount = 0;
        private static List<AnchorBolt_ML> _allActvieAnchorBolts = null;
        private static List<AnchorBolt_ML> _allAnchorBolts = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region Constructors
        #endregion  // Constructors

        #region Properties
        public double BoltHeadAndNutWeight { get; set; }
        public double BoltHoleSizeBasePlate { get; set; }
        public double BoltHoleSizeSmooth { get; set; }
        public double BoltHoleSizeThreaded { get; set; }
        public double BoltShaftWeightPerInch { get; set; }
        public double CenterLineBoltToPoleFace { get; set; }
        public string Description { get; set; }
        public double Diameter { get; set; }
        public double FlatWasherDiameter { get; set; }
        public double FlatWasherThickness { get; set; }
        public double FuKsi { get; set; }
        public double FyKsi { get; set; }
        public int Grade { get; set; }
        /// <summary>
        /// The gross area of this bolt, sq.in.
        /// </summary>
        public double GrossArea { get; set; }

        /// <summary>
        /// The bolt hole diameter in inches.
        /// This property will be equal to BoltHoleSizeBasePlate.
        /// </summary>
        public double HoleDiameter
        {
            get
            {
                double ovrSize = 0.25d;
                if (Diameter >= 2.00d)
                {
                    ovrSize = 0.375d;
                }
                else if (Diameter >= 1.25d)
                {
                    ovrSize = 0.3125d;
                }

                return Diameter + ovrSize;
            }
        }

        public int IDX { get; set; }
        public bool Inactive { get; set; }
        public string MaterialSpecification { get; set; }
        public double MinimumBoltSpacing { get; set; }

        /// <summary>
        /// The minimum bolt length in inches.
        /// </summary>
        public double MinimumBoltLength
        {
            get
            {
                return 48;
            }
        }

        public double MinimumEdgeDistance { get; set; }

        public double MinimumHoleDiameter { get; set; }

        /// <summary>
        /// Minimum anchor bolt projection in inches.
        /// </summary>
        public double MinimumProjection
        {
            get
            {
                double prj = 12; 

                if (Diameter <= 0.75)
                {
                    prj = 6;
                }
                else if (Diameter <= 1.00)
                {
                    prj = 7;
                }
                else if (Diameter <= 1.25)
                {
                    prj = 8;
                }
                else if (Diameter <= 1.50)
                {
                    prj = 9;
                }
                else if (Diameter <= 1.75)
                {
                    prj = 10;
                }
                else if (Diameter <= 2.00)
                {
                    prj = 11;
                }
                else
                {
                    prj = 12;
                }


                return prj;
                //return (Diameter >= 2.25 ? 12.0 : 7.5);
            }
        }

        public double NutBearingArea { get; set; }

        public double NutHeight { get; set; }

        public double NutPointWidth { get; set; }

        public double NutVolume { get; set; }

        public double NutWeight { get; set; }

        public double NutWeightHeavyHex { get; set; }

        public double ProofLoad { get; set; }
        public string QtGradeName { get; set; }

        /// <summary>
        /// The root area of this bolt in sq.in.
        /// </summary>
        public double RootArea { get; set; }

        /// <summary>
        /// The tensile area of this bolt, sq.in.
        /// </summary>
        public double TensileArea { get; set; }

        public double ThreadCountPerInch { get; set; }
        #endregion  // Properties

        #region Methods

        public double FuPsi
        {
            get
            {
                return FuKsi * 1000;
            }
        }

        //public double YoungsModulus
        //{
        //    get
        //    {
        //        return Constants.YOUNGS_MODULUS; 
        //    }
        //}
        public double FyPsi
        {
            get
            {
                return FyKsi * 1000;
            }
        }

        public double Radius
        {
            get
            {
                return Diameter / 2;
            }
        }

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

        public double WasherWeight
        {
            get
            {
                double d = Convert.ToDouble(Diameter);
                double wWt = 180.34 * d * d - 199.74 * d + 95.708;  // wt per 1000 washers
                wWt /= 1000;

                return wWt;
            }
        }

        /// <summary>
        /// Get the Anchor Bolts from the JSON resource
        /// </summary>
        public static List<AnchorBolt_ML> GetAnchorBolts(bool includeInactive)
        {
            callCount++;
                        
            try
            {
                if (_allAnchorBolts == null || _allActvieAnchorBolts == null)
                {
                    logger.Debug("Populating Anchor Bolts cache from JSON");
                    AnchorBolt_ML[] ab = JsonConvert.DeserializeObject<AnchorBolt_ML[]>(Properties.Resources.AnchorBolts_json);

                    _allAnchorBolts = ab.ToList();
                    _allActvieAnchorBolts = ab.ToList();
                    _allActvieAnchorBolts.RemoveAll(abp => abp.Inactive == true);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Could not read Anchor Bolt json data from resources. ");
                throw;
            }

            return includeInactive ? _allAnchorBolts : _allActvieAnchorBolts;
        }
        public override string ToString()
        {
            string msg = string.Format("{0} x {1:f3} in. dia. ", MaterialSpecification, Diameter);
            //return base.ToString();
            return msg;
        }

        #endregion  // Methods

    }
}
