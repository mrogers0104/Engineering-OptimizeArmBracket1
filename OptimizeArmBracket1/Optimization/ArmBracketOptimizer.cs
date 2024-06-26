using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using GeneticSharp;
using OptimizeArmBracket1.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizeArmBracket1.Optimization
{
    public class ArmBracketOptimizer
    {
        public event EventHandler RanIteration;

        protected virtual void OnRanIteration(EventArgs e)
        {
            if (RanIteration != null)
                RanIteration(this, e);
        }

        public ArmBracketDesignResults Optimize(ArmBracketDesignInputBundle structure)
        {
            var chromosome = new ArmBracketChromosome();

            var population = new Population(75, 200, chromosome);

            var fitness = new ArmBracketFitness(structure);

            //var selection = new EliteSelection(); // good
            //var selection = new TournamentSelection();
            //var selection = new StochasticUniversalSamplingSelection(); // NO!!
            var selection = new RankSelection();  // good
            //var selection = new TruncationSelection();

            var crossover = new UniformCrossover(0.5f);
            //var crossover = new OnePointCrossover();
            //var crossover = new TwoPointCrossover();
            //var crossover = new UniformCrossover();
            //var crossover = new CycleCrossover();

            var mutation = new UniformMutation();
            //var mutation = new FlipBitMutation();   // NO!!
            //var mutation = new DisplacementMutation();  // NO!!
            //var mutation = new ReverseSequenceMutation(); // NO!!
            //var mutation = new PartialShuffleMutation(); // NO!!

            var termination = new FitnessStagnationTermination(10);

            var ga = new GeneticAlgorithm(
                population,
                fitness,
                selection,
                crossover,
                mutation);

            ga.Termination = termination;

            ga.GenerationRan += GenerationRan;

            ga.Start();

            var structRes = ga.BestChromosome as ArmBracketChromosome;

            return structRes.Results;
        }

        private void GenerationRan(object sender, EventArgs e)
        {
            GeneticAlgorithm ga = sender as GeneticAlgorithm;

            var bktFitness = ga.Fitness as ArmBracketFitness;

            var results = bktFitness.LatestResults;

            var args = new ArmBktEventArgs();

            args.ArmBktResults = results;
            var bktChromosome = ga.BestChromosome as ArmBracketChromosome;
            var bkt = bktChromosome.Results.ArmBracketDesignOutputs[0].BracketDTO;

            args.Fitness = (bkt == null ? double.MinValue : bktFitness.CurrentFitness);

            OnRanIteration(args);
        }
    }
}