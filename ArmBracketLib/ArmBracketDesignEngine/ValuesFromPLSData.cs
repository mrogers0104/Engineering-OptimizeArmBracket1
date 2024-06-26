using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class ValuesFromPLSData
    {
        public string ArmLabel { get; set; }
        public decimal ArmDft { get; set; }
        public string ArmAttachLabel { get; set; }
        public decimal ArmLengthFt { get; set; }
        public string ArmPropertyLabel { get; set; }

        /// <summary>
        /// TubularDavitArm or TubularCrossArm
        /// </summary>		
        public string ArmType { get; set; }

        public decimal PoleDiameterIn { get; set; }
        public int PoleSideCount { get; set; }
        public decimal PoleStrengthKsi { get; set; }
        public decimal PoleWallThickness { get; set; }
        public decimal PoleTaper { get; set; }
        public decimal ArmTipVerticalOffset { get; set; }
        public decimal ArmTipHorizontalOffset { get; set; }
        public decimal ArmTubeStrengthKsi { get; set; }
        public string ArmShapeName { get; set; }
        public decimal ArmTipFlatWidthInches { get; set; }
        public decimal ArmBaseFlatWidthInches { get; set; }
        public decimal ArmMaterialThicknessInches { get; set; }
        public List<ArmJointDTO> ArmJoints { get; set; }
        public List<ArmLoadDTO> ArmLoads { get; set; }			
						
	}
}
