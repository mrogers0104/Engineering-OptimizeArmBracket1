using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketLib.BracketAnalysis;

// using NLog;

namespace ArmBracketLib.ArmBracketDesignEngine
{
    public class BracketLoadsDTO
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public BracketLoadsDTO()
        {
        }

        #endregion  // Constructors

        #region Properties

        /// <summary>
        /// This bracket design works for the given loads (ie not overstressed and W/t within limits).  
        /// Look in ControllingLoadCase to find out which one.
        /// </summary>
        public bool DesignWorked { get; set; }

        /// <summary>
        /// The controlling load case by BracketStressType.
        /// </summary>
        public Dictionary<BracketStressType, BracketControllingResult> BracketAnalysisResults { get; set; }

        /// <summary>
        /// List of unique controlling load cases for this bracket.
        /// </summary>
        public List<BracketLoad> ControllingLoadcaseList { get; set; }


        public Dictionary<string, List<BracketLoad>> BracketLoadItems { get; }


        #endregion  // Properties

        #region Methods


        #endregion  // Methods
    }
}
