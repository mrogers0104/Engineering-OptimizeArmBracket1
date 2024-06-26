using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using NLog;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    public class CustomBracketDataItem : IComparable<CustomBracketDataItem>
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public CustomBracketDataItem() { }

        #endregion  // Constructors

        #region Properties

            public double ShaftID { get; set; }
            public double XSectArea { get; set; }
            public double CenterCutoutDia { get; set; }
            public double DrainHoleDia { get; set; }
            public int DrainHoleQty { get; set; }
            public double DrainHoleDist { get; set; }
            public double CutoutArea { get; set; }
            public double PercentXSectArea { get; set; }
            public string CutoutType { get; set; }
            public string PartNumber { get; set; }

        #endregion  // Properties

        #region Methods

        public int CompareTo(CustomBracketDataItem obj)
        {
            int result;
            result = ShaftID.CompareTo(obj.ShaftID);
            if (result != 0) return result;

            return result;
        }

        public override string ToString()
        {
            string msg = $"{PartNumber}:  Arm ID = {ShaftID:f3}(id) CntrDia = {CenterCutoutDia:f2} DrainDia {DrainHoleDia:f2}";
            return msg;
        }

        #endregion  // Methods
    }
}
