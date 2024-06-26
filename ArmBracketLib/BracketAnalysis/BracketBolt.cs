using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary.StructureComponents;


namespace ArmBracketDesignLibrary.BracketAnalysis
{
    /// <summary>
    /// Define a bolt in one quadrant of the bracket leg.
    /// </summary>
    public class BracketBolt
    {

        private SaddleBracket _bracket = null;

        public double _centerSpace = 0;    // c
        private double _boltSpacing = 0;    // s

        #region Constructors
        public BracketBolt(int boltNum, SaddleBracket bracket)
        {
            BoltNum = boltNum;
            _bracket = bracket;

            _centerSpace = _bracket.GetBktBoltCenterSpace(); // c
            _boltSpacing = _bracket.BoltSpacing; // s

        }

        #endregion

        #region Properties

        /// <summary>
        /// Bolt number from center line of bracket.
        /// </summary>
        public int BoltNum { get; set; }

        private double _distFromCenter = 0;
        /// <summary>
        /// Distance from center of bracket leg to first bolt.
        /// </summary>
        public double DistFromCenter
        {
            get
            {
                double d = _distFromCenter == 0 ? _centerSpace + _boltSpacing * BoltNum : _distFromCenter;

                return d;
            }
            set
            {
                _distFromCenter = value;
            }
        }

        public double DistSqFromCenter { get { return DistFromCenter * DistFromCenter; } }


        /// <summary>
        /// Shear stress along x (Longitudinal), ksi
        /// </summary>
        public double ShearStressXksi { get; set; }

        /// <summary>
        /// Shear stress along z (Translational), ksi
        /// </summary>
        public double ShearStressZksi { get; set; }

        /// <summary>
        /// Combined shear stress, ksi
        /// </summary>
        public double ShearCombinedStressKsi { get { return Math.Sqrt(ShearStressXksi * ShearStressXksi + ShearStressZksi * ShearStressZksi); } }

        public double ShearForceKips { get; set; }

        public double MomentFtKips { get; set; }

        /// <summary>
        /// The bracket leg moment arm, in.
        /// Used to compute bracket stresses.
        /// </summary>
        public double BracketMomArm { get; set; }

        /// <summary>
        /// The bracket leg moment, ft-kips
        /// </summary>
        public double BracketMomentFtKips { get; set; }

        #endregion


        #region Methods

        public override string ToString()
        {
            string msg = $"Bolt #{BoltNum} @ {DistFromCenter}\" Shear Force = {ShearForceKips:f2} kips";
            return msg;
        }

        #endregion
    }
}
