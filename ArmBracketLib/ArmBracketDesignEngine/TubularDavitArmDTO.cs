namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class TubularDavitArmDTO : TubularArmDTO
    {
        public TubularDavitArmDTO()
        {
        }

        public TubularDavitArmDTO(StructureComponents.Arms.TubularDavitArm da) : base(da)
        {
            RiseFeet = da.RiseFeet;
            Bent = da.Bent;
            Radius = da.Radius;
        }

        public double RiseFeet { get; set; }

        public bool Bent { get; set; }

        public double Radius { get; set; }
    }
}