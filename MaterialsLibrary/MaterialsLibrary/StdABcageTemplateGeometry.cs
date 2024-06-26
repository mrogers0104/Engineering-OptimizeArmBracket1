using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace MaterialsLibrary
{

    /// <summary>
    /// The bolt circle types for determining anchor bolt template thicknesses.
    /// </summary>
    public enum BoltCircleTypes
    {
        /// <summary>
        /// All bolt circle types
        /// </summary>
        All = 0,

        /// <summary>
        /// A single bolt circle
        /// </summary>
        Single = 1,

        /// <summary>
        /// A double bolt circle
        /// </summary>
        Double = 2
    }

    /// <summary>
    /// Define the standard anchor bolt cage geometry by customer code.
    /// If no customer code is given, use customer code = "SabreFWT-std"
    /// </summary>
    [Serializable]
    public class StdABCageTemplateGeometry
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        #region Properties

        /// <summary>
        /// The index for this entry.
        /// </summary>
        public int IDX { get; set; }

        /// <summary>
        /// Customer code from QT for this template geometry
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// The bolt circle type for this geometry
        /// </summary>
        public BoltCircleTypes BoltCircleType { get; set; }

        /// <summary>
        /// The thickness of the top anchor bolt template, inches.
        /// </summary>
        public decimal TopTemplateThickness { get; set; }

        /// <summary>
        /// The thickness of the intermediate(middle) anchor bolt template, inches.
        /// </summary>
        public decimal IntermediateTemplateThickness { get; set; }

        /// <summary>
        /// The thickness of the bottom anchor bolt template, inches.
        /// </summary>
        public decimal BottomTemplateThickness { get; set; }

        /// <summary>
        /// The bolt circle adder for the outside diameter of the anchor bolt template, inches.
        /// Template OD = largest bolt circle diameter + OdBoltCircleAdder
        /// </summary>
        public decimal OdBoltCircleAdder { get; set; }

        /// <summary>
        /// The bolt circle factor fo the outside diameter of the anchor bolt template, inches
        /// Template OD = OdBoltCircleFactor * largest bolt circle diameter + OdBoltCircleAdder
        /// </summary>
        public decimal OdBoltCircleFactor { get; set; }

        /// <summary>
        /// Reduce the bottom anchor bolt template OD.
        /// </summary>
        public bool ReducedOD { get; set; } 

        /// <summary>
        /// The bolt circle adder for the inside diameter of the anchor bolt template, inches.
        /// Template ID = smallest bolt circle diameter + IdBoltCircleAdder
        /// </summary>
        public decimal IdBoltCircleAdder { get; set; }

        /// <summary>
        /// The bolt circle factor fo the inside diameter of the anchor bolt template, inches
        /// Template ID = IdBoltCircleFactor * smallest bolt circle diameter + IdBoltCircleAdder
        /// </summary>
        public decimal IdBoltCircleFactor { get; set; }

        /// <summary>
        /// This template is used when the largest bolt circle diameter is &le; MaxBoltCircleDiameter, inches
        /// </summary>
        public decimal MaxBoltCircleDiameter { get; set; }

        /// <summary>
        /// This template is used when the total anchor bolt quantity is &le; MaxBoltQty
        /// </summary>
        public decimal MaxBoltQty { get; set; }

        /// <summary>
        /// The material specification to use for this template
        /// </summary>
        public string MaterialSpec { get; set; }

        public bool Inactive { get; set; }


        #endregion  // Properties

        #region Methods

        /// <summary>
        /// Get the Anchor Bolt template thicknesses from the JSON resource and fill the 
        /// Anchor Bolt Template thicknesses list.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<StdABCageTemplateGeometry> GetStdABCageTemplateGeometries(bool includeInactive)
        {
            logger.Debug("Getting Std AB Cage Template Geometries");

            List<StdABCageTemplateGeometry> sAbCageGeom = new List<StdABCageTemplateGeometry>();

            try
            {
                StdABCageTemplateGeometry[] p = JsonConvert.DeserializeObject<StdABCageTemplateGeometry[]>(Properties.Resources.StdAnchorBoltCageTemplateGeometry_json);
                sAbCageGeom = p.ToList();

                logger.Debug("\tFull list contains {0} items", sAbCageGeom.Count);
                logger.Debug("\t\tInclude inactive flag = {0}", includeInactive);

                if(!includeInactive)
                {
                    int x = sAbCageGeom.RemoveAll(sab => sab.Inactive);
                    logger.Debug("\t\t\tRemoved {0} inactive items", x);
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Could not read Standard Anchor Bolt Cage Geometry json data from resources. ");
                throw;
            }

            return sAbCageGeom;
        }

        #endregion  // Methods

    }
}
