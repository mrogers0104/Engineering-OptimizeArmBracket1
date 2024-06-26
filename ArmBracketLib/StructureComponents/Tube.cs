using System;
using System.Runtime.Serialization;
using ArmBracketAnalysisLib.Helpers;
using ArmBracketAnalysisLib.Materials;
using ArmBracketAnalysisLib.StructureComponents.Arms;
//using LUtility.LMath;
using MaterialsLibrary;
//using Sts.Materials;

namespace ArmBracketAnalysisLib.StructureComponents
{
    /// <summary>
    /// !!!! NO LONGER NEEDED !!!!
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class Tube
    {
        #region Delegates

        public delegate void DataChangedHandler();

        #endregion


        #region Constructors

        

        protected Tube() : this(Guid.NewGuid())
        {
        }

        protected Tube(Guid guid)
        {
            Id = guid;
        }

        #endregion  // Constructors

        #region Properties

        public Guid Id { get; }

        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// The arm material specification.
        /// </summary>
        public virtual PlateMaterialSpecification MaterialSpecification { get; set; }


        public double TipFlatWidthInches { get; set; }


        public double BaseFlatWidthInches { get; set; }


        public double MaterialThicknessInches { get; set; }

        public SteelShape Shape { get; set; }


        public PlateFinish Finish { get; set; }

        /// <summary>
        /// Initially, this will contain the PLS arm length.
        /// However, in <see cref="Structure.ModifyArmToMatchQt(TubularArm)"/>, 
        /// the arm length is modified so the arm weight more closely matches the QT arm wt.
        /// </summary>
        public double LengthFt { get; set; }

        #endregion  // Properties

        #region Methods

        public override string ToString()
        {
            return
                $"Tube: {TipFlatWidthInches:f2}\"X{BaseFlatWidthInches:f2}\"X{MaterialThicknessInches:f4}\"X{LengthFt}\'";
        }

        #endregion  // Methods

    }
}