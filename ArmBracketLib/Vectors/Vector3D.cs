using System;
using ArmBracketDesignLibrary.Helpers;

namespace ArmBracketDesignLibrary.Vectors
{
    /// <summary>
    /// A 3D vector class
    /// See "3D Math Primer for Graphics and Game Development", Chapter 6.2, page 70
    /// </summary>
    [Serializable]
    public class Vector3D
    {

        #region Fields

        public double x;
        public double y;
        public double z;

        #endregion  // Fields


        #region Constructors

        /// <summary>
        /// Default constructor leaves vector in indetriminate state
        /// </summary>
        public Vector3D()
        {

        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public Vector3D(Vector3D a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }

        /// <summary>
        /// Constructor given three values
        /// </summary>
        public Vector3D(decimal nx, decimal ny, decimal nz)
        {
            x = (double) nx;
            y = (double) ny;
            z = (double) nz;
        }

        /// <summary>
        /// Constructor given three values
        /// </summary>
        public Vector3D(double nx, double ny, double nz)
        {
            x = nx;
            y = ny;
            z = nz;
        }

        #endregion  // Constructors

        #region Standard object maintenance


        #endregion

        #region Vector Operations

        /// <summary>
        /// Unary minus returns the negative of the vector
        /// </summary>
        /// <returns></returns>
        public static Vector3D operator -(Vector3D v)
        {
            return new Vector3D(-v.x, -v.y, -v.z);
            //return new Vector3D(-x, -y, -z);
        }

        /// <summary>
        /// Binary subtraction of vectors (-)
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static Vector3D operator -(Vector3D vectorA, Vector3D vectorB)
        {
            return new Vector3D(vectorA.x - vectorB.x, vectorA.y - vectorB.y, vectorA.z - vectorB.z);
        }

        /// <summary>
        /// Check for Inequality
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns></returns>
        public static bool operator !=(Vector3D a, Vector3D b)
        {
            //// If one or both are null, return false
            //if ((object)a == null || (object)b == null)
            //{
            //    return false;
            //}

            //return (a.x != b.x || a.y != b.y || a.z != b.z);
            return !(a == b);
        }

        /// <summary>
        /// Multiplication by a scaler
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3D operator *(Vector3D vectorA, double B)
        {
            return new Vector3D(vectorA.x * B, vectorA.y * B, vectorA.z * B);
        }

        /// <summary>
        /// Multiplication of two vector, also known the Dot Product
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static double operator *(Vector3D vectorA, Vector3D vectorB)
        {
            double val = vectorA.x * vectorB.x + vectorA.y * vectorB.y + vectorA.z * vectorB.z;
            return val;
        }

        /// <summary>
        /// Division by a scaler
        /// If the scalar = 0, then divide by 1.
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3D operator /(Vector3D vectorA, double B)
        {
            double BoverOne = 1;
            if (B != 0)
            {
                BoverOne = 1 / B;
            }
            return new Vector3D(vectorA.x * BoverOne, vectorA.y * BoverOne, vectorA.z * BoverOne);
        }

        /// <summary>
        /// Binary addition of vectors (+)
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static Vector3D operator +(Vector3D vectorA, Vector3D vectorB)
        {
            return new Vector3D(vectorA.x + vectorB.x, vectorA.y + vectorB.y, vectorA.z + vectorB.z);
        }

        /// <summary>
        /// Check Equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3D a, Vector3D b)
        {
            // If both are null or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null but not both, return false
            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        /// <summary>
        /// Check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // if parameter is null return false
            if (obj == null)
            {
                return false;
            }

            // If parameter cann ot be cast to Vector3D return false.
            Vector3D v = obj as Vector3D;
            if (v == null)
            {
                return false;
            }

            // return true if the fields match
            return (x == v.x) && (y == v.y) && (z == v.z);
            //return base.Equals(obj);
        }

        public bool Equals(Vector3D v)
        {
            // IF parameter is null return false.
            if (v == null)
            {
                return false;
            }

            // return true if the fields match
            return (x == v.x) && (y == v.y) && (z == v.z);
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(x * y * z).GetHashCode();
            //return base.GetHashCode();
        }

        /// <summary>
        /// Set vector to zero
        /// </summary>
        public void zero()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        #endregion


        public Vector3D Copy()
        {
            return new Vector3D(x, y, z);
        }

        /// <summary>
        /// Normalize the vector, also known as a unit vector or direction vector
        /// </summary>
        public void Normalize()
        {
            double magSq = this.x*this.x + this.y*this.y + this.z*this.z;

            if (magSq > 0)
            {
                double mag = Math.Sqrt(magSq);
                this.x /= mag;
                this.y /= mag;
                this.z /= mag;
            }
        }

        /// <summary>
        /// The angle of the resultant in radians
        /// </summary>
        /// <returns>Returns the angle arcTan(y/x) in radians</returns>
        public decimal ResultAngle()
        {
            decimal angle = ((decimal)x == 0.0m ? 0.0m : (decimal) Math.Atan(y / x));

            return angle;
            //return (decimal)Math.Atan(y / x);
        }

        /// <summary>
        /// The angle of the resultant in degrees
        /// </summary>
        /// <returns></returns>
        public decimal ResultAngleDeg()
        {
            decimal rad2deg = 180 / (decimal)Math.PI;
            decimal resAngle = ResultAngle();
            return resAngle * rad2deg;
        }

        /// <summary>
        /// The resultant value for this vector.
        /// </summary>
        /// <returns></returns>
        public decimal Resultant()
        {
            double Prod = x*x + y*y + z*z;
            return (decimal) Math.Sqrt(Prod);
        }
        #region nonMember

        /// <summary>
        /// Compute the cross product of two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3D CrossProduct(Vector3D a, Vector3D b)
        {
            double x = a.y * b.z - a.z * b.y;
            double y = a.z * b.x - a.x * b.z;
            double z = a.x * b.y - a.y * b.x;
            return new Vector3D(x, y, z);
        }

        /// <summary>
        /// The length(distance) of the vector.
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns></returns>
        public static double Distance(Vector3D a, Vector3D b)
        {
            double dx = a.x - b.x;
            double dy = a.y - b.y;
            double dz = a.z - b.z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Find the direction vector mid-way between v1 and v2 by averaging the values.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3D MidVector(Vector3D v1, Vector3D v2)
        {

            double x = (v1.x + v2.x) / 2;
            double y = (v1.y + v2.y) / 2;
            double z = (v1.z + v2.z) / 2;

            return new Vector3D(x, y, z);

        }

        /// <summary>
        /// Compute x^2 + y^2 + z^2 and return scalar value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Norm2(Vector3D a)
        {
            return a.x * a.x + a.y * a.y + a.z * a.z;
        }

        /// <summary>
        /// Normalize the vector, also known as a unit vector or direction vector
        /// </summary>
        /// <param name="v1">The vector to normalize</param>
        /// <returns>Return a new normalized vector</returns>
        public static Vector3D Normalize(Vector3D v1)
        {
            Vector3D newV = new Vector3D(v1.x, v1.y, v1.z);
            double magSq = newV.x * newV.x + newV.y * newV.y + newV.z * newV.z;

            if (magSq > 0)
            {
                double mag = Math.Sqrt(magSq);
                newV.x /= mag;
                newV.y /= mag;
                newV.z /= mag;
            }

            return newV;
        }

        /// <summary>
        /// The angle between two unit vectors. It is very important that these are unit vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double VectorAngle(Vector3D a, Vector3D b)
        {
            double ang = Math.Acos(a * b);
            return ang;
        }

        /// <summary>
        /// Compute the magnitude of a vector
        /// </summary>
        /// <param name="a"></param>
        /// <returns>Return the magnitude of the vector as a double</returns>
        public static double VectorMag(Vector3D a)
        {
            double Prod = (double)(a.x * a.x + a.y * a.y + a.z * a.z);
            return Math.Sqrt(Prod);
        }

        /// <summary>
        /// Compute the magnitude of a vector
        /// </summary>
        /// <param name="a"></param>
        /// <returns>Return the magnitude of the vector as a decimal</returns>
        public static decimal VectorMagDecimal(Vector3D a)
        {
            double Prod = (a.x * a.x + a.y * a.y + a.z * a.z);
            return (decimal) Math.Sqrt(Prod);
        }
        /// <summary>
        /// Scalar on the left multiplication, for symmetry
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        //public static Vector3D operator *(Vector3D v, double k )
        //{
        //    return new Vector3D(k * v.x, k * v.y, k * v.z);
        //}
        #endregion

        #region Methods

        /// <summary>
        /// Compute the best-fit plane normal for a set of points
        /// </summary>
        /// <param name="v"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Vector3D ComputeBestFitNormal(Vector3D[] v, int n) 
        {
            // Zero out sum
            Vector3D results = new Vector3D();

            // Start with the "previous" vertex as the last one.
            // This avoids an if-statement in the loop.
            Vector3D p = v[n-1];

            // Iterate through the verticies
            for (int i = 0; i < n; i++)
            {
                //Get shortcut to the "current" vertex
                Vector3D c = v[i];

                // Add in edge vector products appropriately
                results.x += (p.z + c.z) * (p.y - c.y);
                results.y += (p.x + c.x) * (p.z - c.z);
                results.z += (p.y + c.y) * (p.x - c.x);

                // Next vertex

                p = c;
            }

            // Normalize the results and return it
            results.Normalize();
            return results;

        }

        /// <summary>
        /// This method will determine if a point, Q, lies within two rays formed 
        /// radiating from a single point - the origin (0, 0, 0),  through each end of a line.
        /// The point, Q,  lies within the triangle formed by PT1, PT2, and PT3, if and only if 
        /// <para>Ni * (Q - Pi) &gt; -epsilon for i in set (1, 2, 3)</para>
        /// Note:  * is a dot product operation, Ni, Q, and Pi are vectors.
        /// Typically, epsilon = 0.001.
        /// 
        /// <para>
        /// See section 9.6 Triangulation: Mathematics for 3D Game Programming and Computer Graphics. (pg 296-298)
        /// </para>
        /// The triangle is formed by determining the intersection of the rays with a circle.  
        /// The radius of the circle is equal to 1.25 * d, where d = the distance from 
        /// the point Q, to the origin.
        /// </summary>
        /// <param name="pt">Does this point lie within two rays formed </param>
        /// <param name="line">The line used to form two rays from the origin through each end of this line.</param>
        /// <returns></returns>
        public static bool IsPointBetweenRays(Vector3D pt, Line3D line)
        {
            double eTol = -0.001;

            Vector3D origin = new Vector3D();
            decimal rad = pt.Resultant(); // the distance from the point to the origin
            rad *= 1.25m;  // increase the point distance by 25%

            Vector3D N0 = new Vector3D(0.0, 0.0, 1.0); // direction cosines (normal) for the plate containing the three points.

            Line3D line1 = new Line3D(origin, line.PStrt);
            var ray1 = Vector3D.RayIntersectCircle(line1, rad, origin);
            Line3D line2 = new Line3D(origin, line.PEnd);
            var ray2 = Vector3D.RayIntersectCircle(line2, rad, origin);

            Vector3D pt1 = origin;
            Vector3D pt2 = ray1.PEnd;
            Vector3D pt3 = ray2.PEnd;

            var dP1 = pt2 - pt1;
            var dP2 = pt3 - pt2;
            var dP3 = pt1 - pt3;
            var N1 = Vector3D.CrossProduct(N0, dP1);
            var N2 = Vector3D.CrossProduct(N0, dP2);
            var N3 = Vector3D.CrossProduct(N0, dP3);
            N1.Normalize();
            N2.Normalize();
            N3.Normalize();

            var e1 = N1 * (pt - pt1);
            var e2 = N2 * (pt - pt2);
            var e3 = N3 * (pt - pt3);

            return (e1 > eTol && e2 > eTol && e3 > eTol);
        }

        /// <summary>
        /// Find the intersection of a ray and a circle.
        /// </summary>
        /// <param name="ray">The ray as a Line3D</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="originCircle">The origin of the circle as a Vector3D</param>
        /// <returns>
        /// The line of intersection as type: Line3D
        /// <p>
        /// Returns null if the ray does not intersect the circle.
        /// </p>
        /// </returns>
        /// <remarks>
        /// See "3D Math Primer for Graphics and Game Development", Chapter 13.12, page 286
        /// </remarks>
        public static Line3D RayIntersectCircle(Line3D ray, decimal radius, Vector3D originCircle)
        {
            Vector3D d = ray.Direction;
            //Vector3D e = originCircle - ray.PStrt ;

            //double a = e * d;
            //double eSq = e * e; // dot product gives length of vector
            //double f = Math.Sqrt((double) (radius * radius) - (eSq - a * a));    // distance from center of chord to circle

            //// The line is either tangent to the circle (1 point) or does not intersect at all.
            //if (f <= 0)
            //{
            //    return null;                
            //}

            //double t = a - f;



            double tStrt = DistFromPointToCircle(ray.PStrt, d, radius, originCircle);
            double tEnd = DistFromPointToCircle(ray.PEnd, -d, radius, originCircle);

            Vector3D iPt1 = ray.PStrt + d * tStrt;
            Vector3D iPt2 = ray.PEnd + -d * tEnd;
            //Vector3D iPt1 = ray.PStrt + d * t;
            //Vector3D iPt2 = ray.PEnd + d * -t;

            Line3D thisLn = new Line3D(iPt1, iPt2);

            return thisLn;
        }

        /// <summary>
        /// Find the intersection of a ray and a line segment.  
        /// <p>
        /// EG. For a polygon, a ray will intersect at two points. One of these points
        /// may not be in the direction of the ray.  If only the intersection along the direction
        /// vector is needed, set the optional parameter <see cref="inRayDirection"/> to true. 
        /// Which means, only return one intersection point. 
        /// </p>
        /// </summary> 
        /// <param name="ray">The ray as a Line3D</param>
        /// <param name="seg">The line segment as a line3D</param>
        /// <param name="inRayDirection">(optional) Only return point in direction of ray.</param>
        /// <returns>
        /// The point of intersection as type: Vector3D
        /// <p>
        /// Returns null if the ray does not intersect the line segement within the end points.
        /// </p>
        /// </returns>
        /// <remarks>
        /// See "3D Math Primer for Graphics and Game Development", Chapter 13.8, page 283
        /// </remarks>
        public static Vector3D RayIntersectSegment(Line3D ray, Line3D seg, bool inRayDirection = false)
        {
            Vector3D iPt1 = null;   // point of intersection
            Vector3D iPt2 = null;

            Vector3D p1 = ray.PStrt;
            Vector3D p2 = seg.PStrt;
            Vector3D d1 = ray.Delta;
            Vector3D d2 = seg.Delta;

            double num = (CrossProduct((p2 - p1), d2)) * CrossProduct(d1, d2);
            double den = Norm2(CrossProduct(d1, d2));

            if (den.Equals(0))   // ray and seg are parallel (or coincident)
            {
                return null;
            }

            // Compute the scale factor
            double t1 = num / den;

            // If scale factor does not lie between 0 and 1, the intersection lies
            // outside the line segment.
            if (t1 >= 0 && t1 <= 1.0)   // there is an intersection
            {
                iPt1 = p1 + d1 * t1;      // point on ray
            }

            //// 
            //if (inRayDirection)
            //{
            //    return iPt1;
            //}

            num = (CrossProduct((p2 - p1), d1)) * CrossProduct(d1, d2);
            double t2 = num / den;

            if (t2 >= 0 && t2 <= 1.0)   // there is an intersection
            {
                iPt2 = p2 + d2 * t2;     // point on seg         
            }

            // Check for skew (points closest to each other)
            double skew = 0;   // should be zero, if not, the lines are skewed.
            if (iPt1 != null && iPt2 != null)
            {
                skew = Distance(iPt1, iPt2);   // should be zero, if not, the lines are skewed.                
            }

            return iPt2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RayStrt">Starting point of ray.</param>
        /// <param name="RayEnd">Ending point of ray.</param>
        /// <param name="Pn0">Point 1 on plane</param>
        /// <param name="Pn1">Point 2 on plane</param>
        /// <param name="Pn2">Point 3 on plane</param>
        /// <param name="I">Intersection point</param>
        /// <returns>
        ///  Results: -1 = triangle is degenerate (a segment or point)
        ///            0 =  disjoint (no intersect)
        ///            1 =  intersect in unique point I1
        ///            2 =  are in the same plane
        /// </returns>
        /// <remarks>
        /// See URL: http://geomalgorithms.com/a06-_intersect-2.html
        /// </remarks>
        public static int RayIntersectsPlane(Vector3D RayStrt, Vector3D RayEnd, 
                                              Vector3D Pn0, Vector3D Pn1, Vector3D Pn2, 
                                              out Vector3D I)
        {
            Vector3D u, v;      // vectors on the plane
            Vector3D n;         // the plane normal
            double d;           // plane distance
            Vector3D dir;       // ray direction vector
            double r, a, b;     // params to calc ray-plane intersect
            Vector3D zeroVect = new Vector3D();   // zero vector

            //int results;                    // contains error results (see above)

            I = zeroVect;

            // get plane vectors and normal
            u = Pn1 - Pn0;
            v = Pn2 - Pn0;
            n = Vector3D.CrossProduct(u, v); // cross product
            n.Normalize();
            if (n == zeroVect)
            { return -1; }                 // Degenerate: do not deal with this case

            d = (double)(Pn1 * n);         // Plane distance

            dir = RayStrt - RayEnd;     // Direction vector for Ray

            a = (double)(RayStrt * n);
            b = (double)(dir * n);
            r = a / b;
           // if (r < 0.0)                    // ray goes away from plane
           // { return 0; }                   // => no intersect ???

            double t = (d - a) / b; // distance from Ray to Plane
            I = RayStrt + (dir * t);            // this works!!!

            return 1;  // all is good.
        }
        /// <summary>
        /// Rotate the vector about the X, Y, and/or Z axis.
        /// </summary>
        /// <param name="v">Vector or direction vector to rotate</param>
        /// <param name="thetaXD">Rotate about the X-Axis in degrees. Positive rotation is counter-clockwise.</param>
        /// <param name="thetaYD">Rotate about the Y-Axis in degrees. Positive rotation is counter-clockwise.</param>
        /// <param name="thetaZD">Rotate about the Z-Axis in degrees. Positive rotation is counter-clockwise.</param>
        /// <returns></returns>
        /// <remarks>
        /// The rotation uses the left-hand rule where counter-clockwise is negative and clockwise is positive .
        /// </remarks>
        public static Vector3D RotateVector(Vector3D v, decimal thetaXD, decimal thetaYD, decimal thetaZD)
        {
            Vector3D newV = v.Copy();

            double deg2rad = Math.PI / 180;
            double thetaX = (double)thetaXD * deg2rad;
            double thetaY = (double)thetaYD * deg2rad;
            double thetaZ = (double)thetaZD * deg2rad;

            decimal cosThetaX = (decimal)Math.Cos(thetaX);
            decimal sinThetaX = (decimal)Math.Sin(thetaX);
            decimal[,] aboutX = new decimal[,]
            {
                {1, 0, 0},
                {0, cosThetaX, -sinThetaX},
                {0, sinThetaX, cosThetaX}
            };
            if (thetaXD != 0)
            {
                newV = RotateAboutAxis(aboutX, newV);
            }

            decimal cosThetaY = (decimal)Math.Cos(thetaY);
            decimal sinThetaY = (decimal)Math.Sin(thetaY);
            decimal[,] aboutY = new decimal[,]
            {
                {cosThetaY, 0, sinThetaY},
                {0, 1, 0},
                {-sinThetaY, 0, cosThetaY}
            };
            if (thetaYD != 0)
            {
                newV = RotateAboutAxis(aboutY, newV);
            }

            decimal cosThetaZ = (decimal)Math.Cos(thetaZ);
            decimal sinThetaZ = (decimal)Math.Sin(thetaZ);
            decimal[,] aboutZ = new decimal[,]
            {
                {cosThetaZ, -sinThetaZ, 0},
                {sinThetaZ, cosThetaZ, 0},
                {0, 0, 1}
            };
            if (thetaZD != 0)
            {
                newV = RotateAboutAxis(aboutZ, newV);
            }

            return newV;

        }

        /// <summary>
        /// Find the intersection of a line segment with a line segment.  
        /// </summary>
        /// <param name="seg1">Line segment 1 as a Line3D</param>
        /// <param name="seg2">Line segment 2 as a line3D</param>
        /// <returns>
        /// The point of intersection as type: Vector3D
        /// <p>
        /// Returns null if the line segments do not intersect.
        /// </p>
        /// </returns>
        /// <remarks>
        /// See "3D Math Primer for Graphics and Game Development", Chapter 13.8, page 283
        /// </remarks>
        public static Vector3D SegmentIntersectSegment(Line3D seg1, Line3D seg2)
        {
            Vector3D iPt1 = null;   // point of intersection
            Vector3D iPt2 = null;

            Vector3D p1 = seg1.PStrt;
            Vector3D p2 = seg2.PStrt;
            Vector3D d1 = seg1.Delta;
            Vector3D d2 = seg2.Delta;

            double num = (CrossProduct((p2 - p1), d2)) * CrossProduct(d1, d2);
            double den = Norm2(CrossProduct(d1, d2));

            if (den.Equals(0))   // ray and seg are parallel (or coincident)
            {
                return null;
            }

            // Compute the scale factor
            double t1 = num / den;

            // If scale factor does not lie between 0 and 1, the intersection lies
            // outside the line segment.
            if (t1 >= 0 && t1 <= 1.0)   // there is an intersection
            {
                iPt1 = p1 + d1 * t1;      // point on ray
            }

            num = (CrossProduct((p2 - p1), d1)) * CrossProduct(d1, d2);
            double t2 = num / den;

            if (t2 >= 0 && t2 <= 1.0)   // there is an intersection
            {
                iPt2 = p2 + d2 * t2;     // point on seg         
            }

            // Check for skew (points closest to each other)
            double skew = 0;   // should be zero, if not, the lines are skewed.
            if (iPt1 != null && iPt2 != null)
            {
                skew = Distance(iPt1, iPt2);   // should be zero, if not, the lines are skewed.                
            }

            Vector3D thePt = (PointsEqual(iPt1,iPt2) ? iPt1 : null);

            return thePt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //return base.ToString();
            string s = $"[{x:f4} {this.y:f4} {this.z:f4}]";
            return s;
        }

        /// <summary>
        /// Find the distance from thePt in the direction of 'd' to the intersection with circle with radius at originCircle
        /// </summary>
        /// <param name="thePt">The point to find the distance to the circle intersection</param>
        /// <param name="d">The direction vector of the line.</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="originCircle">The origin of the circle.</param>
        /// <returns>
        /// The distance from thePt to the intersection of the circle.
        /// <p>
        /// Returns 0 if the ray does not intersect the circle.
        /// </p>
        /// </returns>
        /// <remarks>
        /// See "3D Math Primer for Graphics and Game Development", Chapter 13.12, page 286
        /// </remarks>
        private static double DistFromPointToCircle(Vector3D thePt, Vector3D d, decimal radius, Vector3D originCircle)
        {
            //Logger.Log.Write("IN DistFromPointToCircle");
            //Logger.Log.Write(string.Format("thePt {0}", thePt));
            //Logger.Log.Write(string.Format("d {0}", d));
            //Logger.Log.Write(string.Format("radius {0}", radius));
            //Logger.Log.Write(string.Format("originCircle {0}", originCircle));

            Vector3D e = originCircle - thePt;

            //Logger.Log.Write(string.Format("e {0}", e));

            double a = e * d;
            double eSq = e * e; // dot product gives length of vector
                                //Logger.Log.Write(string.Format("e {0}", e));

            //Logger.Log.Write("components of f");
            //Logger.Log.Write("Math.Sqrt((double)(radius * radius) - (eSq - a * a))");
            //Logger.Log.Write(string.Format("radius {0}", radius));
            //Logger.Log.Write(string.Format("eSq {0}", eSq));
            //Logger.Log.Write(string.Format("a {0}", a));

            double f = Math.Sqrt((double)(radius * radius) - (eSq - a * a));    // distance from center of chord to circle

            //Logger.Log.Write(string.Format("f {0}", f));

            // The line is either tangent to the circle (1 point) or does not intersect at all.
            if (f <= 0)
            {
                return 0;
            }

            double t = a - f;

            //Logger.Log.Write(string.Format("t {0}", t));

            return t;
        }

        /// <summary>
        /// Check to see if two points are equal. The points of type Vector3D.
        /// </summary>
        /// <param name="pt1">Point #1</param>
        /// <param name="pt2">Point #2</param>
        /// <returns>Returns true if the points are equal, false if they are not.</returns>
        /// <remarks>The points are considered equal if they are within tolerance (epsilon) of plus/minus 0.001</remarks>
        private static bool PointsEqual(Vector3D pt1, Vector3D pt2)
        {
            if (pt1 == null || pt2 == null)
            {
                return (pt1 == pt2);
            }

            //var epsilon = new DoubleComparison.Epsilon(0.001);

            //bool testX = pt1.x.EQ(pt2.x, epsilon);
            //bool testY = pt1.y.EQ(pt2.y, epsilon);
            //bool testZ = pt1.z.EQ(pt2.z, epsilon);
            bool testX = pt1.x.AreEqual(pt2.x);
            bool testY = pt1.y.AreEqual(pt2.y);
            bool testZ = pt1.z.AreEqual(pt2.z);

            return testX && testY && testZ;

        }
        /// <summary>
        /// Multiply the direction vector times the rotation matrix to get the rotated vector.
        /// </summary>
        /// <param name="rotateMatrix">The rotation matrix</param>
        /// <param name="dv">The vector to rotate.</param>
        /// <returns>The rotated vector</returns>
        private static Vector3D RotateAboutAxis(decimal[,] rotateMatrix, Vector3D dv)
        {
            decimal[] v = new decimal[] { (decimal)dv.x, (decimal)dv.y, (decimal)dv.z };
            int rows = rotateMatrix.GetLength(0);
            int cols = rotateMatrix.GetLength(1);
            decimal[] vNew = new decimal[3];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    vNew[r] += rotateMatrix[r, c] * v[c];
                }
            }

            Vector3D newDV = new Vector3D(vNew[0], vNew[1], vNew[2]);
            return newDV;
        }
        ///// <summary>
        ///// See URL: http://www.cprogramming.com/tutorial/3d/rotation.html
        ///// </summary>
        ///// <param name="v"></param>
        ///// <param name="thetaZD"></param>
        ///// <returns></returns>
        //public static Vector3D ArbitraryRotationVector(Vector3D v,  decimal thetaZD)
        //{
        //    Vector3D newV = Utils.CloneObject<Vector3D>(v);
        //    Vector3D newX = Utils.CloneObject<Vector3D>(v);
        //    newX.Normalize();   // unit vector for X-Axis
        //    Vector3D newY = new Vector3D(-newX.y, newX.x, newX.z);  // unit vector for Y-Axis
        //    Vector3D newZ = Vector3D.CrossProduct(newX, newY);      // unit vector for Z-Axis

        //    double ang = Math.Acos(newX*newY);
        //    double angDeg = ang*180/Math.PI;

        //    double deg2rad = Math.PI / 180;
        //    double thetaZ = (double)thetaZD * deg2rad;

        //    decimal cosTheta = (decimal)Math.Cos(thetaZ);
        //    decimal sinTheta = (decimal)Math.Sin(thetaZ);
        //    decimal tTheta = 1 - cosTheta;

        //    decimal X = (decimal) newX.x;
        //    decimal Y = (decimal) newX.y;
        //    decimal Z = (decimal) newX.z;

        //    decimal[,] aboutZ = new decimal[,]
        //    {
        //        {tTheta*X*X + cosTheta, tTheta *X*Y + sinTheta*Z, tTheta*X*Z - sinTheta*Y},
        //        {tTheta*X*Y - sinTheta*Z, tTheta*Y*Y+cosTheta, tTheta*Y*Z+sinTheta*X},
        //        {tTheta*X*Z+sinTheta*Y, tTheta*Y*Z-sinTheta*X,tTheta*Z*Z+cosTheta}
        //    };

        //    newV = RotateAboutAxis(aboutZ, newV);

        //    return newV;


        //}
        #endregion  // Methods


        //#region ICloneable Implementation

        ///// <summary>
        ///// Perform a deep or shallow clone (copy) of this object.
        ///// Deep copying or cloning means that the copied object's fields will reference new copies of the original object's fields. 
        ///// Shallow copying means that the copied object's fields will reference the same objects as the original object. 
        ///// </summary>
        ///// <param name="doDeepCopy">True: do a deep copy, False: do a shallow copy</param>
        ///// <returns></returns>
        ///// <remarks>
        ///// See URL: http://codeidol.com/csharp/csharpckbk2/Classes-and-Structures/Building-Cloneable-Classes/
        ///// </remarks>
        //public object Clone(bool doDeepCopy)
        //{
        //    if (doDeepCopy)
        //        {
        //            BinaryFormatter BF = new BinaryFormatter();
        //            MemoryStream memStream = new MemoryStream();

        //            BF.Serialize(memStream, this);
        //            memStream.Position = 0;

        //            return (BF.Deserialize(memStream));
        //        }
        //        else
        //        {
        //            return (this.MemberwiseClone());
        //        }
        //    }

        ///// <summary>
        ///// Perform a shallow copy of this object
        ///// </summary>
        ///// <returns></returns>
        //public object Clone()
        //{
        //    return (Clone(false));
        //}        

        //#endregion  // ICloneable Implementation
    }   

}