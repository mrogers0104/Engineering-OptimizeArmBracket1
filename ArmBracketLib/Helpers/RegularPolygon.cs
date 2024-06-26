using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 

namespace ArmBracketDesignLibrary.Helpers
{
    public class RegularPolygon
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public RegularPolygon()
        {
        }

        #endregion  // Constructors

        #region Properties


        #endregion  // Properties

        #region Methods

        public static double GetPointDiameter(int sideCount, double flatDiameter)
        {
          return flatDiameter / Math.Cos(RegularPolygon.GetSideAngleRadians(sideCount));
        }

        public static double GetRoundCornerAdjustmentAmount(int sideCount, double thickness)
        {
            double bendradius = thickness * 4;
          return (bendradius + thickness) * (1.0 / Math.Cos(RegularPolygon.GetSideAngleRadians(sideCount)) - 1.0);
        }

        public static double GetPointDiameterAdjustedForRoundCorners(int sideCount, double flatDiameter, double thickness)
        {
          return RegularPolygon.GetPointDiameter(sideCount, flatDiameter) - RegularPolygon.GetRoundCornerAdjustmentAmount(sideCount, thickness);
        }

        public static double GetSideAngleRadians(int sideCount)
        {
          //return UnitConversion.DegreeToRadian(180.0 / Convert.ToDouble(sideCount));
          return (180.0 / Convert.ToDouble(sideCount)).DegreeToRadian();
        }

        public static double GetArea(int sideCount, double flatDiameter)
        {
          return 0.5 * RegularPolygon.GetSideLength2(sideCount, flatDiameter / 2.0) * flatDiameter;
        }

        public static double GetPerimeter(int sideCount, double flatDiameter)
        {
          return RegularPolygon.GetSideLength2(sideCount, flatDiameter / 2.0) * (double) sideCount;
        }

        public static double GetCircumradius(int sidecount, double apothem)
        {
          if (sidecount <= 0)
            return 0.0;
          return apothem / Math.Cos(Math.PI / (double) sidecount);
        }

        public static double GetApothem(int sidecount, double circumradius)
        {
          if (sidecount <= 0)
            return 0.0;
          return circumradius * Math.Cos(Math.PI / (double) sidecount);
        }

        public static double GetApothem2(int sidecount, double sidelength)
        {
          if (sidecount <= 0)
            return 0.0;
          return sidelength / (2.0 * Math.Tan(Math.PI / (double) sidecount));
        }

        public static double GetSideLength(int sidecount, double circumradius)
        {
          if (sidecount <= 0)
            return 0.0;
          return 2.0 * circumradius * Math.Sin(Math.PI / (double) sidecount);
        }

        public static double GetSideLength2(int sidecount, double apothem)
        {
          if (sidecount <= 0)
            return 0.0;
          return 2.0 * RegularPolygon.GetCircumradius(sidecount, apothem) * Math.Sin(Math.PI / (double) sidecount);
        }

        #endregion  // Methods
    }
}
