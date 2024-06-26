using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 

namespace ArmBracketDesignLibrary.Helpers
{

    public enum TubeOrientation
    {
        FlatToZero = 0,
        PointToZero = 1
    }

    //public enum BendRadiusType
    //{
    //    None = 0,
    //    _2T = 1,
    //    _3T = 2
    //}

    
    public enum PlateFinish
    {
        None = 0,
        Galvanized = 1,
        Weathering = 2,
    }

    //public enum PlateMaterialSpecification
    //{
    //    None = 0,
    //    A572_50 = 1,
    //    A572_65 = 2,
    //    A871_65 = 3,
    //    A588_50 = 4,
    //    A588_46 = 5,
    //    A572_42 = 6,
    //    A588_60 = 7,
    //    A633_46 = 8,
    //    A633_50 = 9,
    //    A633_60 = 10,
    //    A36 = 11,
    //    A847_50 = 12,
    //    A500_46 = 13
    //}

    /// <summary>
    /// Plate Material Specification matching SUEET enums. 
    /// 6/20/2023  MAR
    /// </summary>
    public enum PlateMaterialSpecification
    {
        None = 0,
        A572_65 = 1,
        A572_50 = 2,
        A572_42 = 3,

        A871_65 = 4,

        A588_60 = 5,
        A588_50 = 6,
        A588_46 = 7,

        A633_46 = 8,
        A633_50 = 9,
        A633_60 = 10,
        A36 = 11,
        A847_50 = 12,
        A500_46 = 13
    }


    public enum BracketArmType
    {
        Unknown = 0,
        Common = 1,
        Hex = 2,
        FWTall = 3,
        FWThex= 4
    }

    public enum BracketBoltPattern
    {
        None = 0,
        SingleRow = 1
    }

    public enum BracketOrientation
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2
    }

    public enum BracketType
    {
        None = 0,
        Bent = 1,
        ThreePiece = 2
    }


    public enum ArmConnectionMethod
    {
        None = 0,
        Fixed = 1,
        Pinned = 2
    }

    public enum ConnectionType
    {
        None = 0,
        ThruPlate = 1,
        SinglePieceBracket = 2
    }

    public enum BracketDesignMethod
    {
        UNKN = 0,
        STND = 1,
        OVRD = 2, 
        NOHX = 3,
        DSGN = 99,
        FAIL = 999
    }

    /// <summary>
    /// Message categories.
    /// </summary>
    public enum MessageCategory
    {
        Info = 0,
        Warn = 1,
        Error = 2
    }


    public class Enums
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public Enums()
        {
        }

        #endregion  // Constructors

        #region Properties


        #endregion  // Properties

        #region Methods


        #endregion  // Methods
    }
}
