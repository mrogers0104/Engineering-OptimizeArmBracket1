using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;

// 

namespace ArmBracketDesignLibrary.StructureComponents.Pole
{
    [Serializable]
    public class PoleStructure
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PoleStructure()
        {

        }

        public PoleStructure(ArmBracketDesignEngine.PoleStructureDTO dto)
        {
            PoleDiameterIn = dto.PoleDiameterIn;
            WallThickness = dto.WallThickness;
            PoleTaper = dto.PoleTaper;
            PlateFinish = (PlateFinish)dto.PlateFinish;

        }

        #region Properties

        /// <summary>
        /// The diameter at the center of the arm in inches.
        /// </summary>
        public double PoleDiameterIn { get; set; }

        /// <summary>
        /// The pole wall thickness in inches.
        /// </summary>
        public double WallThickness { get; set; }

        /// <summary>
        /// The taper for this pole section in inch/ft
        /// </summary>
        public double PoleTaper { get; set; }

        /// <summary>
        /// The plate type for this pole structure: Galvanized or Weathering.
        /// </summary>
        public PlateFinish PlateFinish { get; set; }


        #endregion  // Properties

        #region Methods

        public override string ToString()
        {
            return $"Pole @ Thruplate cntr: {PoleDiameterIn:f2}\"X{WallThickness:f4}\" ({PoleTaper} in/ft)";
        }

        #endregion  // Methods
    }
}
