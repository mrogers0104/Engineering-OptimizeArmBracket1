using ArmBracketDesignLibrary.StructureComponents.Arms;
using MaterialsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 

namespace ArmBracketDesignLibrary.Helpers
{
    /// <summary>
    /// Define various utilities methods
    /// </summary>
    public static class Utils
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        //public Utils(){ }

        /// <summary>
        /// Get the distance from the pole face to the center of the bracket bolt, in.
        /// </summary>
        /// <param name="bracket"></param>
        /// <returns></returns>
        public static double GetPoleFaceToBracketBolt(TubularArmAttachmentPoint bracket)
        {
            string boltSpec = bracket.BoltSpec.ToString();
            boltSpec = (boltSpec.Contains("354") ? boltSpec.Insert(4, "-") : boltSpec);
            
            StructuralBolt_ML bktBolt = MaterialsLibraryBO.GetStructuralBolt((decimal)bracket.BoltDiameter, boltSpec, includeInactive: false);

            double faceToBolt = bktBolt.BktBoltToPoleFace;

            return faceToBolt;
        }

        /// <summary>
        /// Compute the distance from the pole face to the bracket face in inches
        /// </summary>
        /// <param name="bracket"></param>
        /// <returns></returns>
        public static double GetPoleFaceToBracketFace(TubularArmAttachmentPoint bracket)
        {
            // ** Is bracket dimensions defined?
            if (bracket.BoltDiameter.IsZero() || bracket.BracketThick <= 0)
            {
                return 0.0;
            }

            double faceToBolt = GetPoleFaceToBracketBolt(bracket);

            double length = bracket.BracketSideWidth + faceToBolt;

            return length;
        }


        /// <summary>
        /// Get the bolt spacing from the front of the bracket to the bolt hole in inches
        /// </summary>
        /// <param name="frontPlateThickness">The bracket thickness in inches</param>
        /// <returns></returns>
        public static double GetBoltSpacingFromFrontPlate(double frontPlateThickness)
        {
            if (frontPlateThickness < .625) return 3;
            if (frontPlateThickness < 1) return 3;
            if (frontPlateThickness.AreEqual(1d)) return 3;
            if (frontPlateThickness.AreEqual(1.25)) return 3;
            if (frontPlateThickness.AreEqual(1.5)) return 3;
            if (frontPlateThickness.AreEqual(1.75)) return 3;
            if (frontPlateThickness.AreEqual(2)) return 3.5;
            if (frontPlateThickness.AreEqual(2.25)) return 4.5;
            if (frontPlateThickness.AreEqual(2.5)) return 5;
            if (frontPlateThickness.AreEqual(2.75)) return 5.5;
            if (frontPlateThickness.AreEqual(3)) return 6;

            return 3.25;
        }


        public static double GetMinimumSideWidthNewRadius(double thickness)
        {
            if (thickness < 1)
            {
                return 5;
            }

            if (thickness.AreEqual(1))
            {
                return 6;
            }

            return 7;
        }

      

    }
}
