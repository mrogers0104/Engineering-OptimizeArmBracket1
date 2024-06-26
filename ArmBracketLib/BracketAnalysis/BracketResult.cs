using System;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    /// <summary>
    /// 
    /// </summary>
    public class BracketResult : IEquatable<BracketResult>
    {

        #region Constructors
        public BracketResult()
        {

        }

        /// <summary>
        /// The bracket result
        /// </summary>
        /// <param name="val">The actual value.</param>
        /// <param name="allowable">The allowable value</param>
        public BracketResult(double val, double allowable)
        {
            Value = val;
            Allowable = allowable;
        }

        /// <summary>
        /// The bracket result
        /// </summary>
        /// <param name="val">The actual value.</param>
        /// <param name="allowable">The allowable value</param>
        /// <param name="description">The description of this results</param>
        public BracketResult(double val, double allowable, string description):this(val, allowable)
        {
            Description = description;
        }

        #endregion  // Constructors

        public string Description { get; set; }

        /// <summary>
        /// The actual value: stress, thickness, etc.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The allowable value: stress, thickness, etc.
        /// </summary>
        public double Allowable { get; set; }

        /// <summary>
        /// The interaction ratio percent
        /// </summary>
        public string Ratio
        {
            get
            {
                double ratio = InteractionRatio * 100.0;
                return $"{ratio:f2}%";
            }
        }

        /// <summary>
        /// The interaction ratio.  Expecting a ratio &le; 1.0
        /// </summary>
        public double InteractionRatio
        {
            get
            {
                if (Allowable == 0)
                {
                    return 0.0;
                }

                double ratio = Value / Allowable;

                return ratio;
            }

        }

        /// <summary>
        /// Message indicating the results of this value.
        /// </summary>
        public string ResultsMsg
        {
            get
            {
                string alert = (IsOverstressed ? "is overstressed: " : "works: ") + Ratio;
                string msg = (string.IsNullOrEmpty(Description) ? string.Empty : $"{Description} {alert}");

                return msg;
            }
        }

        /// <summary>
        /// Do a deep copy of BracketResult source.
        /// </summary>
        /// <param name="src">The source object.</param>
        /// <returns></returns>
        public BracketResult Copy()
        {
            BracketResult target = new BracketResult();
            target.Allowable = this.Allowable;
            target.Description = this.Description;
            target.Value = this.Value;

            return target;
        }

        /// <summary>
        /// Is this result overstress?  That is, is the Interaction Ratio > 1.0?
        /// </summary>
        public bool IsOverstressed { get { return (InteractionRatio > 1.0); } }

        public override string ToString()
        {
            string msg = $"{Value:f2} / {Allowable:f2} = {Ratio}";

            return msg;
        }

        #region IEquatable Interface

        #endregion
        public bool Equals(BracketResult other)
        {
            bool eq = this.Description.Equals(other.Description);

            return eq;
        }
    }
}
