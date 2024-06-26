using ArmBracketDesignLibrary.Helpers;
using System;
using System.Linq;

// 

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{
    [Serializable]
    public class TubularDavitArm : TubularArm
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TubularDavitArm() //Guid guid) : base(guid)
        {
        }

        public TubularDavitArm(ArmBracketDesignEngine.ArmBracketDesignInput di, ArmProject parent) : base(di)
        {
            Parent = parent;
        }

        public override ArmType Armtype => ArmType.TubularDavitArm;

        public double RiseFeet
        {
            get
            {
                if (ArmJoints == null || ArmJoints.Count == 0)
                {
                    return 0d;
                }

                double horizOffset = ArmJoints.Max(o => Math.Abs(o.HorizontalOffset));  // end (tip) of arm.
                int idx = ArmJoints.FindIndex(o => o.HorizontalOffset == horizOffset);
                if (idx < 0)
                {
                    return 0d;
                }

                // *** The QT Backend writes the inverse of the vertical offset to the POST.
                double rise = ArmJoints[idx].VerticalOffset * -1;

                return rise;
                //return ArmJoints.Max(o => Math.Abs(o.VerticalOffset));

            }
        }

        /// <summary>
        /// Bent should be true for radius and gull wing arms.
        /// </summary>
        public bool Bent
        {
            get
            {
                if (ArmJoints.Count <= 2)
                {
                    return false;
                }

                double slope = (ArmJoints[1].VerticalOffset - ArmJoints[0].VerticalOffset) /
                               (ArmJoints[1].HorizontalOffset - ArmJoints[0].HorizontalOffset);

                for (int counter = 1; counter < ArmJoints.Count - 1; counter++)
                {
                    double nextSlope = (ArmJoints[counter + 1].VerticalOffset - ArmJoints[counter].VerticalOffset) /
                                       (ArmJoints[counter + 1].HorizontalOffset - ArmJoints[counter].HorizontalOffset);

                    if (!slope.AreEqual(nextSlope))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public double Radius { get; set; }

        public override string ToString()
        {
            string msg = $"{AttachLabel} @ {Dft:f2}' ({Azimuth} deg) :: butt = {BaseFlatWidthInches:f2} x {MaterialThicknessInches:f4} x {PLSLengthFt:f2}'";
            return msg;
        }

    }
}