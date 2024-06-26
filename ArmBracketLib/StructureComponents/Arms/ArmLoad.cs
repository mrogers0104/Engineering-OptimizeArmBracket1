using System;

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{
    public class ArmLoad
    {
        #region Constructors

        public ArmLoad(Guid guid)
        {
            Id = guid;
        }

        public ArmLoad() : this(Guid.NewGuid())
        {
        }

        #endregion  // Constructors
        public Guid Id { get; }

        public string LoadCase { get; set; } = string.Empty;

        public string Label { get; set; } = string.Empty;

        public double RelDist { get; set; }

        public string JointLabel { get; set; } = string.Empty;

        public double LongitudinalOffset { get; set; }

        public double TransverseOffset { get; set; }

        public double VerticalOffset { get; set; }

        /// <summary>
        /// Axial force, kips
        /// Hock's  bracket analsysis spreadsheet: Vert., Fv
        /// </summary>
        public double AxialForce { get; set; }

        /// <summary>
        /// Vertical shear force, kips
        /// Hock's  bracket analsysis spreadsheet: Trans., FT
        /// </summary>
        public double VerticalShear { get; set; }

        /// <summary>
        /// Horizontal Shear force, kips
        /// Hock's  bracket analsysis spreadsheet: Long., FL
        /// </summary>
        public double HorizontalShear { get; set; }

        /// <summary>
        /// Horizontal moment, ft-kips
        /// Hock's  bracket analsysis spreadsheet: Long., ML
        /// </summary>
        public double HorizontalMoment { get; set; }

        /// <summary>
        /// Vertical moment, ft-kips
        /// Hock's  bracket analsysis spreadsheet: Vert., Mv
        /// </summary>
        public double VerticalMoment { get; set; }

        /// <summary>
        /// Torsion, ft-kips
        /// Hock's bracket analsysis spreadsheet: Trans., FT
        /// </summary>
        public double Torsion { get; set; }



        #region Methods

        public override string ToString()
        {
            string msg = $"JointLbl {JointLabel}: RelDist = {RelDist} ft && VertOffset = {VerticalOffset} ft";
            return msg;
        }

        #endregion  // Methods

    }
}