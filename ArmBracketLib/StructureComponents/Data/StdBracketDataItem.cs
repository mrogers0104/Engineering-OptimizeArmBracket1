using System;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{


    public class StdBracketDataItem : IComparable<StdBracketDataItem>
    {
        public BracketArmType ArmType { get; set; }

        //public double BlackWeight { get; set; }

        public double BlackWeight => WeatheringWeight;

        public double BoltDiameter { get; set; }

        public double BoltLength { get; set; }

        public int BoltQty { get; set; }

        public double BoltSpacing { get; set; }

        public double BoltCenterSpacing { get; set; }

        public StructuralBolts.StructuralBoltGrades BoltSpec { get; set; }

        public string BracketID { get; set; }

        public double BracketOpening { get; set; }

        public double BracketOutsideWidth
        {
            get
            {
                return BracketOpening + 2 * BracketThick;
            }
        }

        /// <summary>
        /// Side width from bolt hole to butt of arm, in.  [[A + Tb]]
        /// </summary>
        public double BracketSideWidth { get; set; }

        /// <summary>
        /// Side width from back of bracket to end of leg [[A + e]]
        /// </summary>
        public double LegDim { get; set; }

        /// <summary>
        /// Bracket leg length from butt of arm to end of leg, in.  [[B = A + e + Tb]]
        /// Where:
        ///      A = Inside face of bracket to bolt hole
        ///      e = Side edge distance from bolt hole to end of leg
        ///     Tb = bracket thickness
        /// </summary>
        public double LegLength
        {
            get
            {
                return BracketSideWidth + MinEdgeDist;
            }
        }
        public double BracketThick { get; set; }

        public double GalvWeight { get; set; }

        /// <summary>
        /// Weathering weight = Black weight in lbs
        /// </summary>
        public double WeatheringWeight { get; set; }

        public double Height { get; set; }

        /// <summary>
        /// Minimum edge distance, in.   [[e]]
        /// </summary>
        public double MinEdgeDist { get; set; }

        public double STDMaxArmBaseDiam { get; set; }

        public double STDMaxArmThick { get; set; }

        public double STDMinArmBaseDiam { get; set; }

        public double HEXMaxArmBaseDiam { get; set; }

        public double HEXMinArmBaseDiam { get; set; }

        public int StiffenerQty { get; set; }

        public double StiffenerThick { get; set; }

        public double StiffenerVertSpacing { get; set; }

        public double StiffenerWidth { get; set; }

        public string ThruPlateDBLID { get; set; }

        public string ThruPlateSGLID { get; set; }

        public double ThruPlateThick { get; set; }

        public double ThruPlateWidth { get; set; }

        public int CompareTo(StdBracketDataItem obj)
        {
            int result;
            result = STDMaxArmThick.CompareTo(obj.STDMaxArmThick);
            if (result != 0) return result;

            result = STDMaxArmBaseDiam.CompareTo(obj.STDMaxArmBaseDiam);
            if (result != 0) return result;

            result = STDMinArmBaseDiam.CompareTo(obj.STDMinArmBaseDiam);
            return result;

        }

        public override string ToString()
        {
            string msg = $"{BracketID}:  {BracketOpening:f3}(w) x {BracketThick:f4}(t)  {GalvWeight:f2}(wt)";
            return msg;
        }
    }
}
