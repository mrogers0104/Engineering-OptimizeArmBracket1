

// 

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class DesignEngine
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public DesignEngine()
        {
        }

        #endregion  // Constructors

        #region Properties


        #endregion  // Properties

        #region Methods

        public static ArmBracketDesignResults RunDesigns(ArmBracketDesignInputBundle bracketInputs)
        {
            return new ArmBracketDesignResults(bracketInputs.Inputs);
          
        }

        #endregion  // Methods
    }
}
