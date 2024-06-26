using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class SteelShapeDTO
    {
        public string Description { get; set; }
        public string Id { get; set; }
        public bool IsRegularPolygon { get; set; }
        public bool IsTubing { get; set; }
        public int SideCount { get; set; }
        public int TubeOrientation { get; set; }
    }
}
