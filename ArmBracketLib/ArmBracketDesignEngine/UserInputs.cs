

//

using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;
using ArmBracketDesignLibrary.StructureComponents.Pole;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class UserInputs
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


        // ArmsProject => ArmsProject.AllowA354BCBolts
        /// <summary>
        /// Allow A354BC bolts in the bracket design
        /// </summary>
        public bool AllowA354BCBolts { get; set; }

        // => ArmsProject.AllowArmCoping
        /// <summary>
        /// Allow coping of the arm.
        /// As of 6-03-2017, coping is no longer used per Hock
        /// This property remains in place if and when Hock or Sabre
        /// changes their minds.
        /// </summary>
        public bool AllowArmCoping => false;

        // => ArmsProject.AllowCenterBolts
        /// <summary>
        /// Allow a center hole in the bracket
        /// </summary>
        public bool AllowCenterBolts { get; set; }

        public decimal ArmAzimuth { get; set; }

        public string ArmFinish { get; set; }

        //public string ArmMaterialSpecification { get; set; }

        public bool TwoArmsAtLocation { get; set; }
        /// <summary>
        /// The bid number for this order.
        /// </summary>
        public string BidNumber { get; set; }

        /// <summary>
        /// User provided ID to distinguish this specific arm bracket.
        /// ID will be returned with the results.
        /// </summary>
        public string BracketID { get; set; }

        public string BracketFinish { get; set; }
        public string BracketMaterialSpecification { get; set; }
        public CustomBracketInput CustomBracketInput { get; set; }

        // => ArmsProject.Customer
        /// <summary>
        /// The customer name
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// The engineer name.
        /// </summary>
        public string Engineer { get; set; }

        /// <summary>
        /// The order number.
        /// </summary>
        public string OrderNumber { get; set; }


        public string PolePlateFinish { get; set; }

        // => ArmsProject.ProjectDescription
        /// <summary>
        /// The description of this project
        /// </summary>
        public string ProjectDescription { get; set; }

    }
}
