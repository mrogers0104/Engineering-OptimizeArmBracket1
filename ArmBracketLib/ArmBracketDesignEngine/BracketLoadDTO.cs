using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary.BracketAnalysis;

// 

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class BracketLoadDTO
    {

        public BracketLoadDTO()
        {

        }

        internal BracketLoadDTO(BracketAnalysis.BracketLoad obj)
        {
            Id = obj.Id;
            LoadCase = obj.LoadCase;
            Label = obj.Label;
            RelDist = obj.RelDist;
            JointLabel = obj.JointLabel;
            LongitudinalOffset = obj.LongitudinalOffset;
            TransverseOffset = obj.TransverseOffset;
            VerticalOffset = obj.VerticalOffset;
            AxialForce = obj.AxialForce;
            VerticalShear = obj.VerticalShear;
            HorizontalShear = obj.HorizontalShear;
            HorizontalMoment = obj.HorizontalMoment;
            VerticalMoment = obj.VerticalMoment;
            Torsion = obj.Torsion;
            LoadsAtPoleFace = new ArmLoadDTO(obj.LoadsAtPoleFace);

            foreach(int key in obj.ControllingStressTypeResults.Keys)
            {
                ControllingStressTypeResults.Add(key, obj.ControllingStressTypeResults[(BracketStressType)key]);
            }

            GoverningElement = obj.GoverningElement;
        }


        public Guid Id { get; set; }

        public ArmLoadDTO LoadsAtPoleFace {get;set;}

        public Dictionary<int, bool> ControllingStressTypeResults { get; set; } = new Dictionary<int, bool>();
        public string LoadCase { get; set; } 

        public string Label { get; set; }

        public double RelDist { get; set; }

        public string JointLabel { get; set; }

        public double LongitudinalOffset { get; set; }

        public double TransverseOffset { get; set; }

        public double VerticalOffset { get; set; }

        public double AxialForce { get; set; }

        public double VerticalShear { get; set; }

        public double HorizontalShear { get; set; }

        public double HorizontalMoment { get; set; }

        public double VerticalMoment { get; set; }

        public double Torsion { get; set; }

        /// <summary>
        /// The governing element for this load: Bracket stress, Shear Rupture, Bearing, Bolt ...
        /// This property is used to display load case values for the Bracket Analysis Report.
        /// </summary>
        public string GoverningElement { get; set; }


    }
}
