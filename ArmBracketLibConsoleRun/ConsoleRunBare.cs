using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ArmBracketLibConsoleRun
{
    public static class ConsoleRunBare
    {
        public static void Run()
        {
            DesignEngine eng = new DesignEngine();

            //string jsonPath = "CA1_Davit_Arm_Input.json";
            //string jsonPath = "arm2.json";
            //string jsonPath = "arm2Stiffeners.json";
            string jsonPath = "CA1_Davit_Arm_InputStiffeners.json";
            string json;
            using (StreamReader sr = new StreamReader(jsonPath))
            {
                json = sr.ReadToEnd();
            }

            ArmBracketDesignInputBundle bundle
                = JsonConvert.DeserializeObject<ArmBracketDesignInputBundle>(json);

            ArmBracketDesignResults results = DesignEngine.RunDesigns(bundle);
        }

        public static List<ArmBracketDesignResults> GenerateDataSet()
        {
            List<ArmBracketDesignResults> dataSet = new List<ArmBracketDesignResults>();

            List<decimal> wallThicknesses = new List<decimal> { 0.50m, 0.625m, 0.75m, 0.875m, 1.00m, 1.25m };

            //string jsonPath = "CA1_Davit_Arm_Input.json";
            //string jsonPath = "arm2.json";
            //string jsonPath = "arm2Stiffeners.json";
            string jsonPath = "CA1_Davit_Arm_InputStiffeners.json";
            string json;
            using (StreamReader sr = new StreamReader(jsonPath))
            {
                json = sr.ReadToEnd();
            }

            ArmBracketDesignInputBundle bundle
                = JsonConvert.DeserializeObject<ArmBracketDesignInputBundle>(json);

            var bktInput = bundle.Inputs[0].UserInputs.CustomBracketInput;

            double minBlackWeight = double.MaxValue;
            int cntrlBkt = -1;

            int cnt = -1;
            foreach (var thick in wallThicknesses)
            {
                for (decimal dia = 0.625m; dia <= 1.50m; dia += 0.125m)
                {
                    for (int num = 2; num < 12; num += 2)
                    {
                        bktInput.BoltDiameter = dia;
                        bktInput.TotalBoltQty = num;
                        bktInput.BracketThick = thick;

                        ArmBracketDesignResults results = DesignEngine.RunDesigns(bundle);
                        if (results.ArmBracketDesignOutputs[0].DesignWorked)
                        {
                            cnt++;
                            dataSet.Add(results);

                            if (results.ArmBracketDesignOutputs[0].BracketDTO.BlackWeight < minBlackWeight)
                            {
                                minBlackWeight = results.ArmBracketDesignOutputs[0].BracketDTO.BlackWeight;
                                cntrlBkt = cnt;
                            }
                        }
                    }
                }
            }

            return dataSet;
        }
    }
}