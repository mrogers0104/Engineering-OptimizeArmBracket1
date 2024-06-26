using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{
    public enum ArmType
    {
        None = 0,
        TubularDavitArm = 1,
        TubularCrossArm = 2
    }

    public abstract class TubularArm : IComparable<TubularArm>
    {
        protected TubularArm()
        {
            DesignMethodUsed = BracketDesignMethod.UNKN;
        }

        protected TubularArm(ArmBracketDesignEngine.ArmBracketDesignInput di)
        {
            Dft = (double)di.ValuesFromPLSData.ArmDft;
            AttachLabel = di.ValuesFromPLSData.ArmAttachLabel;
            Azimuth = (double)di.UserInputs.ArmAzimuth;
            DavitPropertyLabel = di.ValuesFromPLSData.ArmPropertyLabel;
            PoleDiameterIn = (double)di.ValuesFromPLSData.PoleDiameterIn;

            DesignMethodSpecified = di.UserInputs.CustomBracketInput.AllNull()
                ? BracketDesignMethod.STND
                : BracketDesignMethod.OVRD;

            DesignMethodUsed = BracketDesignMethod.UNKN;

            Finish = (PlateFinish)Enum.Parse(typeof(PlateFinish), di.UserInputs.ArmFinish);
            Label = di.ValuesFromPLSData.ArmLabel;

            //string ms = di.UserInputs.ArmMaterialSpecification.Replace("-", "_");
            //MaterialSpecification = (PlateMaterialSpecification)Enum.Parse(typeof(PlateMaterialSpecification), ms);
            TipFlatWidthInches = (double)di.ValuesFromPLSData.ArmTipFlatWidthInches;

            foreach (ArmBracketDesignEngine.ArmJointDTO ajd in di.ValuesFromPLSData.ArmJoints)
            {
                ArmJoints.Add(new ArmJoint(ajd));
            }

            Shape = SteelShape.SteelShapes.Find(s => s.Id.Equals(di.ValuesFromPLSData.ArmShapeName));
            BaseFlatWidthInches = (double)di.ValuesFromPLSData.ArmBaseFlatWidthInches;
            DavitPropertyLabel = di.ValuesFromPLSData.ArmPropertyLabel;
            MaterialThicknessInches = (double)di.ValuesFromPLSData.ArmMaterialThicknessInches;

            PLSLengthFt = (double)di.ValuesFromPLSData.ArmLengthFt;
            TipHorizontalOffset = (double)di.ValuesFromPLSData.ArmTipHorizontalOffset;
            TipVerticalOffset = (double)di.ValuesFromPLSData.ArmTipVerticalOffset;

            TubeStrength = (double)di.ValuesFromPLSData.ArmTubeStrengthKsi;

            foreach (var ad in di.ValuesFromPLSData.ArmLoads)
            {
                var newl = new ArmLoad
                {
                    AxialForce = ad.AxialForce,
                    HorizontalMoment = ad.HorizontalMoment,
                    HorizontalShear = ad.HorizontalShear,
                    JointLabel = ad.JointLabel,
                    Label = ad.Label,
                    LoadCase = ad.LoadCase,
                    LongitudinalOffset = ad.LongitudinalOffset,
                    RelDist = ad.RelDist_Feet,
                    Torsion = ad.Torsion,
                    TransverseOffset = ad.TransverseOffset,
                    VerticalMoment = ad.VerticalMoment,
                    VerticalOffset = ad.VerticalOffset_Feet,
                    VerticalShear = ad.VerticalShear
                };

                if (ArmLoads.ContainsKey(ad.LoadCase))
                {
                    ArmLoads[ad.LoadCase].Add(newl);
                }
                else
                {
                    ArmLoads.Add(ad.LoadCase, new List<ArmLoad>() { newl });
                }

            }
        }


        [JsonIgnore]
        public ArmProject Parent { get; set; }

        public double Dft { get; set; }

        public string AttachLabel { get; set; } = string.Empty;

        public double Azimuth { get; set; }

        public string DavitPropertyLabel { get; set; } = string.Empty;

        //public Structure Parent { get; set; }

        public List<ArmJoint> ArmJoints { get; set; } = new List<ArmJoint>();

        public Dictionary<string, List<ArmLoad>> ArmLoads { get; set; } = new Dictionary<string, List<ArmLoad>>();

        /// <summary>
        /// The bracket design method specified for this arm.
        /// </summary>
        public BracketDesignMethod DesignMethodSpecified { get; set; } = BracketDesignMethod.STND;

        /// <summary>
        /// The bracket design method used for this arm.
        /// </summary>
        public BracketDesignMethod DesignMethodUsed { get; set; } = BracketDesignMethod.UNKN;

        public List<ArmJoint> NonUserArmJoints { get; } = new List<ArmJoint>();

        public abstract ArmType Armtype { get; }

        /// <summary>
        /// Yield strength of the tube (arm)
        /// </summary>
        public double TubeStrength { get; set; }

        /// <summary>
        /// The pole diameter at the center line of the thruplate in inches.
        /// </summary>
        public double PoleDiameterIn { get; set; }


        /// <summary>
        /// Use tubular arm to pole offsets
        /// </summary>
        public bool UseArmPoleOffsets
        {
            get
            {
                return Parent?.UseArmPoleOffsets ?? false;
            }
        }

        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// The arm material specification.
        /// </summary>
        //public virtual PlateMaterialSpecification MaterialSpecification { get; set; }

        public double TipFlatWidthInches { get; set; }

        public double BaseFlatWidthInches { get; set; }

        public double MaterialThicknessInches { get; set; }

        public SteelShape Shape { get; set; }

        public PlateFinish Finish { get; set; }

        public double Weight
        {
            get
            {
                double sideWidth = RegularPolygon.GetSideLength(Shape.SideCount, AverageCircumRadius);
                double volume = sideWidth.InchesToFeet() * PLSLengthFt * Shape.SideCount *
                                MaterialThicknessInches.InchesToFeet();
                return Math.Round(volume * 490, 2);
            }
        }

        /// <summary>
        /// The PLS arm length in feet
        /// </summary>
        public double PLSLengthFt { get; set; }

        public double TipVerticalOffset { get; set; }

        public double TipHorizontalOffset { get; set; }
        public double AverageCircumRadius
        {
            get
            {
                int sideCount = Shape.SideCount;
                double averageApothem = (BaseFlatWidthInches / 2d + TipFlatWidthInches / 2d) / 2d;
                return RegularPolygon.GetCircumradius(sideCount, averageApothem);
            }
        }

        public override string ToString()
        {
            string msg = $"{AttachLabel} @ {Dft:f2}' ({Azimuth} deg) :: butt = {BaseFlatWidthInches:f2} x {MaterialThicknessInches:f4} x {PLSLengthFt:f2}'";
            return msg;
        }

        public int CompareTo(TubularArm other)
        {
            return Dft.CompareTo(other.Dft);
        }
    }
}