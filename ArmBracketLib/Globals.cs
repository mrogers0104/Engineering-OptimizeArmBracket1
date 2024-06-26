using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArmBracketDesignLibrary
{
    public static class Globals
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Fields

        /// <summary>
        /// The percent of additional weight due to galvanizing.
        /// </summary>
        public const double GalvanizingRate = 0.06;

        /// <summary>
        /// The allowable difference
        /// </summary>
        public const double Epsilon = .00001;

        /// <summary>
        /// The density of steel in lbs/ft^3
        /// </summary>
        public const double SteelDensityPerCuFt = 490.0;

        #endregion  // Fields
    
    }
}
