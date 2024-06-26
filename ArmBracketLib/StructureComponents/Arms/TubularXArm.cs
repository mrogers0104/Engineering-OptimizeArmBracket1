using ArmBracketDesignLibrary.Helpers;

// 

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{
    public class TubularXArm : TubularArm
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TubularXArm(ArmBracketDesignEngine.ArmBracketDesignInput di, ArmProject parent) : base(di) 
        {
            Parent = parent;
        }

        #region Implement TubularArm

        public override ArmType Armtype => ArmType.TubularCrossArm;

        #endregion Implement TubularArm

        public ArmConnectionMethod ConnectionType { get; set; } = ArmConnectionMethod.Fixed;
    }
}