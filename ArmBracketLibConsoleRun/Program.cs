using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmBracketLibConsoleRun
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsoleRunBare.Run();
            var dataSet = ConsoleRunBare.GenerateDataSet();

            //StructuralDesignNN.DesignNN(dataSet);

            Trainer.MainTrainer(null);
        }
    }
}
