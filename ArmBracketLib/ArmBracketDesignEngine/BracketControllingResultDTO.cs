using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class BracketControllingResultDTO : BracketResultDTO
    {
        public BracketControllingResultDTO()
        {

        }

        public BracketControllingResultDTO(BracketAnalysis.BracketControllingResult obj)
        {
            Allowable = obj.Allowable;
            Description = obj.Description;
            Value = obj.Value;

            Loadcase = obj.Loadcase;
            JointLabel = obj.JointLabel;
            LoadID = obj.LoadID;
            ControllingBracketLoad = new BracketLoadDTO(obj.ControllingBracketLoad);

            Ratio = obj.Ratio;
        }

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
        public BracketLoadDTO ControllingBracketLoad { get; set; }

        public string Ratio { get; set; }

    }
}
