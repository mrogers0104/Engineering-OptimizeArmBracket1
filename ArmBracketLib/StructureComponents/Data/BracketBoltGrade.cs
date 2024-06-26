using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    public class BracketBoltGrade
    {
        #region Constructors

        public BracketBoltGrade()
        {
        }

        #endregion  // Constructors

        #region Properties

        public string BoltSpec { get; set; }

        public double Fy_1 { get; set; }

        public double Fy_2 { get; set; }

        public double Fy_3 { get; set; }

        public double Fu_1 { get; set; }

        public double Fu_2 { get; set; }

        public double Fu_3 { get; set; }

        #endregion  // Properties

        #region Methods

        public override string ToString()
        {
            return $"{BoltSpec}: Fy(1)={Fy_1} ksi :: Fu(1)={Fu_1} ksi";
        }

        #endregion  // Methods
    }
}
