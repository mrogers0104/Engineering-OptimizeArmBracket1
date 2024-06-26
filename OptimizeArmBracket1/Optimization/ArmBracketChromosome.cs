using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizeArmBracket1.Helpers;
using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using GeneticSharp;

namespace OptimizeArmBracket1.Optimization
{
    internal class ArmBracketChromosome : ChromosomeBase
    {
        private delegate decimal ArmBracketGenes();
        private List<ArmBracketGenes> _genes = new List<ArmBracketGenes>();

        // Change the argument value passed to base constructor to change the length
        // of your chromosome.
        private int _numGenes;

        //public AnalysisResults Results { get; set; }
        public ArmBracketDesignResults Results { get; set; }

        /// <summary>
        /// A chromosome represents a potential solution to the arm bracket.
        /// </summary>
        /// <param name="numGenes">The number of genes in this chromosome (formerly numPixels)</param>
        public ArmBracketChromosome() : base(5)
        {
            // *** Define the gene delegates
            _genes.Add(BoltDiaGene);
            _genes.Add(BoltQtyGene);
            _genes.Add(BracketThicknessGene);
            _genes.Add(StiffenerQtyGene);
            _genes.Add(StiffenerThicknessGene);

            _numGenes = _genes.Count;

            int i = 0;
            foreach (var gene in _genes)
            {
                 ReplaceGene(i, new Gene(gene()));
                i++;
            }
        }

        /// <summary>
        /// Each gene corresponds to a specific structural element
        /// (EG., bean, column, joint, etc)
        /// For an arm bracket, a gene will represent:
        ///         Gene
        ///     Index   Name
        ///     0       Number of bolts
        ///     1       Bolt diameter
        ///     2       bracket thickness
        ///     3       stiffener thickness
        ///     4       stiffener qty
        /// </summary>
        /// <param name="geneIndex"></param>
        /// <returns></returns>
        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(_genes[geneIndex]());
        }

        public override IChromosome CreateNew()
        {
            return new ArmBracketChromosome();
        }

        /// <summary>
        /// Check chromosome (solution) to see if it is a valid design.
        /// </summary>
        /// <returns>
        /// Returns Arm Bracket DesignWorked, but returns true if Results or
        /// ArmBracketDesignOutputs is null.  Null indicates the chromosome
        /// has not been defined.
        /// </returns>
        public bool IsDeadOnArrival()
        {
            var bkt = this.Results?.ArmBracketDesignOutputs[0].BracketDTO;
            var boltCntrSpc = bkt?.BracketBoltCenterSpace ?? double.MinValue;
            var designWorked = this.Results?.ArmBracketDesignOutputs[0]?.DesignWorked ?? true;

            var isDead = !designWorked || boltCntrSpc < 2.0;

            return isDead;
        }

        private decimal BoltDiaGene()
        {
            List<decimal> boltDiameters = new List<decimal>();
            for (decimal dia = Globals.BoltDiaStart ; dia <= Globals.BoltDiaEnd; dia += Globals.BoltDiaIncrement)
            {
                boltDiameters.Add(dia);
            }

            Random rand = new Random();
            int idx = rand.Next(0, boltDiameters.Count);

            return boltDiameters[idx];
        }

        private decimal BoltQtyGene()
        {
            Random random = new Random();
            int evenQty = random.Next(2, Globals.MaxBoltQty);
            evenQty = evenQty % 2 == 0 ? evenQty : evenQty + 1;

            return (decimal) evenQty;
        }

        private decimal BracketThicknessGene()
        {
            Random random = new Random();
            int idx = random.Next(0, Globals.BracketThick.Count);

            return Globals.BracketThick[idx];
        }

         private decimal StiffenerQtyGene()
        {
            Random random = new Random();
            int idx = random.Next(0, Globals.StiffenerQty.Count);

            return (decimal) Globals.StiffenerQty[idx];
        }

       private decimal StiffenerThicknessGene()
        {
            Random random = new Random();
            int idx = random.Next(0, Globals.BracketThick.Count);

            return Globals.BracketThick[idx];
        }

    }
}