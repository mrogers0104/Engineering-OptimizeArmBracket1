using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using ArmBracketDesignLibrary.BracketAnalysis;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using System;
using System.Collections.Generic;

namespace ArmBracketDesignLibrary
{
    [Serializable]
    public class ArmProject
    {
        #region Constructors

        public ArmProject()
        {
            //SavedBy = string.Empty;
            //SavedDate = DateTime.MinValue;

            AnalysisWarningMessages = new List<Message>();
            ProjectDescription = string.Empty;
            Customer = string.Empty;
            Label = string.Empty;
            AllowA354BCBolts = false;
            AllowCenterBolts = false;
        }

        #endregion Constructors

        #region Properties

        public bool AllowA354BCBolts { get; set; }

        public bool AllowArmCoping
        {
            get
            {
                //2017-03-06 no more arm coping per Hock
                return false;
            }
        }

        public List<Message> AnalysisWarningMessages { get; set; }
        public bool AllowCenterBolts { get; set; }

        /// <summary>
        /// The plate finish for this arm.
        /// </summary>
        public PlateFinish ArmPlateFinish { get; set; }

        public int ArmsAtLocation { get; set; }
        public PlateMaterialSpecification BracketMaterialSpecification { get; set; }
        public PlateFinish BracketFinish { get; set; }
        public string Customer { get; set; }
        public string Label { get; set; }

        /// <summary>
        /// Pole Diameter at center of arm bracket
        /// </summary>
        public decimal PoleOuterDiameterIn { get; set; }

        public int PoleSideCount { get; set; }
        public decimal PoleTaper { get; set; }
        public decimal PoleWallThickness { get; set; }
        public string ProjectDescription { get; set; }
        public SaddleBracket SaddleBracket { get; set; }
        public ThruPlateConnection ThruPlateConnection { get; set; }

        public TubularDavitArm TubularDavitArm { get; set; }

        public TubularXArm TubularXArm { get; set; }

        public TubularArm TubularArm => (TubularDavitArm == null ? TubularXArm : TubularDavitArm);

        //public DateTime SavedDate { get; set; }

        //public string SavedBy { get; set; }

        /// <summary>
        /// Apply arm offsets.
        /// Where:
        ///     True = loads are applied to the pole face
        ///     False = compute offset from the center of the pole.  Offset = pole radius
        /// </summary>
        public bool UseArmPoleOffsets { get; set; }

        //public double MaximumArmStressRatio
        //{
        //    get { return _maximumStressRatio > 0 ? _maximumStressRatio : 1; }
        //    set { _maximumStressRatio = value; }
        //}

        #endregion Properties

        #region Methods

        /// <summary>
        /// Analyze non-standard saddle brackets.
        /// </summary>
        /// <returns>Returns true if the saddle bracket is adequate for the loads.  Otherwise, returns false.</returns>
        public bool RunAnalysis()
        {
            if (ThruPlateConnection == null)
            {
                return false;
            }

            if (SaddleBracket == null)
            {
                return false;
            }

            bool designWorked = BracketThruPlateDesign.BracketAnalysis(SaddleBracket, TubularArm); // arm);

            TubularArm.DesignMethodUsed = designWorked ? BracketDesignMethod.OVRD : BracketDesignMethod.FAIL;

            return designWorked;
        }

        public SaddleBracket RunDesign()
        {
            if (ThruPlateConnection == null)
            {
                ThruPlateConnection = new ThruPlateConnection();
            }

            if (SaddleBracket == null)
            {
                SaddleBracket = new SaddleBracket(ThruPlateConnection, Guid.NewGuid());
            }

            if (TubularArm.Parent == null)
            {
                TubularArm.Parent = this;
            }

            ThruPlateConnection.FindWorkingStdBracket(this);

            if (ThruPlateConnection.Attachments != null && ThruPlateConnection.Attachments.Count > 0)
            {
                return (SaddleBracket)ThruPlateConnection.Attachments[0]; // SaddleBracket;
            }
            else
            {
                return null;
            }
        }

        #endregion Methods
    }
}