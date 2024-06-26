

// 

using System;
using System.Linq;
using ArmBracketDesignLibrary.Helpers;
using MaterialsLibrary;

namespace ArmBracketDesignLibrary.Materials
{
    public class PlateMaterial
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public PlateMaterial()
        {
        }

        #endregion  // Constructors

        #region Properties


        #endregion  // Properties

        #region Methods

        /// <summary>
        /// Get the material yield strength (Fy) from the Materials library
        /// </summary>
        /// <param name="materialSpec"></param>
        /// <returns></returns>
        public static double GetMaterialFyKsi(PlateMaterialSpecification materialSpec)
        {
            string specTxt = GetMaterialSpecText(materialSpec, true);

            SteelPlateMaterial sPlate = MaterialsLibraryBO.GetPlates(specTxt, false).First();

            if (sPlate != null)
            {
                return (double)sPlate.FyKsi;
            }

            return 0;
        }

        public static string GetMaterialSpecText(PlateMaterialSpecification spec, bool includeFy)
        {
            string result = Enum.GetName(typeof(PlateMaterialSpecification), spec).Replace("_", "-");

            if (!includeFy)
            {
                result = result.Substring(0, result.IndexOf("-", StringComparison.InvariantCultureIgnoreCase));
            }

            return result.ToUpper();
        }



        public static PlateMaterialSpecification GetStandardPlateMaterialSpecification(PlateFinish finish, double thickness)
        {
            switch (finish)
            {
                case PlateFinish.Galvanized:
                    if (thickness <= 1.375)
                    {
                        return PlateMaterialSpecification.A572_65;
                    }
                    if (thickness <= 4)
                    {
                        return PlateMaterialSpecification.A572_50;
                    }
                    return PlateMaterialSpecification.None;

                case PlateFinish.Weathering:
                    if (thickness <= 1.375)
                    {
                        return PlateMaterialSpecification.A871_65;
                    }
                    if (thickness <= 4)
                    {
                        return PlateMaterialSpecification.A588_50;
                    }
                    return PlateMaterialSpecification.None;
            }
            return PlateMaterialSpecification.None;
        }

        #endregion  // Methods
    }
}
