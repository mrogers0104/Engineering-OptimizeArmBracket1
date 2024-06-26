using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialsLibrary;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ICommonBolt> bolts = MaterialsLibraryBO.GetAllCommonBolts(false);
            ICommonBolt bolt1 = bolts[18];
            ICommonBolt bolt2 = bolts[29];

            AnchorBolt_ML bolt1Downcast = (AnchorBolt_ML)bolt1;
            StructuralBolt_ML bolt2Downcast = (StructuralBolt_ML)bolt2;

            Console.WriteLine("bolt1.Diameter: {0}", bolt1.Diameter);
            Console.WriteLine("bolt1.MaterialSpecification: {0}", bolt1.MaterialSpecification);
            Console.WriteLine("bolt1Downcast.QtGradeName: {0}", bolt1Downcast.QtGradeName);

            Console.WriteLine("bolt2.Diameter: {0}", bolt2.Diameter);
            Console.WriteLine("bolt2.MaterialSpecification: {0}", bolt2.MaterialSpecification);
            Console.WriteLine("bolt2Downcast.HoleDiameter: {0}", bolt2Downcast.HoleDiameter);


            int x = 1;
        }
    }
}
