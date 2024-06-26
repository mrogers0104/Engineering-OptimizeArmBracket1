using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 

namespace ArmBracketDesignLibrary.Vectors
{
    /// <summary>
    /// Define 2D vector using LUtility.Math class
    /// </summary>
    public class Vector2
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public Vector2()
        {
            this.X = 0.0;
            this.Y = 0.0;
        }

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion  // Constructors

       

        public double X { get; set; }

        public double Y { get; set; }

       

     
    }
}
