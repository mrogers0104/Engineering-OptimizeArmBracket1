using System;
using ArmBracketDesignLibrary.Helpers;
using MaterialsLibrary;

namespace ArmBracketDesignLibrary.Materials
{
    public class StructuralBolts
    {

        #region StructuralBoltGrades enum

        public enum StructuralBoltGrades
        {
            None = 0,
            A325 = 1,
            A449 = 2,
            A354BC = 3
        }

        #endregion



        public static string GetBoltGradeASTMText(StructuralBoltGrades grade)
        {
            return Enum.GetName(typeof(StructuralBoltGrades), grade);
        }

        public static double GetMinimumBoltSpacing(double boltDiameter)
        {
            //per Hock Lim, 2 2/3 * bolt diameter, rounded to 1/4 in. (3" min.)

            double distance = boltDiameter * 2.666667;
            if (distance < 3) return 3;

            //return LUtility.LMath.Rounding.RoundUp(distance, .25);
            return distance.RoundUp(0.25);
        }

        public static double GetBoltMinimumEdgeDistance(double boltDiameter)
        {

            //logger.Debug("Getting bolt minimum edge distance from stored structural bolt properties for bolt diam: {0}", boltDiameter);
            StructuralBolt_ML sprop = MaterialsLibraryBO.GetStructuralBolts(false).Find(sb => (decimal)sb.Diameter == (decimal)boltDiameter);

            if (sprop != null)
            {
                //logger.Debug("Found bolt minimum edge distance: {0}", sprop.MinimumEdgeDistance);
                return Convert.ToDouble(sprop.MinimumEdgeDistance);
            }

            Exception ex = new Exception(string.Format("GetBoltMinimumEdgeDistance -> No edge distance found for bolt diameter '{0}'.", boltDiameter));
            //            logger.Fatal(ex);
            throw ex;

        }

        public static double GetBoltMinimumEdgeDistance(double boltDiameter, double boltHoleDiameter, double plateThickness, double shearForce, double plateFy, double plateFu)
        {
            //            logger.Debug("Getting bolt minimum edge distance for:");
            //            logger.Debug("\tbolt diam: {0}", boltDiameter);
            //            logger.Debug("\tbolt bolt hole diameter: {0}", boltHoleDiameter);
            //            logger.Debug("\tbolt plate thickness: {0}", plateThickness);
            //            logger.Debug("\tbolt shear force: {0}", shearForce);
            //            logger.Debug("\tbolt plate Fy: {0}", plateFy);
            //            logger.Debug("\tbolt plate Fu: {0}", plateFu);

            // I guess we are just making sure that a bolt of this diameter exists because this boltprop is never used.
            StructuralBolt_ML boltProp = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            double distance = 0;

            if (boltProp != null)
            {
                distance = GetBoltMinimumEdgeDistance(boltDiameter);

                double le = 1.2 * shearForce / plateFu * plateThickness;
                distance = Math.Max(distance, le);

                le = plateThickness + boltDiameter / 2;
                distance = Math.Max(distance, le);

                le = .5 * (shearForce / (.58 * plateFy * plateThickness) + boltHoleDiameter);
                distance = Math.Max(distance, le);

                //                logger.Debug("Minimum edge distance is: {0}", distance);

                return distance;
            }

            Exception ex = new Exception(string.Format("GetBoltMinimumEdgeDistance -> No edge distance found for bolt diameter '{0}'.", boltDiameter));
            //            logger.Fatal(ex);
            throw ex;

        }

        public static StructuralBoltGrades GetBoltGrade(double diameter, double fy)
        {
            try
            {
                StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)diameter && (decimal)bp.FyKsi == (decimal)fy);

                StructuralBoltGrades grade = StructuralBoltGrades.None;

                if (bprop == null)
                {
                    return StructuralBoltGrades.None;
                }

                string specOnly = MaterialsLibraryBO.GetASTMSpecOnly(bprop.MaterialSpecification).ToUpper();

                foreach (string name in Enum.GetNames(typeof(StructuralBoltGrades)))
                {
                    if (name.ToUpper().Contains(specOnly))
                    {
                        grade = (StructuralBoltGrades)Enum.Parse(typeof(StructuralBoltGrades), name);
                    }

                }
                return grade;

            }
            catch (Exception)
            {
                Exception ex = new Exception(string.Format("GetBoltGrade -> No bolt grade found for bolt diameter {0} and fy {1}.", diameter, fy));
                //                logger.Fatal(ex);
                throw ex;
            }

        }

        public static double GetHeavyHexNutHeight(double boltDiameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.NutHeight);
            }

            return 0d;
        }

        public static double GetThreadCountPerInch(double boltDiameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.ThreadCountPerInch);
            }

            return 0d;
        }

        public static double GetBoltRootArea(double boltDiameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.RootArea);
            }

            return 0d;

        }

        public static double GetBoltGrossArea(double boltDiameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.GrossArea);
            }

            return 0d;

        }

        public static double GetFlatWasherHeight(StructuralBoltGrades grade, double boltDiameter)
        {
            return GetFlatWasherHeight(MaterialsLibraryBO.GetASTMSpecOnly(grade.ToString()), boltDiameter);
        }


        public static double GetFlatWasherHeight(string astmSpec, double boltDiameter)
        {

            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(
                bp => (decimal)bp.Diameter == (decimal)boltDiameter
                && bp.MaterialSpecification.Contains(astmSpec));

            if (bprop != null)
            {
                return (Convert.ToDouble(bprop.FlatWasherThickness));
            }

            Exception ex = new Exception(string.Format("Flat washer thickness not found for {0}\" grade {1} bolt", boltDiameter, astmSpec));
            //            logger.Fatal(ex);
            throw ex;

        }

        public static double GetBoltTensileArea(double boltDiameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.TensileArea);
            }

            return 0d;
        }

        public static double GetBoltFu(StructuralBoltGrades grade, double diameter)
        {
            string astmSpec = MaterialsLibraryBO.GetASTMSpecOnly(grade.ToString());
            return GetBoltFu(astmSpec, diameter);
        }

        public static double GetBoltFu(string astmSpec, double diameter)
        {

            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(
                bp => (decimal)bp.Diameter == (decimal)diameter
                && MaterialsLibraryBO.GetASTMSpecOnly(bp.MaterialSpecification).Equals(astmSpec, StringComparison.InvariantCultureIgnoreCase));

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.FuKsi);
            }

            return 0;
        }

        public static double GetBoltFy(StructuralBoltGrades grade, double diameter)
        {
            string astmSpec = MaterialsLibraryBO.GetASTMSpecOnly(grade.ToString());
            return GetBoltFy(astmSpec, diameter);
        }

        public static double GetBoltFy(string astmSpec, double diameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(
                bp => (decimal)bp.Diameter == (decimal)diameter
                && bp.MaterialSpecification.Equals(astmSpec, StringComparison.InvariantCultureIgnoreCase));

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.FyKsi);
            }

            return 0;
        }

        public static double GetBoltShaftWeightPerInch(double boltDiameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.BoltShaftWeightPerInch);
            }

            return 0d;

        }

        public static double GetBoltHeadAndNutWeight(double boltDiameter)
        {
            StructuralBolt_ML bprop = StructuralBolt_ML.GetStructuralBolts(false).Find(bp => (decimal)bp.Diameter == (decimal)boltDiameter);

            if (bprop != null)
            {
                return Convert.ToDouble(bprop.BoltHeadAndNutWeight);
            }

            return 0d;

        }

        public static double GetBoltShearStrength(string astmSpec, double boltDiameter)
        {
            if (string.IsNullOrEmpty(astmSpec) || boltDiameter < Globals.Epsilon)
            {
                return 0d;
            }

            //.45 is the shear factor defined by ASCE 48-05, section 6.2.2.
            return .45 * GetBoltFu(astmSpec, boltDiameter);
        }


    }
}