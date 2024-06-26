using System;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    /// <summary>
    /// Contains stiffener properties: I, y, C, etc.
    /// </summary>
    public class StiffenerGroup
    {
        //private TubularDavitArm _arm = null;
        private SaddleBracket _bracket = null;
        private ArmConnection _connection = null;
        private BracketBoltGroup _bktBoltGroup = null;

        private double _Tb = 0;     // bracket thickness, in.
        private double _Tt = 0;     // thru plate thickness, in.
        private double _H = 0;      // thru plate (bracket) height, in.
        private double _Ts = 0;     // stiffener thickness, in.
        private double _hs = 0;     // min stiffener width, in.
        private double _Hh = 0;     // ??
        private int _boltQty = 0;   // bracket bolt qty

        #region Constructors

        public StiffenerGroup(SaddleBracket bracket, BracketBoltGroup bracketBoltGroup)
        {
            _bracket = bracket;
            _connection = _bracket.Parent;
            _bktBoltGroup = bracketBoltGroup;

            _Tb = _bracket.BracketThick;
            _Tt = _connection.Thickness;
            _H = _bracket.Height;
            _Ts = _bracket.StiffenerThick;
            _hs = _bracket.StiffenerWidth;
            _boltQty = _bracket.BoltQty;

            _Hh = (_boltQty == 1 ? Math.Min(_H, 24.0 * _Tb) : _H / 2.0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Distance to the centroid of the stiffener group from 
        /// the center of the bracket, in.
        /// </summary>
        public double Centroid { get { return ComputeCentroid(); } }

        /// <summary>
        /// Distance to outer fiber of stiffener
        /// from the center of the bracket, in.
        /// </summary>
        public double DistOuterFiber { get { return ComputeDistOutFiber(); } }

        /// <summary>
        /// Moment of inertia about the bracket center line, in^4
        /// </summary>
        public double I { get { return ComputeMomentOfInertia(); } }

        public double StressKsi { get { return ComputeStiffenerStress(); } }

        #endregion

        #region Methods

        /// <summary>
        /// Compute the centroid of the stiffener group.
        /// </summary>
        /// <returns></returns>
        private double ComputeCentroid()
        {
            double num = ((_Tt * _Tt * _H) / 4.0 + _Ts * _hs * (_Tt + _hs / 2.0));
            double den = (_H * _Tt) / 2.0 + _Ts * _hs;

            if (den == 0)
            {
                return 0;
            }

            double y = num / den;

            return y;
        }

        /// <summary>
        /// Compute the distance to the outer fiber of the stiffener group, in.
        /// </summary>
        /// <returns></returns>
        private double ComputeDistOutFiber()
        {
            double y = Centroid;
            double C = _Tt + _hs - y;

            return C;
        }

        /// <summary>
        /// Compute the moment of inertia of the stiffener group, in^4.
        /// </summary>
        /// <returns></returns>
        private double ComputeMomentOfInertia()
        {
            double y = Centroid;
            double p1 = (_Tt / 2.0) - y;
            double p2 = _Tt + (_hs / 2.0) - y;

            double I = _Hh * _Tt * _Tt * _Tt / 12.0;
            I += _Ts * _hs * _hs * _hs / 12.0;
            I += _Hh * _Tt * p1 * p1;
            I += _Ts * _hs * p2 * p2;

            if (_boltQty == 1)
            {
                I += _Ts * _hs * _hs * _hs / 12.0;
                I += _Ts * _hs * p2 * p2;
            }

            return I;
        }

        /// <summary>
        /// Compute the stress in the stiffener, ksi;
        /// </summary>
        /// <returns></returns>
        private double ComputeStiffenerStress()
        {
            double sumMoment = Math.Abs(_bktBoltGroup.SumMomentFtKip);
            double C = DistOuterFiber;

            double stress = sumMoment * C / I;

            if (_boltQty == 1)
            {
                stress /= 2.0;
            }

            return stress;
        }

        #endregion
    }
}
