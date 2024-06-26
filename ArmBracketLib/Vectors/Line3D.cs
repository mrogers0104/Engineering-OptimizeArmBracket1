using System;
using System.Text;

namespace ArmBracketDesignLibrary.Vectors
{
    [Serializable]
    public class Line3D
    {
        #region Constructor
        public Line3D() { }

        public Line3D(Vector3D pStrt, Vector3D pEnd)
        {
            Vector3D strtP = pStrt.Copy();
            Vector3D endP = pEnd.Copy();

            PStrt = strtP;
            PEnd = endP;
        }
        #endregion

        #region Properties

        /// <summary>
        /// The delta vector, ie. PEnd - PStrt
        /// </summary>
        public Vector3D Delta
        {
            get
            {
                return PEnd - PStrt;
            }
        }

        /// <summary>
        /// The unit vector or direction vector for this line.
        /// </summary>
        public Vector3D Direction
        {
            get
            {
                Vector3D norm = Delta;
                norm.Normalize();
                return norm;
            }
        }

        public double Length
        {
            get
            {
                return Vector3D.Distance(PStrt, PEnd);
            }
        }

        /// <summary>
        /// The mid-point between the start and end of this line.
        /// </summary>
        public Vector3D MidPoint
        {
            get
            {
                Vector3D midPt = (PStrt + PEnd) / 2;

                return midPt;
            }
        }

        public Line3D NormalFromEnd
        {
            get
            {
                return GetNormal(PEnd);
            }
        }

        /// <summary>
        /// The normal(perpendicular) line from the radial line end coorodinate.
        /// The radial is defined from the origin (0,0,0) to the end point of this line.
        /// </summary>
        public Line3D NormalFromRadialEnd
        {
            get
            {
                return GetRadialNormal(PEnd);
            }
        }

        /// <summary>
        /// The normal(perpendicular) line from the radial line start coorodinate.
        /// The radial is defined from the origin (0,0,0) to the start point of this line.
        /// </summary>
        public Line3D NormalFromRadialStrt
        {
            get
            {
                return GetRadialNormal(PStrt);
            }
        }

        public Line3D NormalFromStrt
        {
            get
            {
                return GetNormal(PStrt);
            }
        }

        public Vector3D PEnd { get; set; }
        public Vector3D PStrt { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Are the two lines in the same direction? Check signs of direction cosines.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns>Return true if they are in the same direction, otherwise, false.</returns>
        public static bool LinesInSameDirection(Line3D line1, Line3D line2)
        {
            int signLn1X = Math.Sign(line1.Direction.x);
            int signLn1Y = Math.Sign(line1.Direction.y);
            int signLn2X = Math.Sign(line2.Direction.x);
            int signLn2Y = Math.Sign(line2.Direction.y);

            return (signLn1X == signLn2X && signLn1Y == signLn2Y);
        }

        public Line3D Clone()
        {
            Line3D l = new Line3D();
            l.PEnd = PEnd.Copy();
            l.PStrt = PStrt.Copy();
            return l;
        }

        /// <summary>
        /// Find the closest point on this line from a point.
        /// </summary>
        /// <param name="point">The point to project on this line.</param>
        /// <returns>
        /// Returns the shortest(closest) distance from the point to this line.  
        /// Positive distance ??
        /// Negative distance ??
        /// </returns>
        /// <remarks>
        /// See "3D Math Primer for Graphics and Game Development", Chapter 13.2, page 278
        /// </remarks>
        public decimal ClosestPoint(Vector3D point)
        {
            Vector3D qDelta = point - PStrt;

            double a = Direction * qDelta;

            double t = a / Length;

            Vector3D qPrime = PStrt + Direction * a;

            Line3D perp = new Line3D(point, qPrime);
            //Vector3D dir = perp.Direction;
            double d = perp.Length;

            // What side of the line is this point.
            // < 0, point is "to the left"
            // == 0, point is on the lines
            // > 0, point is "to the right"
            double oSide = Delta.x * (point.y - PStrt.y) - Delta.y * (point.x - PStrt.x);

            return (decimal)d * Math.Sign(oSide);
        }

        public Line3D Copy()
        {
            Vector3D ps = PStrt.Copy();
            Vector3D pe = PEnd.Copy();
            return new Line3D(ps, pe);
        }

        /// <summary>
        /// Get the line segment of length newLength in the direction of this line 
        /// using the mid-point or the start point of this line.
        /// </summary>
        /// <param name="newLength">The length of the new line.</param>
        /// <param name="from">"Mid": length using middle point as reference, "Strt": using starting point to start the line segment.</param>
        /// <returns>Returns line segment (0.0) if "from" is not set to either "Mid" or "Strt", otherwise returns Line3D.</returns>
        public Line3D GetLineSegment(decimal newLength, string from)
        {
            string lFrom = from.ToLower();
            double m = (lFrom.Equals("mid") ? 2.0 : 1.0);

            double nLength = (double)newLength;
            double scale = nLength / (m * Length);

            double deltaX = PEnd.x - PStrt.x;
            double deltaY = PEnd.y - PStrt.y;
            double dX = deltaX * scale;
            double dY = deltaY * scale;

            Vector3D pStrt = null;
            Vector3D pEnd = null;

            switch (lFrom)
            {
                case "mid":
                    pStrt = new Vector3D(MidPoint.x - dX, MidPoint.y - dY, 0.0);
                    pEnd = new Vector3D(MidPoint.x + dX, MidPoint.y + dY, 0.0);
                    break;
                case "strt":
                    pStrt = PStrt;
                    pEnd = new Vector3D(PStrt.x + dX, PStrt.y + dY, 0.0);
                    break;
                default:
                    pStrt = new Vector3D(0.0, 0.0, 0.0);
                    pEnd = new Vector3D(0.0, 0.0, 0.0);
                    break;
            }

            return (PStrt == null ? null : new Line3D(pStrt, pEnd));
        }

        /// <summary>
        /// Find the mid-point between two points on a radius.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        /// <returns>Returns the mid-point on a circle.</returns>
        /// <remarks>
        /// Atan2() of the mean of x1,x2 and the mean of y1,y2 gives you the angle to the mid point. 
        /// Note the factor of 1/2 (for the mean) has been removed from both arguments to Atan2 since that does not change the angle.
        /// </remarks>
        public Vector3D MidPointAtRadius(double radius)
        {
            double theta = Math.Atan2(PStrt.y + PEnd.y, PStrt.x + PEnd.x);
            double x = Math.Cos(theta) * radius;
            double y = Math.Sin(theta) * radius;
            Vector3D midPt = new Vector3D(x, y, 0);

            return midPt;
        }

        //    return midPt;
        //}
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine(string.Format("Strt:({0:f3}, {1:f3}, {2:f3})", PStrt.x, PStrt.y, PStrt.z));
            sb.Append(string.Format("Strt:({0:f3}, {1:f3}, {2:f3})", PStrt.x, PStrt.y, PStrt.z));
            sb.AppendLine(string.Format(" End:({0:f3}, {1:f3}, {2:f3})", PEnd.x, PEnd.y, PEnd.z));

            return sb.ToString();
        }

        /// <summary>
        /// Get the normal to this line through fromPt.
        /// </summary>
        /// <param name="fromPt">The point the normal will go through.</param>
        /// <returns></returns>
        private Line3D GetNormal(Vector3D fromPt)
        {
            Vector3D d = Delta;
            Vector3D p = new Vector3D(-d.y, d.x, d.z);

            Vector3D endPT = fromPt + p;

            return new Line3D(fromPt, endPT);
        }

        /// <summary>
        /// Get the normal to a radial line from the point.  A radial line starts at the origin (0,0,0).
        /// This normal is in the X-Y plane only.
        /// </summary>
        /// <param name="fromPt">Point to start the normal line.</param>
        /// <returns></returns>
        private Line3D GetRadialNormal(Vector3D fromPt)
        {
            Line3D radial = new Line3D(new Vector3D(0.0, 0.0, 0.0), fromPt);
            Vector3D d = radial.Delta;
            Vector3D p = new Vector3D(-d.y, d.x, d.z);

            Vector3D endPT = fromPt + p;

            return new Line3D(fromPt, endPT);
        }
        ///// <summary>
        ///// Fin the mid-point between the start and end of this line.
        ///// </summary>
        ///// <returns></returns>
        //public Vector3D MidPoint()
        //{
        //    Vector3D midPt = (PStrt + PEnd)/2;
        #endregion  // Methods

    }
}
