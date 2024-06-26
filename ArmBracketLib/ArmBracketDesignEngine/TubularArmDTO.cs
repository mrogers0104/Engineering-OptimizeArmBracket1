using ArmBracketDesignLibrary.Materials;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class TubularArmDTO
    {
        public TubularArmDTO()
        {

        }

        public TubularArmDTO(StructureComponents.Arms.TubularArm ta)
        {
            Dft = ta.Dft;
            AttachLabel = ta.AttachLabel;
            Azimuth = ta.Azimuth;
            DavitPropertyLabel = ta.DavitPropertyLabel;
            Armtype = (int)ta.Armtype;
            PoleDiameterIn = ta.PoleDiameterIn;
            UseArmPoleOffsets = ta.UseArmPoleOffsets;
            DesignMethodSpecified = ta.DesignMethodSpecified.ToString();
            DesignMethodUsed = ta.DesignMethodUsed.ToString();
            TubeStrength = ta.TubeStrength;
            TubeStrength = (TubeStrength <= 0 ? 65.0 : TubeStrength);

            foreach (StructureComponents.Arms.ArmJoint aj in ta.ArmJoints)
            {
                ArmJoints.Add(new ArmJointDTO(aj));
            }


            foreach (StructureComponents.Arms.ArmJoint aj in ta.NonUserArmJoints)
            {
                NonUserArmJoints.Add(new ArmJointDTO(aj));
            }

            TipHorizontalOffset = ta.TipHorizontalOffset;
            TipVerticalOffset = ta.TipVerticalOffset;

            foreach (string ak in ta.ArmLoads.Keys)
            {
                foreach (StructureComponents.Arms.ArmLoad al in ta.ArmLoads[ak])
                {
                    if (ArmLoads.ContainsKey(ak))
                    {
                        ArmLoads[ak].Add(new ArmLoadDTO(al));
                    }
                    else
                    {
                        ArmLoads.Add(ak, new List<ArmLoadDTO> { new ArmLoadDTO(al) });
                    }
                }
            }

            Label = ta.Label;
            //MaterialSpecification = (int)ta.MaterialSpecification;
            TipFlatWidthInches = ta.TipFlatWidthInches;
            BaseFlatWidthInches = ta.BaseFlatWidthInches;
            MaterialThicknessInches = ta.MaterialThicknessInches;
            ShapeName = ta.Shape.Id;
            Finish = (int)ta.Finish;
            LengthFt = ta.PLSLengthFt;

            if(ta is TubularDavitArm)
            {
                DavitRadius = (ta as TubularDavitArm).Radius;
                DavitRiseFeet = (ta as TubularDavitArm).RiseFeet;
                DavitBent = (ta as TubularDavitArm).Bent;
            }

            if(ta is TubularXArm)
            {
                XArmConnectionMethod = (ta as TubularXArm).ConnectionType;
            }

            Weight = ta.Weight;

        }

        public int Armtype { get; set; }
        public string AttachLabel { get; set; }
        public double Azimuth { get; set; }
        public double BaseFlatWidthInches { get; set; }
        public bool DavitBent { get; set; }
        public string DavitPropertyLabel { get; set; }
        public double DavitRadius { get; set; }
        public double DavitRiseFeet { get; set; }
        /// <summary>
        /// The bracket design method specified for this arm.
        ///     UNKN = 0,
        ///     STND = 1,
        ///     OVRD = 2, 
        ///     NOHX = 3,
        ///     DSGN = 99,
        ///     FAIL = 999
        /// </summary>
        public string DesignMethodSpecified { get; set; }

        /// <summary>
        /// The bracket design method used for this arm.
        ///     UNKN = 0,
        ///     STND = 1,
        ///     OVRD = 2, 
        ///     NOHX = 3,
        ///     DSGN = 99,
        ///     FAIL = 999
        /// </summary>
        public string DesignMethodUsed { get; set; }

        public double Dft { get; set; }
        public int Finish { get; set; }
        public string Label { get; set; }
        public double LengthAdjustedForBracketFt { get; set; }
        public double LengthFt { get; set; }
        /// <summary>
        /// The arm material specification.
        ///       None = 0,
        ///       A572_50 = 1,
        ///       A572_65 = 2,
        ///       A871_65 = 3,
        ///       A588_50 = 4,
        ///       A588_46 = 5,
        ///       A572_42 = 6,
        ///       A588_60 = 7,
        ///       A633_46 = 8,
        ///       A633_50 = 9,
        ///       A633_60 = 10,
        ///       A36 = 11 
        /// </summary>
        //public int MaterialSpecification { get; set; }

        public double MaterialThicknessInches { get; set; }
        public double PoleDiameterIn { get; set; }
        
        /// <summary>
        /// 4F = 4 sides, Square (Flat)
        /// 4T = 4 sides, Square (Tip)
        /// 6F = 6 sides (Flat)
        /// 6T = 6 sides (Tip)
        /// 8F = 8 sides (Flat)
        /// 8T = 8 sides (Tip)
        /// 12F = 12 sides (Flat)
        /// 12T = 12 sides (Tip)
        /// 16F = 16 sides  (Flat)
        /// 16T = 16 sides (Tip)
        /// 18 = 18 sides (Tip)
        /// 121 = 12 sides, ellipse (3/5)
        /// 122 = 12 sides, ellipse (3/5)
        /// 123 = 12 sides, ellipse (2/3)
        /// 124 = 12 sides, ellipse (2/3)
        /// 125 = 12 sides, ellipse (3/4)
        /// 126 = 12 sides, ellipse (3/4)
        /// </summary>
        public string ShapeName { get; set; }
        public double TipFlatWidthInches { get; set; }
        public double TipHorizontalOffset { get; set; }
        public double TipVerticalOffset { get; set; }
        /// <summary>
        /// Yield strength of the tube (arm)
        /// </summary>
        public double TubeStrength { get; set; }

        public bool UseArmPoleOffsets { get; set; }
        /// <summary>
        /// Yield strength of the tube (arm)
        /// </summary>
        public double Weight { get; set; }
        public double WeightAdjustedForBracket { get; set; }

        public Helpers.ArmConnectionMethod XArmConnectionMethod { get; set; }
        public override string ToString()
        {
            string msg = $"{AttachLabel} @ {Dft:f2}' ({Azimuth} deg) :: butt = {BaseFlatWidthInches:f2} x {MaterialThicknessInches:f4} x {LengthFt:f2}'";
            return msg;
        }
        public SteelShapeDTO Shape
        {
            get
            {
                if (ShapeName != null)
                {
                    SteelShape sh = SteelShape.SteelShapes.Find(s => s.Id.Equals(ShapeName));
                    if (sh != null)
                    {
                        return new SteelShapeDTO
                        {
                            Description = sh.Description,
                            Id = sh.Id,
                            IsRegularPolygon = sh.IsRegularPolygon,
                            IsTubing = sh.IsTubing,
                            SideCount = sh.SideCount,
                            TubeOrientation = (int)sh.TubeOrientation
                        };
                    }
                }
                return null;
            }
        }
        public List<ArmJointDTO> NonUserArmJoints { get; set; }
        public List<ArmJointDTO> ArmJoints { get; set; } = new List<ArmJointDTO>();
        public Dictionary<string, List<ArmLoadDTO>> ArmLoads { get; set; } = new Dictionary<string, List<ArmLoadDTO>>();

    }

    //public class ArmShapeDTO {

    //}

}
