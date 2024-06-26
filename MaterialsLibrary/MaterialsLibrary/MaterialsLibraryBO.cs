using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NLog;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace MaterialsLibrary
{
    /// <summary>
    /// Class defining the plate and bolt properties: grade, available thicknesses, bolt sizes, etc.
    /// This is a Singleton.
    /// </summary>
    
    public class MaterialsLibraryBO
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();


        #region Methods 

        /// <summary>
        /// Get all the available plate material
        /// </summary>
        /// <param name="includeInactive">True = include plates that are marked inactive.</param>
        public static List<SteelPlateMaterial> GetPlates(bool includeInactive)
        {
            return SteelPlateMaterial.GetSteelPlates(includeInactive);
        }

        /// <summary>
        /// Get all the available plate material that has the provided material spec and yield
        /// </summary>
        /// <param name="materialSpecAndYield">In format AMMM-YY where AMMM is ASTM grade and YY is yield strength is ksi.  For example A572-50</param>
        /// <param name="includeInactive">True = include plates that are marked inactive.</param>

        public static List<SteelPlateMaterial> GetPlates(string materialSpecAndYield, bool includeInactive)
        {
            logger.Trace("Getting plates for matspec {0}, include inactive = {1}", materialSpecAndYield, includeInactive);

            List<SteelPlateMaterial> lst = new List<SteelPlateMaterial>();

            if (GetYieldKsi(materialSpecAndYield) > 0)
            {
                lst = GetPlates(includeInactive).FindAll(
                  p => p.MaterialSpecAndYield.Equals(materialSpecAndYield
                  , StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                string astm = GetASTMSpecOnly(materialSpecAndYield);

                lst = GetPlates(includeInactive).FindAll(
                   p => p.ASTM.Equals(astm, StringComparison.InvariantCultureIgnoreCase));
            }

            logger.Trace("returning {0} plates", lst.Count);

            return lst.OrderBy(p => p.ASTM).ThenByDescending(p => p.FyPsi).ThenBy(p => p.Thickness).ToList();
        }


        /// <summary>
        /// Get all plate material specs and yields and put them into a list.
        /// </summary>
        /// <param name="includeInactive">Include all inactive plates?</param>
        /// <returns></returns>
        public static List<string> GetPlateGrades(bool includeInactive)
        {
            List<string> grades = new List<string>();

            foreach (SteelPlateMaterial p in GetPlates(includeInactive))
            {
                if (!grades.Contains(p.MaterialSpecAndYield))
                {
                    grades.Add(p.MaterialSpecAndYield);
                }
            }

            return grades;

        }

        /// <summary>
        /// Get a list of plate grades by ASTM only.
        /// </summary>
        /// <param name="materialSpecAndYield">create a list containing the ASTM spec in this value</param>
        /// <param name="includeInactive"></param>
        /// <returns>Return a list of plate material where ASTM is contained in materialSpecAndYield</returns>
        public static List<string> GetPlateGradesByASTM(string materialSpecAndYield, bool includeInactive)
        {
            List<string> grades = new List<string>();
            string astm = GetASTMSpecOnly(materialSpecAndYield);

            foreach (SteelPlateMaterial p in GetPlates(includeInactive))
            {
                if (p.ASTM != astm)
                {
                    continue;   // only want ASTM of materialSpecAndYield
                }

                if (!grades.Contains(p.MaterialSpecAndYield))
                {
                    grades.Add(p.MaterialSpecAndYield);
                }
            }


            return grades;

        }

        /// <summary>
        /// Gets the minimum available plate thickness based on the material spec and yield provided.
        /// </summary>
        /// <param name="materialSpecAndYield">In format AMMM-YY where AMMM is ASTM designation and YY is yield strength is ksi.  For example A572-50</param>
        /// <param name="includeInactive">True = include plates that are marked inactive.</param>
        /// <returns></returns>
        public static decimal GetPlateMinThickness(string materialSpecAndYield, bool includeInactive)
        {
            decimal? th = null;
            decimal answer = 0;
            foreach (SteelPlateMaterial sp in GetPlates(materialSpecAndYield, includeInactive))
            {
                if (!th.HasValue)
                {
                    th = sp.Thickness;
                }
                else
                {
                    th = Math.Min(th.Value, sp.Thickness);
                }
            }

            if (th.HasValue)
            {
                answer = th.Value;
            }
            else
            {
                answer = 0;
            }

            return answer;
        }

        /// <summary>
        /// Gets the maximum available plate thickness based on the material spec and yield provided.
        /// </summary>
        /// <param name="materialSpecAndYield">In format AMMM-YY where AMMM is ASTM designation and YY is yield strength is ksi.  For example A572-50</param>
        /// <param name="includeInactive">True = include plates that are marked inactive.</param>
        /// <returns></returns>
        public static decimal GetPlateMaxThickness(string materialSpecAndYield, bool includeInactive)
        {
            decimal? th = null;
            decimal answer = 0;
            foreach (SteelPlateMaterial sp in GetPlates(materialSpecAndYield, includeInactive))
            {
                if (!th.HasValue)
                {
                    th = sp.Thickness;
                }
                else
                {
                    th = Math.Max(th.Value, sp.Thickness);
                }
            }

            if (th.HasValue)
            {
                answer = th.Value;
            }
            else
            {
                answer = 0;
            }

            return answer;
        }

        /// <summary>
        /// Get anchor bolts from Materials.AnchorBolts table.
        /// </summary>
        public static List<AnchorBolt_ML> GetAnchorBolts(bool includeInactive)
        {
            return AnchorBolt_ML.GetAnchorBolts(includeInactive);
        }

        ///// <summary>
        ///// Get anchor bolts by diameter.
        ///// </summary>
        ///// <param name="boltDia"></param>
        ///// <param name="includeInactive"></param>
        ///// <returns></returns>
        //public static List<AnchorBolt_ML> GetAnchorBolts(decimal boltDia, bool includeInactive)
        //{
        //    List<AnchorBolt_ML> abList = GetAnchorBolts(includeInactive);
        //    return (from ab in abList where ab.Diameter == (double) boltDia orderby ab.Diameter select ab).ToList();
        //}

        ///// <summary>
        ///// Get anchor bolt by ASTM specification
        ///// </summary>
        ///// <param name="astm"></param>
        ///// <param name="includeInactive"></param>
        ///// <returns></returns>
        //public static List<AnchorBolt_ML> GetAnchorBolts(string astm, bool includeInactive)
        //{
        //    List<AnchorBolt_ML> abList = GetAnchorBolts(includeInactive);
        //    return (from ab in abList where ab.MaterialSpecification.StartsWith(astm) orderby ab.Diameter select ab).ToList();
        //}

        /// <summary>
        /// Get unique anchor bolt grades (the QtGrades for bolt designation).
        /// </summary>
        /// <param name="includeInactive">Include inactive bolts?</param>
        /// <returns></returns>
        public static List<string> GetAnchorBoltGrades(bool includeInactive)
        {
            List<string> grades = new List<string>();

            foreach (AnchorBolt_ML ab in GetAnchorBolts(includeInactive))
            {
                if (!grades.Contains(ab.QtGradeName))
                {
                    grades.Add(ab.QtGradeName);
                }
            }

            return grades;
        }

        /// <summary>
        /// Get a list of structural bolt properties from Materials.Bolts table.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static List<StructuralBolt_ML> GetStructuralBolts(bool includeInactive)
        {
            return StructuralBolt_ML.GetStructuralBolts(includeInactive);
        }

        /// <summary>
        /// Get a list of all bolt properties: Anchor and structural.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static List<ICommonBolt> GetAllCommonBolts(bool includeInactive)
        {
            List<ICommonBolt> lst = new List<ICommonBolt>();
            lst.AddRange(GetAnchorBolts(includeInactive).ToList<ICommonBolt>());
            lst.AddRange(GetStructuralBolts(includeInactive).ToList<ICommonBolt>());
            return lst;
        }

        /// <summary>
        /// Get a list of unique grades for structural bolts.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static List<string> GetStructuralBoltGrades(bool includeInactive)
        {
            List<string> grades = new List<string>();

            foreach (StructuralBolt_ML b in GetStructuralBolts(includeInactive))
            {
                if (!grades.Contains(b.QtGradeName))
                {
                    grades.Add(b.QtGradeName);
                }
            }

            return grades;

        }

        /// <summary>
        /// Get the anchor bolt for the diameter, material spec and yield (A615-75) provided.
        /// </summary>
        /// <param name="boltDia">Bolt diameter in inches.</param>
        /// <param name="QtGradeName">The bolt grade (EG. A325-92)</param>
        /// <param name="includeInactive">Get on active and inactive bolts.</param>
        /// <returns>Returns AnchorBolt_ML as a new reference.</returns>
        public static AnchorBolt_ML GetAnchorBolt(decimal boltDia, string QtGradeName, bool includeInactive)
        {
            List<AnchorBolt_ML> allAnchorBolts = GetAnchorBolts(includeInactive);
            AnchorBolt_ML anchorBolt = allAnchorBolts.Find(ab => ab.Diameter.Equals((double)boltDia) && ab.QtGradeName.Equals(QtGradeName));

            if (anchorBolt == null)
            {
                string msg = string.Format("AnchorBolt_ML bolt not found: {0} x {1} in. dia.", QtGradeName, boltDia);
                Exception ex = new Exception(msg);
                logger.Fatal(ex);
                throw ex;
            }

            return CloneObject<AnchorBolt_ML>(anchorBolt);
        }

        /// <summary>
        /// Get the structural bolt for the diameter, material spec and yield (A325-92) provided.
        /// </summary>
        /// <param name="boltDia">Bolt diameter in inches.</param>
        /// <param name="QtGradeName">The bolt grade (EG. A325-92)</param>
        /// <param name="includeInactive">Get on active and inactive bolts.</param>
        /// <returns>Returns StructuralBolt_ML as a new reference.</returns>
        public static StructuralBolt_ML GetStructuralBolt(decimal boltDia, string QtGradeName, bool includeInactive)
        {
            var structBolts = GetStructuralBolts(includeInactive).Find(sb => sb.Diameter.Equals((double)boltDia) && sb.QtGradeName.Equals(QtGradeName));

            if (structBolts == null)
            {
                string msg = string.Format("Structural_ML bolt not found: {0} x {1} in. dia.", QtGradeName, boltDia);
                Exception ex = new Exception(msg);
                logger.Fatal(ex);
                throw ex;
            }
            return CloneObject<StructuralBolt_ML>(structBolts);
        }

        /// <summary>
        /// Get the structural or anchor bolt for the diameter, material spec and yield (A325-92) provided.
        /// </summary>
        /// <param name="boltDia">Bolt diameter in inches. If less than or equal to 0, default </param>
        /// <param name="QtSpecName">QT Designation for Bolt ASTM and yield. EG. A325-92</param>
        /// <param name="forBasePlate">Is this bolt for a base plate?</param>
        /// <param name="includeInactive">Include inactive bolts?</param>
        /// <returns></returns>
        public static ICommonBolt GetBoltProperty(decimal boltDia, string QtSpecName, bool forBasePlate, bool includeInactive)
        {
            // ** get the bolt property
            if (forBasePlate)
            {
                return GetAnchorBolt(boltDia, QtSpecName, includeInactive);
            }
            else
            {
                return GetStructuralBolt(boltDia, QtSpecName, includeInactive);
            }

        }

        /// <summary>
        /// Get the structural or anchor bolt for the diameter, material spec and yield (A325-92) provided.
        /// </summary>
        /// <param name="boltDia">Bolt diameter in inches. If less than or equal to 0, default </param>
        /// <param name="QtSpecName">QT Designation for Bolt ASTM and yield. EG. A325-92</param>
        /// <param name="forBasePlate">Is this bolt for a base plate?</param>
        /// <param name="includeInactive">Include inactive bolts?</param>
        /// <param name="includeDefault">Get default values for boltDia and QtSpecName if boltDia = 0 or QtSpecName is null or empty</param>
        /// <returns></returns>
        public static ICommonBolt GetBoltProperty(decimal boltDia, string QtSpecName, bool forBasePlate, bool includeInactive, bool includeDefault)
        {
            if (includeDefault)
            {
                // ** use a default bolt diameter if none is provided
                if (boltDia <= 0)
                {
                    boltDia = (forBasePlate ? 2.25m : 1.00m);
                }

                // ** If QtSpecName is empty or null, use default QtSpecName given the bolt diameter.
                if (string.IsNullOrEmpty(QtSpecName))
                {
                    QtSpecName = "A615-75";
                    if (!forBasePlate)
                    {
                        if (boltDia <= 1.25m)
                        {
                            QtSpecName = "A325";
                        }
                        else
                        {
                            QtSpecName = "A354-BC";
                        }
                    }
                }
            }

            return GetBoltProperty(boltDia, QtSpecName, forBasePlate, includeInactive);
        }


        /// <summary>
        /// Get a bolt list for the spec provided.
        /// </summary>
        /// <param name="spec">QtGradeName starts with "spec" (EG. A325 or A325-92).  A null or empty spec will return a blank list.</param>
        /// <param name="yield">The bolt yield strength . If yield is = 0, then return all.</param>
        /// <param name="boltDia">The list will contain the bolt diameter.  The diameter may be 0 in which case it will return all diameters for the spec.</param>
        /// <param name="boltDiaLogic">The logic for finding the bolt diameter: LT, LE, GT, GE, EQ</param>
        /// <returns></returns>
        public static IEnumerable<ICommonBolt> GetBoltList(string spec, double yield, double boltDia, string boltDiaLogic)
        {
            IEnumerable<ICommonBolt> myBoltList = new List<ICommonBolt>();

            if (string.IsNullOrEmpty(spec))
            {
                return myBoltList;
            }

            List<ICommonBolt> allBolts = GetAllCommonBolts(false);
            string logic = boltDiaLogic.ToUpper();

            myBoltList = allBolts.Where(b => b.QtGradeName.StartsWith(spec));

            if (yield > 0)
            {
                myBoltList = myBoltList.Where(b => b.FyKsi >= yield);
            }

            switch (logic)
            {
                case "LT":
                    myBoltList = myBoltList.Where(b => b.Diameter < boltDia);
                    break;
                case "LE":
                    myBoltList = myBoltList.Where(b => b.Diameter <= boltDia);
                    break;
                case "GT":
                    myBoltList = myBoltList.Where(b => b.Diameter > boltDia);
                    break;
                case "GE":
                    myBoltList = myBoltList.Where(b => b.Diameter >= boltDia);
                    break;
                case "EQ":
                    myBoltList = myBoltList.Where(b => b.Diameter == boltDia);
                    break;
                    //default:
                    //    return myBoltList;
            }

            return myBoltList.OrderByDescending(b => b.FyKsi).ThenBy(b => b.Diameter);
        }


        /// <summary>
        /// Get common bolt list for the params provided.
        /// </summary>
        /// <param name="spec">QtGradeName starts with "spec" (EG. A325 or A325-92).  A null or empty spec will return all specs.</param>
        /// <param name="yield">The bolt yield strength . If yield is = 0, then return all.</param>
        /// <param name="minDia">The minimum bolt diameter to return</param>
        /// <param name="maxDia">The maximum bolt diameter to return, if zero then no maximum</param>
        /// <returns></returns>
        public static IEnumerable<ICommonBolt> GetCommonBoltList(string spec, decimal yield, double minDia, double maxDia)
        {
            return ApplyParamsToBoltList(GetAllCommonBolts(false), spec, yield, minDia, maxDia);
        }


        /// <summary>
        /// Get structural bolt list for the params provided.
        /// </summary>
        /// <param name="spec">QtGradeName starts with "spec" (EG. A325 or A325-92).  A null or empty spec will return all specs.</param>
        /// <param name="yield">The bolt yield strength . If yield is = 0, then return all.</param>
        /// <param name="minDia">The minimum bolt diameter to return</param>
        /// <param name="maxDia">The maximum bolt diameter to return, if zero then no maximum</param>
        /// <returns></returns>
        public static IList<ICommonBolt> GetStructuralBoltList(string spec, decimal yield, double minDia, double maxDia)
        {
            List<StructuralBolt_ML> sbolts = GetStructuralBolts(false);
            return sbolts.ToList<ICommonBolt>();
        }


        /// <summary>
        /// Get anchor bolt list for the params provided.
        /// </summary>
        /// <param name="spec">QtGradeName starts with "spec" (EG. A325 or A325-92).  A null or empty spec will return all specs.</param>
        /// <param name="yield">The bolt yield strength . If yield is = 0, then return all.</param>
        /// <param name="minDia">The minimum bolt diameter to return</param>
        /// <param name="maxDia">The maximum bolt diameter to return, if zero then no maximum</param>
        /// <returns></returns>
        public static List<ICommonBolt> GetAnchorBoltList(string spec, decimal yield, double minDia, double maxDia)
        {
            List<AnchorBolt_ML> abolts = GetAnchorBolts(false);
            

            return  ApplyParamsToBoltList(abolts.ToList<ICommonBolt>(),spec,yield,minDia,maxDia);
            
        }

        private static List<ICommonBolt> ApplyParamsToBoltList(List<ICommonBolt> boltList, string spec, decimal yield, double minDia, double maxDia)
        {
            if (!string.IsNullOrEmpty(spec) && !string.IsNullOrWhiteSpace(spec))
            {
                boltList = boltList.FindAll(b => b.QtGradeName.ToUpper().StartsWith(spec.ToUpper()));
            }

            if (yield > 0)
            {
                boltList = boltList.FindAll(b => (decimal)b.FyKsi >= yield);
            }

            boltList = boltList.FindAll(b => b.Diameter >= minDia);

            if (maxDia > 0)
            {
                boltList = boltList.FindAll(b => b.Diameter <= maxDia);
            }

            return boltList;
        }

        /// <summary>
        /// Get the ASTM spec for this grade.  EG. A572-50, the ASTMspec = A572.
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static string GetASTMSpecOnly(string grade)
        {
            logger.Trace("grade: {0}", grade);
            string[] w = grade.Split('-');
            string ASTMSpec = string.Empty;
            if (w.Length >= 1)
            {
                ASTMSpec = w[0];
            }

            logger.Trace("ASTMSpec: {0}", ASTMSpec);
            return ASTMSpec;
        }

        /// <summary>
        /// Get the yield strength (Fy) from the grade.  EG. A572-50, yield = 50 ksi
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        private static decimal GetYieldKsi(string grade)
        {
            logger.Trace("grade: {0}", grade);
            if (string.IsNullOrEmpty(grade))
            {
                Exception ex = new Exception("Grade cannot be null or empty");
                logger.Fatal(ex);
                throw ex;
            }
            string[] w = grade.Split('-');
            string sYield = string.Empty;
            decimal yield = 0;
            if (w.Length == 2)
            {
                sYield = w[1];
            }

            decimal.TryParse(sYield, out yield);
            return yield;
        }

        //public static MemoryStream Serialize(object obj)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    BinaryFormatter bf = new BinaryFormatter();
        //    bf.Serialize(ms, obj);

        //    return ms;
        //}

        //public static T Deserialize<T>(MemoryStream ms)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    ms.Position = 0;
        //    return (T)bf.Deserialize(ms);
        //}

        /// <summary>
        /// Using serialization and deserialization, perform a deep copy of the object. 
        /// Note that to perform serialization and deserialization all types must be marked as "serializable".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CloneObject<T>(object obj)
        {
            //MemoryStream ms = Serialize(obj);
            //return Deserialize<T>(ms);

            //using (MemoryStream ms = new MemoryStream())
            //{
            // *** Serialize the object
            var json = JsonConvert.SerializeObject(obj);

            // *** Deserialize the object and send it back
            var deserialize = JsonConvert.DeserializeObject<T>(json);
            return deserialize;

            //    ////BinaryFormatter bf = new BinaryFormatter();
            //    //bf.Serialize(ms, obj);
            //    //ms.Position = 0;
            //    //return (T)bf.Deserialize(ms);
            //}

            //return null;
        }


        #endregion  // Methods - Get

    }
}
