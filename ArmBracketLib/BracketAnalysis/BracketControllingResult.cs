using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    public class BracketControllingResult : BracketResult
    {
        #region Constructors

        public BracketControllingResult()
        {

        }

        /// <summary>
        /// Do a deep copy of BracketResult source.
        /// </summary>
        /// <param name="src">The source object.</param>
        /// <param name="loadcase">The controlling load case.</param>
        /// <param name="loadLabel">The controlling joint label.</param>
        /// <returns></returns>
        public BracketControllingResult(BracketResult src, string loadcase, Guid loadId, BracketLoad bktLoad)
        {
            Allowable = src.Allowable;
            Description = src.Description;
            Value = src.Value;

            Loadcase = loadcase;
            LoadID = loadId;

            ControllingBracketLoad = bktLoad;
        }

        #endregion  // Constructors

        #region Properties


        /// <summary>
        /// The controlling load case description.
        /// </summary>
        public string Loadcase { get; set; }

        /// <summary>
        /// The controlling joint.
        /// </summary>
        public string JointLabel { get; set; }

        /// <summary>
        /// The load ID.
        /// </summary>
        public Guid LoadID { get; set; }

        /// <summary>
        /// The controlling bracket load, which will contain the results of the bracket analysis.
        /// </summary>
        public BracketLoad ControllingBracketLoad { get; set; }

        #endregion  // Properties


        #region Methods

        public override string ToString()
        {
            string msg = $"{Loadcase} @ {JointLabel} = {Ratio}";

            return msg;
        }

        #endregion  //Methods

    }
}
