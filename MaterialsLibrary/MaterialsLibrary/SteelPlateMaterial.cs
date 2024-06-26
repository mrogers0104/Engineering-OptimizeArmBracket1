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
    /// Class defining Steel Plate
    /// </summary>
    [Serializable]
    public class SteelPlateMaterial
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static List<SteelPlateMaterial> _allSteelPlates = null;
        private static List<SteelPlateMaterial> _allActiveSteelPlates = null;
        public static int callCount = 0;

        #region Properties

        public string ASTM { get; set; }

        public decimal FyPsi { get; set; }
        public decimal FuPsi { get; set; }
        public decimal Thickness { get; set; }
        public bool Inactive { get; set; }

        /// <summary>
        /// The density of steel in lbs / cuft.
        /// </summary>
        public decimal DensityPCF
        {
            get { return 490; }
        }

        /// <summary>
        /// The density of steel in lbs / cuin.
        /// </summary>
        public decimal DensityPCI { get { return DensityPCF / 1728; } }

        public decimal YoungsModulus
        {
            get { return 29500000; }
        }
        public decimal PoissonRatio
        {
            get { return .29m; }
        }

        public decimal FyKsi
        {
            get { return FyPsi * .001m; }
        }

        public decimal FuKsi
        {
            get { return FuPsi * .001m; }
        }

        public string MaterialSpecAndYield
        {
            get
            {
                if (ASTM.ToUpper() == "A36")
                {
                    return "A36";
                }
                else
                {
                    return string.Format("{0}-{1:f0}", ASTM.ToUpper(), FyKsi);
                }
            }
        }



        /// <summary>
        /// Is this weathering steel?
        /// </summary>
        public bool isWeathering
        {
            get { return (ASTM.ToUpper() == "A588" || ASTM.ToUpper() == "A871"); }
        }

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// Get the Steel Plates from the JSON resource and fill the cache if necessary
        public static List<SteelPlateMaterial> GetSteelPlates(bool includeInactive)
        {
            callCount++;

            try
            {
                if (_allSteelPlates == null || _allActiveSteelPlates == null)
                {
                    logger.Debug("Populating Steel Plate cache List from JSON");
                    SteelPlateMaterial[] p = JsonConvert.DeserializeObject<SteelPlateMaterial[]>(Properties.Resources.SteelPlate_json);
                    _allSteelPlates = p.ToList().OrderBy(sp => sp.Thickness).ToList();
                    _allActiveSteelPlates = p.ToList().OrderBy(sp => sp.Thickness).ToList();
                    _allActiveSteelPlates.RemoveAll(pms => pms.Inactive == true);
                }
            }
            catch (Exception ex)
            {

                logger.Fatal(ex, "Error populating steel plates cache");
                throw;
            }

            return includeInactive ? _allSteelPlates : _allActiveSteelPlates;
        }

        #endregion  // Methods

    }
}
