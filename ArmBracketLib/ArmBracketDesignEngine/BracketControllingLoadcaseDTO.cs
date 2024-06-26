using ArmBracketDesignLibrary.BracketAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using NLog;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class BracketControllingLoadcaseDTO
    {

     

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

                /// <summary>
        /// The actual value: stress, thickness, etc.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The allowable value: stress, thickness, etc.
        /// </summary>
        public double Allowable { get; set; }


        /// <summary>
        /// The interaction ratio percent
        /// </summary>
        public string Ratio
        {
            get
            {
                double ratio = InteractionRatio * 100.0;
                return $"{ratio:f2}%";
            }
        }

        /// <summary>
        /// The interaction ratio.  Expecting a ratio &le; 1.0
        /// </summary>
        public double InteractionRatio
        {
            get
            {
                if (Allowable == 0)
                {
                    return 0.0;
                }

                double ratio = Value / Allowable;

                return ratio;
            }

        }

        #endregion  // Properties

       
    }
}
