using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class PoleStructureDTO
    {
        public PoleStructureDTO()
        {

        }

        public PoleStructureDTO(StructureComponents.Pole.PoleStructure pol)
        {
            PoleDiameterIn = pol.PoleDiameterIn;
            PoleTaper = pol.PoleTaper;
            WallThickness = pol.WallThickness;
            PlateFinish = (int)pol.PlateFinish;
        }

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
        public int PlateFinish { get; set; }

        #region Methods

        public override string ToString()
        {
            return $"Pole @ Thruplate cntr: {PoleDiameterIn:f2}\"X{WallThickness:f4}\" ({PoleTaper} in/ft)";
        }

        #endregion  // Methods

    }
}
