using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using ArmBracketDesignLibrary.StructureComponents;

// 

namespace ArmBracketDesignLibrary.Helpers
{
    public static class MyExtensions
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        //public MyExtensions()
        //{
        //}

        #endregion  // Constructors

        #region Properties


        #endregion  // Properties

        #region Methods

        /// <summary>
        /// Check a double to see if it is equal to zero.
        /// The tolerance is set a 0.001% of the value.
        /// </summary>
        /// <remarks>
        /// See URL: https://docs.microsoft.com/en-us/dotnet/api/system.double.equals?view=netframework-4.8
        ///
        /// </remarks>
        /// <param name="dVal"></param>
        /// <returns></returns>
        public static bool IsZero(this double dVal)
        {
            double equalTo = 0.0;
            double diff = Math.Abs(dVal - equalTo);
            double pCent = 0.001;  // percent of the value
            double tol = dVal * pCent / 100.0;

            return (diff <= tol);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dVal"></param>
        /// <returns></returns>
        public static bool AreEqual(this double dVal, double eqVal)
        {
            int status = dVal.CompareTo(eqVal);

            return (status == 0);
        }

        /// <summary>
        /// Round a double value to the nearest <see cref="roundingValue"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="roundingValue">Round value to this roundingValue</param>
        /// <returns></returns>
        public static double Round(this double value, double roundingValue)
        {
            double num = value % roundingValue;
            if (num.IsZero())
            {
                return value;
            }

            if (num > roundingValue / 2.0)
            {
                return value + (roundingValue - num);
            }

            return value - num;
        }

        /// <summary>
        /// Round a double up to the nearest value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toNearest"></param>
        /// <returns></returns>
        public static double RoundUp(this double value, double toNearest)
        {
            double num = value % toNearest;
            if (num.IsZero())
            {
                return value;
            }

            return value + (toNearest - num);
        }


        #region Unit Conversions

        /// <summary>
        /// Convert a double from inches to feet.
        /// </summary>
        /// <param name="inches"></param>
        /// <returns></returns>
        public static double InchesToFeet(this double inches)
        {
            return inches / 12.0;
        }

        /// <summary>
        /// Convert a double from feet to inches.
        /// </summary>
        /// <param name="feet"></param>
        /// <returns></returns>
        public static double FeetToInches(this double feet)
        {
            return feet * 12.0;
        }


        /// <summary>
        /// Convert a double from cubic inches to cubic feet
        /// </summary>
        /// <param name="cubicInches">Value being converted</param>
        /// <returns></returns>
        public static double CubicInchesToCubicFeet(this double cubicInches)
        {
            return cubicInches / 1728.0;
        }

        /// <summary>
        /// Convert a double from degrees to radians.
        /// </summary>
        /// <param name="degreeMeasure"></param>
        /// <returns></returns>
        public static double DegreeToRadian(this double degreeMeasure)
        {
            return degreeMeasure * (Math.PI / 180.0);
        }


        #endregion  // Unit Conversions

        /// <summary>
        /// Compute the total thru plate weight in lbs.
        /// Considering two thru plates at each location.
        /// </summary>
        /// <param name="thick">Thru plate thickness in inches.</param>
        /// <param name="width">Thru plate width in inches.</param>
        /// <param name="ht">Thru plate height in inches.</param>
        /// <returns></returns>
        public static decimal ThruPlateBlackWt(double thick, double width, double ht)
        {
            double blackWeight = 2 * (thick * ht * width * Globals.SteelDensityPerCuFt / 1728d);

            return (decimal) blackWeight;
        }

        #endregion  // Methods
    }
}
