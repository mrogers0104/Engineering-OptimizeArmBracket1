using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    public class BracketToArmWeldSize
    {
        #region Constructors

        public BracketToArmWeldSize()
        {
        }

        #endregion  // Constructors

        #region Properties

        public double ArmWallThick { get; set; }

        public double Threshold { get; set; }

        public double PJP_1 { get; set; }

        public double PJP_2 { get; set; }

        public double CJP { get; set; }

        #endregion  // Properties

        #region Methods

        public override string ToString()
        {
            return $"Arm Thk: {ArmWallThick:f4}\" :: PJP(1)={PJP_1}\" PJP(2)={PJP_2}\" CJP={CJP}\"";
        }

        #endregion  // Methods
    }
}
