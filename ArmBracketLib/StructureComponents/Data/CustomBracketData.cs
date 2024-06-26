using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary.StructureComponents.Arms;
//using LUtility.LMath;
using Newtonsoft.Json;
using NLog;
//using Sts.UtilityStructure.StructureComponents.Arms;

namespace ArmBracketDesignLibrary.StructureComponents.Data
{
    public class CustomBracketData
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly object CustomBracketLock = new object();

        private static List<CustomBracketDataItem> _customBrackets;

        #region Constructors

        public CustomBracketData() { }

        #endregion  // Constructors

        #region Properties

        #endregion  // Properties

        #region Methods

        private static List<CustomBracketDataItem> CustomBrackets
        {
            get
            {
                lock (CustomBracketLock)
                {
                    if (_customBrackets == null)
                    {
                        InitializeCustomBrackets();
                    }

                    return _customBrackets;
                }
            }

        }

        /// <summary>
        /// Initialize the custom bracket list.
        /// </summary>
        private static void InitializeCustomBrackets()
        {
            try
            {
                _customBrackets = JsonConvert.DeserializeObject<List<CustomBracketDataItem>>(Properties.Resources.SabreCustomBrackets_json);
                _customBrackets.Sort();
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "Error trying to initialize Custom Brackets");
                throw;
            }
        }

        /// <summary>
        /// Get the custom bracket for the arm
        /// </summary>
        /// <param name="arm"></param>
        /// <returns>Returns a list of custom brackets for the arm inside diameter (in)</returns>
        public static CustomBracketDataItem GetCustomBracket(TubularArm arm)
        {
            //List<CustomBracketDataItem> customBktList = new List<CustomBracketDataItem>();
            //CustomBracketDataItem customBkt = null;

            //BracketArmType bktArmType = (arm.Shape.SideCount == 6 ? BracketArmType.Hex : BracketArmType.Common);

            double armBaseWidth = arm.BaseFlatWidthInches;
            double armThickness = arm.MaterialThicknessInches;
            double armBaseId = armBaseWidth - 2.0 * armThickness;

            // ** Get the custom Sabre brackets.
            CustomBracketDataItem customBkt = FindCustomBracketSmartPart(armBaseId);
            //customBktList = CustomBrackets.FindAll(o => useArmBaseId <= o.ShaftID);

            // ** Check the width vs height for an irregular polygon (ie hex arm)
            if (!arm.Shape.IsRegularPolygon)
            {
                if (SaddleBracketData.GetPolygonWidthAndHeight(arm, out double minBktWidth, out double minBktHt))
                {
                    double bkt = Math.Max(minBktWidth, minBktHt);
                    customBkt = FindCustomBracketSmartPart(bkt);
                }
            }

            return customBkt;
        }

        private static CustomBracketDataItem FindCustomBracketSmartPart(double armBaseId)
        {
            CustomBracketDataItem customBkt = null;

            for (int i = 0; i < CustomBrackets.Count - 1; i++)
            {
                var custBkt = CustomBrackets[i];
                var nxtCustBkt = CustomBrackets[i + 1];
                if (armBaseId >= custBkt.ShaftID && armBaseId < nxtCustBkt.ShaftID )
                {
                    customBkt = custBkt;
                    break;
                }
            }

            return customBkt;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion  // Methods
    }
}
