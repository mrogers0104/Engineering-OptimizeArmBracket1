using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    /// <summary>
    /// Define the bend radius associated with a specific bracket thickness
    /// </summary>
    public class BracketBendRadius
    {
        #region Constructors

        public BracketBendRadius()
        {
        }

        #endregion  // Constructors

        #region Properties

        /// <summary>
        /// The bracket thickness in inches
        /// </summary>
        public double BktThickness { get; set; }

        /// <summary>
        /// The associated bend radius with the bracket thickness in inches
        /// </summary>
        public double BendRadius { get; set; }

        #endregion  // Properties

        #region Methods

        public override string ToString()
        {
            return $"{BktThickness:f4}\" X {BendRadius:f4}\"";
        }

        #endregion  // Methods
    }
}
