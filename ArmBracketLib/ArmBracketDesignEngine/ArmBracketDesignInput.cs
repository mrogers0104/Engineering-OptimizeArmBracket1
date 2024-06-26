using ArmBracketDesignLibrary.Helpers;
using System;
using NLog;


namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class ArmBracketDesignInput
    {
        public ArmBracketDesignInput()
        {
            BundleId = Guid.NewGuid();
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Guid identifying this run.  System generated.
        /// </summary>
        public Guid BundleId { get; private set; }

        /// <summary>
        /// The standard input values for bracket analysis
        /// </summary>
        public UserInputs UserInputs { get; set; } = new UserInputs();

        public ValuesFromPLSData ValuesFromPLSData { get; set; } = new ValuesFromPLSData();

    }
}