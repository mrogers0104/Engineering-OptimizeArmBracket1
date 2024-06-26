using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    /// <summary>
    /// Bracket load to us for analysis and design of the bracket.
    /// </summary>
    public class BracketLoad : ArmLoad, IEquatable<BracketLoad>
    {

        private BracketBoltGroup _bracketBoltGroup = null;
        private BracketProperties _bracketProperties = null;

        private TubularArm _arm = null;
        private SaddleBracket _bracket = null;

        #region Constructors

        public BracketLoad(Guid armResultId, TubularArm arm, SaddleBracket bracket):base(armResultId)
        {
            _arm = arm;
            _bracket = bracket;

            ControllingStressTypeResults = new Dictionary<BracketStressType, bool>();

            SetupControllingStressTypeDictionary();
        }

        public BracketLoad(ArmLoad result, TubularArm arm, SaddleBracket bracket) : this(result.Id, arm, bracket)
        {
            this.Label = result.Label;
            this.JointLabel = result.JointLabel;
            this.AxialForce = result.AxialForce;
            this.VerticalShear = result.VerticalShear;
            this.VerticalMoment = result.VerticalMoment;
            this.HorizontalShear = result.HorizontalShear;
            this.HorizontalMoment = result.HorizontalMoment;
            this.Torsion = this.Torsion;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The loads at the pole face before translation to the bolt centerline.
        /// </summary>
        public ArmLoad LoadsAtPoleFace { get; set; }

        /// <summary>
        /// The controlling load case by BracketStressType.
        /// </summary>
        public Dictionary<BracketStressType, bool> ControllingStressTypeResults { get; set; }


        #endregion

        #region Methods

        /// <summary>
        /// Create the bolt group and the bracket properties for this bracket load case.
        /// </summary>
        public void SetupBracketBolts()
        {
            _bracketBoltGroup = new BracketBoltGroup(_arm, _bracket, this);

            _bracketProperties = new BracketProperties(_arm, _bracket, _bracketBoltGroup, nonCoped: true);
        }

        /// <summary>
        /// Get the BracketBolts object: contains summary of stresses and moments 
        /// plus bolt group properties: Ix, Iy, Iz, BoltStressT, BoltStressV
        /// </summary>
        /// <returns></returns>
        public BracketBoltGroup GetBracketBoltGroup()
        {
            return _bracketBoltGroup;
        }

        public List<BracketBolt> GetBracketBoltList()
        {
            return _bracketBoltGroup.GetBracketBolts();
        }

        /// <summary>
        /// Get the bracket properties for this load
        /// </summary>
        /// <returns></returns>
        public BracketProperties GetBracketProperties()
        {
            return _bracketProperties;
        }

        public override string ToString()
        {
            string msg = (string.IsNullOrWhiteSpace(Label) ? base.ToString() : $"Load: {this.Label}");

            return msg;
        }

        #endregion


        #region Controlling stress type
        /// <summary>
        /// The governing element for this load: Bracket stress, Shear Rupture, Bearing, Bolt ...
        /// This property is used to display load case values for the Bracket Analysis Report.
        /// </summary>
        public string GoverningElement { get { return GetGoverningElements(); } }

        /// <summary>
        /// Setup the Controlling stress type dictionary 
        /// </summary>
        private void SetupControllingStressTypeDictionary()
        {
            // ** Get the BracketStressTypes
            Array stressTypes = Enum.GetValues(typeof(BracketStressType));

            if (ControllingStressTypeResults == null)
            {
                ControllingStressTypeResults = new Dictionary<BracketStressType, bool>();
            }
            else
            {
                ControllingStressTypeResults.Clear();
            }

            foreach (var typ in stressTypes)
            {
                BracketStressType stressType = (BracketStressType)typ;
                if (stressType == BracketStressType.Unknown)
                {
                    continue;
                }

                ControllingStressTypeResults.Add(stressType, false);

            }
        }

        /// <summary>
        /// Set up the governing element string.
        /// </summary>
        /// <returns></returns>
        private string GetGoverningElements()
        {
            if (ControllingStressTypeResults.Count <= 0)
            {
                return string.Empty;
            }

            // ** determine how many controlling stress types.
            int bracketCnt = 0;
            int bracketBolt = 0;

            // ** Setup the governing elements:
            // **   if bracketCnt >= 3 then all bracket stresses
            // **   if bracketBolt > 0 then the bracket bolt.
            string gov = string.Empty;
            List<string> governs = new List<string>();
            //bool bktThickness = false;
            foreach (var kvp in ControllingStressTypeResults)
            {
                BracketStressType stressType = kvp.Key;
                bool controls = kvp.Value;

                if (controls)
                {
                    switch (stressType)
                    {
                        //case BracketStressType.BracketThickness:
                        //    bracketCnt++;
                        //    bktThickness = true;
                        //    break;
                        case BracketStressType.BracketLegStress:
                            bracketCnt++;
                            //if (bktThickness)
                            //{
                            //    continue;
                            //}
                            break;
                        case BracketStressType.BracketBolts:
                            bracketBolt++;
                            break;
                        default:
                            bracketCnt++;
                            break;
                    }

                    governs.Add(stressType.ToString());
                }

            }

            string gov1 = (bracketBolt > 0 ? "Bolt" : string.Empty);
            string gov2 = (bracketCnt > 2 ? "Bracket" : string.Empty);

            if (!string.IsNullOrEmpty(gov1) && !string.IsNullOrEmpty(gov2))
            {
                gov = string.Join(" & ", gov1, gov2);
            }
            else
            {
                gov = string.Join(", ", governs.ToArray());
                gov = gov.Replace("Bracket", string.Empty); // remove Bracket from string.
            }

            gov = SpaceCamelCase(gov);

            return gov;

        }

        /// <summary>
        /// Add spaces to camel case string.
        /// </summary>
        /// <param name="camel"></param>
        /// <returns></returns>
        /// <remarks>
        /// The regex (?!^)(?=[A-Z]) consists of two assertions:
        ///     (?!^)       - we're not at the beginning of the string
        ///     (?=[A-Z])   - we're just before an uppercase letter
        ///     
        /// See URL: https://stackoverflow.com/questions/3103730/is-there-a-elegant-way-to-parse-a-word-and-add-spaces-before-capital-letters
        /// </remarks>
        private string SpaceCamelCase(string camel)
        {
            Regex r = new Regex(@"(?!^)(?=[A-Z])");

            return r.Replace(camel, " ");
        }

        public bool Equals(BracketLoad other)
        {
            bool eq = this.LoadCase.Equals(other.LoadCase);

            return eq;
        }
        #endregion
    }
}
