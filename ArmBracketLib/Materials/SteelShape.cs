using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArmBracketDesignLibrary.Helpers;
using Newtonsoft.Json;

namespace ArmBracketDesignLibrary.Materials
{

    /// <summary>
    /// Define steel shapes used for arms.
    /// </summary>
    public class SteelShape
    {

        private static readonly object SteelShapesLock = new object();

        private static List<SteelShape> _steelShapes = null;

        public static List<SteelShape> SteelShapes
        {
            get
            {

                lock (SteelShapesLock)
                {
                    if (_steelShapes == null)
                    {
                        _steelShapes = new List<SteelShape>();
                        try
                        {
                            SteelShape[] s = JsonConvert.DeserializeObject<SteelShape[]>(Properties.Resources.SteelShapes_json);
                            _steelShapes = s.ToList();
                        }
                        catch (Exception ex)
                        {
                            // logger.Fatal(ex, "Could not read Steel Shapes json data from resources.");
                            throw;
                        }
                    }

                    return _steelShapes;
                }

            }
        }

        public string Description { get; set; }
        public string Id { get; set; }

        public bool IsRegularPolygon { get; set; }
        public bool IsTubing { get; set; }
        public int SideCount { get; set; }

        public TubeOrientation TubeOrientation { get; set; }
        //public bool UseFWTBracketTable { get; set; }
        ///// <summary>
        ///// This will return the original ID of this part, with any Oct in the ID switched to Octa
        ///// For example, 4-Oct will return 4-Octa.   8F will return 8F.
        ///// </summary>
        //public string FILTERED_ID
        //{
        //    get
        //    {
        //        string pattern = @"Oct\b";
        //        string result = Id;
        //        RegexOptions regexOptions = RegexOptions.None;
        //        Regex regex = new Regex(pattern, regexOptions);
        //        string replacement = @"Octa";
        //        if (regex.IsMatch(Id))
        //        {
        //            result = regex.Replace(Id, replacement);
        //        }
        //        return result;
        //    }
        //}
        //public static SteelShape GetSteelShapeById(string id)
        //{
        //    SteelShape shape = null;
        //    logger.Debug("Getting steel shape for shape id {0}", id);
        //    shape = SteelShapes.Find(s => s.Id == id);
        //    if (shape == null)
        //    {
        //        string msg = string.Format("Steel shape {0} not supported", id);
        //        Exception ex = new Exception(msg);
        //        logger.Fatal(ex);
        //        throw ex;
        //    }
        //    logger.Debug("Found steel shape {0}", shape.FILTERED_ID);
        //    return shape;
        //}

        public override string ToString()
        {
            string m = (IsRegularPolygon ? "regular" : "irregular");
            string msg = $"{Id} {m} :: {TubeOrientation} w/ {SideCount} sides";

            return msg;
        }
    }
}

