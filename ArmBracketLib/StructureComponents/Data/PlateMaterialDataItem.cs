using System;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    public class PlateMaterialDataItem : IComparable<PlateMaterialDataItem>
    {
        public string FinishType { get; set; }

        public decimal MaterialThickness { get; set; }

        public PlateFinish FinishEnum
        {
            get
            {
                return (PlateFinish) Enum.Parse(typeof (PlateFinish), FinishType);
            }
        }

        public int CompareTo(PlateMaterialDataItem obj)
        {
            return MaterialThickness.CompareTo(obj.MaterialThickness);
        }

        public override string ToString()
        {
            return $"{FinishType}: Thick={MaterialThickness:f4}\"";
        }
    }
}
