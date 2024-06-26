
using OptimizeArmBracket1.Helpers;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("~~~ Arm Bracket Design Optimization ~~~");

        // *** Using GeneticSharp: https://github.com/giacomelli/GeneticSharp
        // *** Using Frixel as a model: https://github.com/EmilPoulsen/Frixel

        var runArmBkt = new Run();

        runArmBkt.OptimizeArmBracket();
    }
}