using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using Newtonsoft.Json;

namespace OptimizeArmBracket1.Helpers
{
    public static class ModelCreator
    {

        public static ArmBracketDesignInputBundle CreateArmBracketModel()
        {
            //string jsonPath = "arm2.json";
            //string jsonPath = "arm2Stiffeners.json";
            string jsonPath = "CA1_Davit_Arm_InputStiffeners.json";
            //string jsonPath = "CA1_Davit_Arm_Input.json";
            string json;
            using (StreamReader sr = new StreamReader(jsonPath))
            {
                json = sr.ReadToEnd();
            }

            ArmBracketDesignInputBundle bundle = JsonConvert.DeserializeObject<ArmBracketDesignInputBundle>(json);

            return bundle;
        }
    }
}
