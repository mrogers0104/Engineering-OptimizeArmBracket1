using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class ArmLoadDTO
    {
        public ArmLoadDTO()
        {

        }

        internal ArmLoadDTO(StructureComponents.Arms.ArmLoad obj)
        {
            Id = obj.Id;
            LoadCase = obj.LoadCase;
            Label = obj.Label;
            RelDist_Feet = obj.RelDist;
            JointLabel = obj.JointLabel;
            LongitudinalOffset = obj.LongitudinalOffset;
            TransverseOffset = obj.TransverseOffset;
            VerticalOffset_Feet = obj.VerticalOffset;
            AxialForce = obj.AxialForce;
            VerticalShear = obj.VerticalShear;
            HorizontalShear = obj.HorizontalShear;
            HorizontalMoment = obj.HorizontalMoment;
            VerticalMoment = obj.VerticalMoment;
            Torsion = obj.Torsion;
        }

        public Guid Id { get; set; }

        public string LoadCase { get; set; }

        public string Label { get; set; }

        public double RelDist_Feet { get; set; }

        public string JointLabel { get; set; }

        public double LongitudinalOffset { get; set; }

        public double TransverseOffset { get; set; }

        public double VerticalOffset_Feet { get; set; }
        public double AxialForce { get; set; }

        public double VerticalShear { get; set; }

        public double HorizontalShear { get; set; }

        public double HorizontalMoment { get; set; }

        public double VerticalMoment { get; set; }

        public double Torsion { get; set; }


    }
}
