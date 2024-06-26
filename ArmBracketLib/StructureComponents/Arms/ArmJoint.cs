using System;

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{
    public class ArmJoint
    {


        public ArmJoint(Guid guid)
        {
            Id = guid;
        }

        public ArmJoint() : this(Guid.NewGuid())
        {
        }

        internal ArmJoint(ArmBracketDesignEngine.ArmJointDTO dto)
        {
            Id = dto.Id;
            JointLabel = dto.JointLabel;
            JointPosition = dto.JointPosition;

            HorizontalOffset = dto.HorizontalOffset;
            VerticalOffset = dto.VerticalOffset;
            Diameter = dto.Diameter;
            UserDefined = dto.UserDefined;
        }

        public Guid Id { get; set; }

        public string JointLabel { get; set; } = string.Empty;

        public string JointPosition { get; set; } = string.Empty;

        public double HorizontalOffset { get; set; }

        public double VerticalOffset { get; set; }

        public double Diameter { get; set; }

        public bool UserDefined { get; set; } = true;


        #region Methods

        public override string ToString()
        {
            return $"{JointLabel}: dia={Diameter:f2}\" :: Offset(h X v) - ({HorizontalOffset:f2} X {VerticalOffset})";
        }

        #endregion
    }
}