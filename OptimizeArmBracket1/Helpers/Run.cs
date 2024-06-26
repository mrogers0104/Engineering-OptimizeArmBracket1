using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizeArmBracket1.Optimization;
using OptimizeArmBracket1.Helpers;
using GeneticSharp;

namespace OptimizeArmBracket1.Helpers
{
    internal class Run
    {
        int cnt = 0;

        public void OptimizeArmBracket()
        {
            var bktStructure = ModelCreator.CreateArmBracketModel();

            var optimizer = new ArmBracketOptimizer();

            optimizer.RanIteration += Optimizer_RanIteration;

            var bestResults = optimizer.Optimize(bktStructure);

            var bkt = bestResults.ArmBracketDesignOutputs[0].BracketDTO;
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("~~~ ~~~ ~~~ ~~~ BEST SOLUTION ~~~ ~~~ ~~~ ~~~");
            Console.WriteLine($"Bkt Weight: {bkt.GalvWeight:f3} lbs");
            Console.WriteLine($"{bkt.BracketHeight}\" X {bkt.BracketThick:f3}\" " +
                $"Bolts: {bkt.BoltQty} @ {bkt.BoltDiameter:f4}\" " +
                $"Cntr Spc: {bkt.BracketBoltCenterSpace:f3}");

        }

        private void Optimizer_RanIteration(object sender, EventArgs e)
        {
            var bktArg = e as ArmBktEventArgs;
            var results = bktArg.ArmBktResults;
            var fitness = bktArg.Fitness;

            cnt++;
            var bkt = results.ArmBracketDesignOutputs[0].BracketDTO;

            Console.WriteLine($"--- #{cnt} -----------------------------------------");
            Console.WriteLine($"Weight Diff: {fitness:f3} lbs");
            //Console.WriteLine($"Bkt Weight: {fitness:f3} lbs");
            if (bkt != null)
            {
                Console.WriteLine($"{bkt.BracketHeight}\" X {bkt.BracketThick:f3}\" " +
                    $"Bolts: {bkt.BoltQty} @ {bkt.BoltDiameter:f4}\" " +
                    $"Cntr Spc: {bkt.BracketBoltCenterSpace:f3}");
            }
            else {
                Console.WriteLine("Bracket not defined!");
            }
        }
    }
}