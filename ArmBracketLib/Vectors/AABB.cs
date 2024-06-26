namespace ArmBracketDesignLibrary.Vectors
{
    /// <summary>
    /// Define an Axially Aligned Bounding Box (AABB).
    /// Reference: 3D Math Primer for Graphics and Game Development - section 12.4.1
    /// </summary>
    public class AABB
    {
        private const decimal KBigNumber = 1E27m;

        #region Constructors

        public AABB()
        {
            Empty();
        }

        #endregion

        #region Properties

        public Vector3D MyMin { get; set; }

        public Vector3D MyMax { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Empty the bounding box by setting the min/max values to a really large/small number.
        /// </summary>
        public void Empty()
        {
            MyMin = new Vector3D(KBigNumber, KBigNumber, KBigNumber);

            MyMax = new Vector3D(-KBigNumber, -KBigNumber, -KBigNumber);
        }

        /// <summary>
        /// Add a single point into the AABB by expanding the AABB if necessary to contain the point.
        /// </summary>
        /// <param name="pt">The point used to expand the bounding box.</param>
        public void Add(Vector3D pt)
        {
            if (pt.x < MyMin.x) { MyMin.x = pt.x; }
            if (pt.x > MyMax.x) { MyMax.x = pt.x; }

            if (pt.y < MyMin.y) { MyMin.y = pt.y; }
            if (pt.y > MyMax.y) { MyMax.y = pt.y; }

            if (pt.z < MyMin.z) { MyMin.z = pt.z; }
            if (pt.z > MyMax.z) { MyMax.z = pt.z; }

        }

        /// <summary>
        /// Return true if the bounding box contains the point.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool Contains(Vector3D pt)
        {
            return ((pt.x >= MyMin.x) && (pt.x <= MyMax.x) && 
                    (pt.y >= MyMin.y) && (pt.y <= MyMax.y) && 
                    (pt.z >= MyMin.z) && (pt.z <= MyMax.z));
        }

        /// <summary>
        /// Return true if the bounding box plus the tolerance contains the point 'pt'.
        /// </summary>
        /// <param name="pt">Is this point within the bounding box?</param>
        /// <param name="vTolerance">The tolerance vector: plus or minus.</param>
        /// <returns></returns>
        public bool Contains(Vector3D pt, Vector3D vTolerance)
        {
            double vTolX = vTolerance.x;
            double vTolY = vTolerance.y;
            double vTolZ = vTolerance.z;

            return ((pt.x >= MyMin.x - vTolX) && (pt.x <= MyMax.x + vTolX) &&
                    (pt.y >= MyMin.y - vTolY) && (pt.y <= MyMax.y + vTolY) &&
                    (pt.z >= MyMin.z - vTolZ) && (pt.z <= MyMax.z + vTolZ));
        }
        #endregion
    }
}
