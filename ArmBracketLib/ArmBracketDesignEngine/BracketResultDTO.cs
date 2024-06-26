using ArmBracketDesignLibrary.BracketAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class BracketResultDTO
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public BracketResultDTO()
        {
        }

        #endregion  // Constructors

        #region Properties

        public string Description { get; set; }

        /// <summary>
        /// The actual value: stress, thickness, etc.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The allowable value: stress, thickness, etc.
        /// </summary>
        public double Allowable { get; set; }

        /// <summary>
        /// The interaction ratio.  Expecting a ratio &le; 1.0
        /// </summary>
        public double InteractionRatio { get; set; }

        #endregion  // Properties

    }
}
