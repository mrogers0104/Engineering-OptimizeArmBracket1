using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizeArmBracket1.Helpers
{
    internal static class Globals
    {
        /// <summary>
        /// Start range for bolt diameter in inches
        /// </summary>
        public const decimal BoltDiaStart = 0.625m;

        /// <summary>
        /// End range for bolt diameter in inches
        /// </summary>
        public const decimal BoltDiaEnd = 1.50m;

        /// <summary>
        /// Increment bolt diameter range in inches
        /// </summary>
        public const decimal BoltDiaIncrement = 0.125m;

        /// <summary>
        /// Maximum bolt quantity
        /// </summary>
        public const int MaxBoltQty = 20;

        /// <summary>
        /// Available plate thicknesses in inches.
        /// </summary>
        public static readonly List<decimal> BracketThick = new List<decimal> { 0.50m, 0.625m, 0.75m, 0.875m, 1.00m, 1.25m };

        /// <summary>
        /// The stiffener qty
        /// </summary>
        public static readonly List<int> StiffenerQty = new List<int> { 0, 2 };

    }
}
