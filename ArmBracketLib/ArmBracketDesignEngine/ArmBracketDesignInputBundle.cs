using System;
using System.Collections.Generic;

// 

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class ArmBracketDesignInputBundle
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Properties

        /// <summary>
        /// The list of brackets from the Web API to design.
        /// </summary>
        public List<ArmBracketDesignInput> Inputs { get; set; } = new List<ArmBracketDesignInput>();

        public Guid Guid { get; set; } = Guid.NewGuid();

        #endregion Properties
    }
}