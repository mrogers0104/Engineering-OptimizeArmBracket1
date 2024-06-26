using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class CustomBracketInput
    {
        public decimal? BoltDiameter { get; set; }
        public decimal? BoltLength { get; set; }

        /// <summary>
        /// The total number of bolts for the bracket
        /// </summary>
        public int? TotalBoltQty { get; set; }
        public decimal? BracketOpening { get; set; }
        public decimal? BracketThick { get; set; }
        public decimal? BracketHeight { get; set; }
        public int? StiffenerQty { get; set; }
        public decimal? StiffenerThick { get; set; }
        public decimal? StiffenerWidth { get; set; }
        public decimal? ThruPlateWidth { get; set; }
        public decimal? ThruPlateThick { get; set; }


        public bool IsValid()
        {
            return AllNull() || AllNotNull();
        }

        public bool AllNull()
        {
            return BoltDiameter == null
                && BoltLength == null
                && TotalBoltQty == null
                && BracketOpening == null
                && BracketThick == null
                && BracketHeight == null
                && StiffenerQty == null
                && StiffenerThick == null
                && ThruPlateWidth == null
                && ThruPlateThick == null;
        }

        public bool AllNotNull()
        {
            return BoltDiameter != null
                && BoltLength != null
                && TotalBoltQty != null
                && BracketOpening != null
                && BracketThick != null
                && BracketHeight != null
                && StiffenerQty != null
                && StiffenerThick != null
                && ThruPlateWidth != null
                && ThruPlateThick != null;
        }
    }
}
