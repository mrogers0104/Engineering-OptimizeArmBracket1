using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizeArmBracket1.Optimization
{
    internal class ArmBktEventArgs : EventArgs
    {
        public ArmBracketDesignResults? ArmBktResults{ get; set; }

        public double Fitness { get; set; }
    }
}
