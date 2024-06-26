using System;
using System.Collections.Generic;
using System.Linq;
using ArmBracketDesignLibrary.Helpers;
using Newtonsoft.Json;


namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    /// <summary>
    /// Define all the bracket bend radii by bracket thickness.
    /// </summary>
    public class BracketBendRadii
    {
//        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static readonly object BracketBendRadiiLock = new object();

        private static List<BracketBendRadius> _bracketBendRadii;

        #region Constructors

        public BracketBendRadii()
        {
        }

        #endregion  // Constructors

        #region Properties

        private static List<BracketBendRadius> BracketBendRadiiList
        {
            get
            {
                lock (BracketBendRadiiLock)
                {
                    if (_bracketBendRadii == null)
                    {
                        InitializeBracketBendRadii();
                    }

                    return _bracketBendRadii;
                }
            }

        }


        #endregion  // Properties

        #region Methods

        /// <summary>
        /// Get the bracket bend radius for the bracket thickness in inches.
        /// </summary>
        /// <param name="bktThickness"></param>
        /// <returns></returns>
        public static double GetBendRadius(double bktThickness)
        {
            BracketBendRadius bktRadius = BracketBendRadiiList.FirstOrDefault(r => r.BktThickness.AreEqual(bktThickness));

            return bktRadius.BendRadius;
        }

        /// <summary>
        /// Get the bracket bend radius factor for the bracket thickness.
        /// Where factor = radius / thickness
        /// </summary>
        /// <param name="bktThickness">Bracket thickness in inches</param>
        /// <returns>
        /// Returns the factor, n for nT used to compute bend radius.
        /// If bracket thickness is not found, return 3.0
        /// </returns>
        public static double GetBendRadiusFactor(double bktThickness)
        {
            BracketBendRadius bktRadius = BracketBendRadiiList.FirstOrDefault(r => r.BktThickness.AreEqual(bktThickness));
            if (bktRadius == null)
            {
                return 3.0;
            }

            double factor = bktRadius.BendRadius / bktThickness;

            return factor;
        }

        /// <summary>
        /// Initialize the bracket bend radii listr
        /// </summary>
        private static void InitializeBracketBendRadii()
        {
            try
            {
                _bracketBendRadii = JsonConvert.DeserializeObject<List<BracketBendRadius>>(Properties.Resources.BracketBendRadii_json);
            }
            catch (Exception ex)
            {
//                _logger.Fatal(ex, "Error trying to initialize Bracket Bend Radii");
                throw;
            }
        }

        #endregion  // Methods
    }
}
