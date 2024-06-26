using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class TubularXArmDTO : TubularArmDTO
    {
        public TubularXArmDTO() 
        {

        }

        public TubularXArmDTO(StructureComponents.Arms.TubularXArm xa) : base(xa)
        {
            ConnectionType = (int)xa.ConnectionType;
        }

        public int ConnectionType { get; set; }

    }
}
