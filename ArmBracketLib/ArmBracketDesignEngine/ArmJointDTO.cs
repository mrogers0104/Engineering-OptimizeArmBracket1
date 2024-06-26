using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class ArmJointDTO
    {
        public ArmJointDTO()
        {
            Id = Guid.NewGuid();
        }

        internal ArmJointDTO(StructureComponents.Arms.ArmJoint aj)
        {
            Id = aj.Id;
            JointLabel = aj.JointLabel;
            JointPosition = aj.JointPosition;
            HorizontalOffset = aj.HorizontalOffset;
            VerticalOffset = aj.VerticalOffset;
            Diameter = aj.Diameter;
            UserDefined = aj.UserDefined;
        }

        public Guid Id { get; set; }

        public string JointLabel { get; set; }

        public string JointPosition { get; set; }

        public double HorizontalOffset { get; set; }

        public double VerticalOffset { get; set; }

        public double Diameter { get; set; }

        public bool UserDefined { get; set; } = true;


        public override string ToString()
        {
            string msg = $"{JointLabel} :: dia = {Diameter:f2}\" -> Offset :: Horiz = {HorizontalOffset:f2}' Vert = {VerticalOffset * 12d:f2}\"";
            return msg;
        }


    }
}
