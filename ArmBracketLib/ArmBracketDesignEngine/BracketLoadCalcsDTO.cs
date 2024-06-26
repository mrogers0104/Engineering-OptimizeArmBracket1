using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class BracketLoadCalcsDTO
    {

        #region Constructors

        public BracketLoadCalcsDTO()
        {

        }

        public BracketLoadCalcsDTO(BracketAnalysis.BracketLoadCalcs obj) : this()
        {
            DesignWorked = obj.DesignWorked;
            foreach(int key in obj.BracketAnalysisResults.Keys)
            {
                var brktResult = new BracketControllingResultDTO(obj.BracketAnalysisResults[(BracketAnalysis.BracketStressType) key]);
                BracketAnalysisResults.Add(key, brktResult);
            }

            foreach(BracketAnalysis.BracketLoad load in obj.ControllingLoadcaseList)
            {
                ControllingLoadcaseList.Add(new BracketLoadDTO(load));
            }

            foreach(string key in obj.BracketLoadItems.Keys)
            {
                List<BracketLoadDTO> dtoList = new List<BracketLoadDTO>();
                foreach (BracketAnalysis.BracketLoad load in obj.BracketLoadItems[key])
                {
                    BracketLoadDTO brktLst = new BracketLoadDTO(load);
                    dtoList.Add(brktLst);
                }

                BracketLoadItems.Add(key, dtoList);

            }
        }

        #endregion  // Constructors

        #region Properties

        /// <summary>
        /// This bracket design works for the given loads (ie not overstressed and W/t within limits).  
        /// Look in ControllingLoadCase to find out which one.
        /// </summary>
        public bool DesignWorked { get; set; }

        /// <summary>
        /// The controlling load case by BracketStressType.
        /// </summary>
        public Dictionary<int, BracketControllingResultDTO> BracketAnalysisResults { get; set; } = new Dictionary<int, BracketControllingResultDTO>();

        /// <summary>
        /// List of unique controlling load cases for this bracket.
        /// </summary>
        public List<BracketLoadDTO> ControllingLoadcaseList { get; set; } = new List<BracketLoadDTO>();

        public Dictionary<string, List<BracketLoadDTO>> BracketLoadItems { get; set; } = new Dictionary<string, List<BracketLoadDTO>>();

        #endregion  // Properties

    }
}
