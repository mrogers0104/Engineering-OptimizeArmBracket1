using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OptimizeArmBracket1.Analysis
{
    internal class ArmBracketAnalyzer
    {
        //public AnalysisResults Analyze(PixelStructure pixelStructure)
        public ArmBracketDesignResults Analyze(ArmBracketDesignInputBundle bktStructure)
        {

            var bktModel = BuildModel(bktStructure);

            var results = AnalyzeModel(bktModel);

            return results;
        }

        public ArmBracketDesignResults AnalyzeModel(ArmBracketDesignInputBundle bktModel)
        {

            //List<ArmBracketDesignResults> dataSet = new List<ArmBracketDesignResults>();

            //List<decimal> wallThicknesses = new List<decimal> { 0.50m, 0.625m, 0.75m, 0.875m, 1.00m, 1.25m };

            ////string jsonPath = "CA1_Davit_Arm_Input.json";
            ////string jsonPath = "arm2.json";
            ////string jsonPath = "arm2Stiffeners.json";
            //string jsonPath = "CA1_Davit_Arm_InputStiffeners.json";
            //string json;
            //using (StreamReader sr = new StreamReader(jsonPath))
            //{
            //    json = sr.ReadToEnd();
            //}

            //ArmBracketDesignInputBundle bundle
            //    = JsonConvert.DeserializeObject<ArmBracketDesignInputBundle>(json);

            //var bktInput = bundle.Inputs[0].UserInputs.CustomBracketInput;

            //double minBlackWeight = double.MaxValue;
            //int cntrlBkt = -1;

            //int cnt = -1;
            //foreach (var thick in wallThicknesses)
            //{
            //    for (decimal dia = 0.625m; dia <= 1.50m; dia += 0.125m)
            //    {
            //        for (int num = 2; num < 12; num += 2)
            //        {
            //            bktInput.BoltDiameter = dia;
            //            bktInput.TotalBoltQty = num;
            //            bktInput.BracketThick = thick;

            //            ArmBracketDesignResults results = DesignEngine.RunDesigns(bundle);
            //            if (results.ArmBracketDesignOutputs[0].DesignWorked)
            //            {
            //                cnt++;
            //                dataSet.Add(results);

            //                if (results.ArmBracketDesignOutputs[0].BracketDTO.BlackWeight < minBlackWeight)
            //                {
            //                    minBlackWeight = results.ArmBracketDesignOutputs[0].BracketDTO.BlackWeight;
            //                    cntrlBkt = cnt;
            //                }
            //            }
            //        }
            //    }
            //}

            ArmBracketDesignResults results = DesignEngine.RunDesigns(bktModel);

            return results;
        }

        public ArmBracketDesignInputBundle BuildModel(ArmBracketDesignInputBundle bundle)
        {
            var bktInput = bundle.Inputs[0].UserInputs.CustomBracketInput;

            bktInput.BoltDiameter = 1.0m;
            bktInput.TotalBoltQty = 4;
            bktInput.BracketThick = 0.1875m;
            bktInput.StiffenerQty = 0;
            bktInput.StiffenerThick = bktInput.BracketThick;

            return bundle;
        }
    }
}
