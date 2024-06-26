using ArmBracketDesignLibrary.BracketAnalysis;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class BracketLoadcaseDTO
    {

        public BracketLoadDTO LoadAtPoleFace { get; set; }

        public Dictionary<BracketStressType, bool> ControllingStressTypeResults { get; set; }

    }
}
