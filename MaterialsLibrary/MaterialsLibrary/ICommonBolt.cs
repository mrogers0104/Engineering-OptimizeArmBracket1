using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaterialsLibrary
{

    public enum BoltShaftType
    {
        None,
        Smooth,
        Deformed
    }


    public interface ICommonBolt
    {
        BoltShaftType ShaftType { get; }

        double BoltHeadAndNutWeight { get; set; }

        double BoltShaftWeightPerInch { get; set; }

        /// <summary>
        /// The diameter of this bolt in inches.
        /// </summary>
        double Diameter { get; set; }

        double FlatWasherDiameter { get; set; }

        double FlatWasherThickness { get; set; }

        double FuKsi { get; set; }

        double FuPsi { get; }

        double FyKsi { get; set; }

        double FyPsi { get; }

        double GrossArea { get; set; }

        double HoleDiameter { get; }

        double MinimumBoltSpacing { get; set; }

        double MinimumBoltLength { get; }

        double MinimumEdgeDistance { get; set; }

        double NutHeight { get; set; }

        double NutPointWidth { get; set; }

        double NutWeightHeavyHex { get; set; }

        double NutWeight { get; set; }

        double Radius { get; }

        double RootArea { get; set; }

        double SectionModulus { get; }

        double TensileArea { get; set; }

        double WasherWeight { get; }

        string MaterialSpecification { get; set; }

        string QtGradeName { get; set; }

        bool Inactive { get; set; }


    }
}
