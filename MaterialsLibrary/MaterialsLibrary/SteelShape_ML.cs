using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Newtonsoft.Json;

namespace MaterialsLibrary
{   
    [Serializable]
    public class SteelShape_ML
    {
        public enum TubeOrientation
        {
            FlatToZero = 0,
            PointToZero = 1
        }
        public class SteelShape
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            private static List<SteelShape> _steelShapes = null;

            public string Id { get; set; }
            public int SideCount { get; set; }
            public TubeOrientation TubeOrientation { get; set; }
            public string Description { get; set; }

            public bool IsTubing { get; set; }

            public bool UseFWTBracketTable { get; set; }

            public bool IsRegularPolygon { get; set; }
            public double FirstCornerAngle
            {
                get
                {
                    double ans = 0d;
                    if (TubeOrientation == TubeOrientation.FlatToZero && SideCount != 0)
                    {
                        ans = Math.PI / SideCount;
                    }
                    return ans;
                }
            }

            public string POST_SHEET_STRING_ORIENTATION_LETTER
            {
                get
                {
                    return TubeOrientation == TubeOrientation.FlatToZero ? "F" : "T";
                }
            }

            /// <summary>
            /// This is the QT Equivalent ID 
            /// For a regular QT ID such as 8F, this function will return the identical ID, 8F
            /// For a FWT ID such as 3-Oct or hex1, it will return the closest number of sides (rounded up)
            /// that is divisible by 4 and the 
            /// orientation letter, the result being 8T,  12F, etc... 
            /// For tubing, this will return either SQUARE TUBING or RECT TUBING
            /// </summary>
            public string ID_CONVERTED_TO_MULTIPLE_OF_4
            {
                get
                {
                    string answer = string.Empty;
                    if (IsTubing)
                    {
                        if (IsRegularPolygon)
                        {
                            answer = string.Format("SQUARE TUBING{0}", POST_SHEET_STRING_ORIENTATION_LETTER);
                        }
                        else
                        {
                            answer = string.Format("RECT TUBING{0}", POST_SHEET_STRING_ORIENTATION_LETTER);
                        }
                    }
                    else
                    {
                        int sideCountMultOf4 = (int)(4 * Math.Ceiling((decimal)SideCount / 4));

                        answer = string.Format("{0}{1}", sideCountMultOf4, POST_SHEET_STRING_ORIENTATION_LETTER);
                    }
                    return answer;
                }
            }

            /// <summary>
            /// This will return the original ID of this part, with any Oct in the ID switched to Octa
            /// For example, 4-Oct will return 4-Octa.   8F will return 8F.
            /// </summary>
            public string FILTERED_ID
            {
                get
                {
                    string pattern = @"Oct\b";
                    string result = Id;
                    RegexOptions regexOptions = RegexOptions.None;
                    Regex regex = new Regex(pattern, regexOptions);
                    string replacement = @"Octa";
                    if (regex.IsMatch(Id))
                    {
                        result = regex.Replace(Id, replacement);
                    }
                    return result;
                }
            }

            public static List<SteelShape> SteelShapes
            {
                get
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
                            logger.Fatal(ex, "Could not read Steel Shapes json data from resources.");
                            throw;
                        }
                    }
                    return _steelShapes;
                }
            }

            public static SteelShape GetSteelShapeById(string id)
            {
                SteelShape shape = null;
                logger.Debug("Getting steel shape for shape id {0}", id);
                shape = SteelShapes.Find(s => s.Id == id);
                if (shape == null)
                {
                    string msg = string.Format("Steel shape {0} not supported", id);
                    Exception ex = new Exception(msg);
                    logger.Fatal(ex);
                    throw ex;
                }
                logger.Debug("Found steel shape {0}", shape.FILTERED_ID);
                return shape;
            }

            public SteelShape GetSteelShapeThatIsMultipleOf4()
            {
                return GetSteelShapeById(ID_CONVERTED_TO_MULTIPLE_OF_4);
            }
        }
    }
}
