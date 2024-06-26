using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NLog;

namespace MaterialsLibrary
{
    /// <summary>
    /// Class defining the anchor bolt template thicknesses by maximum bolt circle diameter.
    /// </summary>
    [Serializable]
    public class ABTemplateThickness
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Properties

        public int IDX { get; set; }
        public decimal Thickness { get; set; }
        public decimal MaxBoltCircleDiameter { get; set; }

        public bool Inactive { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the Anchor Bolt template thicknesses from the JSON resource and fill the
        /// Anchor Bolt Template thicknesses list.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<ABTemplateThickness> GetABTemplateThicknesses(bool includeInactive)
        {
            logger.Debug("Getting ABTemplateThickness List");

            List<ABTemplateThickness> abTempThick = new List<ABTemplateThickness>();

            try
            {
                ABTemplateThickness[] p = JsonConvert.DeserializeObject<ABTemplateThickness[]>(Properties.Resources.AnchorBoltTemplateThickness_json);
                abTempThick = p.ToList();

                logger.Debug("\tFull list contains {0} items", abTempThick.Count);
                logger.Debug("\t\tInclude inactive flag = {0}", includeInactive);

                if (!includeInactive)
                {
                    int x = abTempThick.RemoveAll(abt => abt.Inactive == true);
                    logger.Debug("\t\t\tRemoved {0} inactive items", x);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not read Anchor Bolt Thicknesses json data from resources. " + ex.Message);
            }

            return abTempThick;
        }

        #endregion Methods
    }
}