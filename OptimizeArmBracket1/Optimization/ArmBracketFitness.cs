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
    internal class ArmBracketFitness : IFitness
    {
        //FiniteElementModel _model;
        private ArmBracketDesignInputBundle _structure;

        public ArmBracketFitness(ArmBracketDesignInputBundle structure)
        {
            //_model = model;
            _structure = structure;
        }

        public ArmBracketDesignResults LatestResults { get; set; }

        public double CurrentFitness { get; private set; }

        public double Evaluate(IChromosome chromosome)
        {
            var bktChr = chromosome as ArmBracketChromosome;

            var bktAnalyzer = new ArmBracketAnalyzer();

            var model = bktAnalyzer.BuildModel(_structure);

            var genes = chromosome.GetGenes();
            Random rand = new Random();
            int i = 0;

            var bktInput = model.Inputs[0].UserInputs.CustomBracketInput;
            foreach (var gene in genes)
            {
                switch (i)
                {
                    case 0: // Bolt Diameter
                        bktInput.BoltDiameter = (decimal)gene.Value;
                        break;

                    case 1: // bolt Qty
                        bktInput.TotalBoltQty = Convert.ToInt32(gene.Value);
                        break;

                    case 2: // Bracket Thickness
                        bktInput.BracketThick = (decimal)gene.Value;
                        bktInput.ThruPlateThick = (decimal)gene.Value;
                        break;

                    case 3: // Stiffener Qty
                        bktInput.StiffenerQty = Convert.ToInt32(gene.Value);
                        break;

                    case 4: // Stiffener Thickness
                        bktInput.StiffenerThick = (decimal)gene.Value;
                        if (bktInput.StiffenerQty > 0 && bktInput.StiffenerThick > bktInput.BracketThick)
                        {
                            bktInput.StiffenerThick = bktInput.BracketThick;
                        }
                        break;
                        //default:
                        //    break;
                }

                i++;
            }
            var results = bktAnalyzer.AnalyzeModel(model);

            double wt = double.MinValue;

            if (bktChr == null)
            {
                return wt;
            }

            bktChr.Results = results;
            if (bktChr.IsDeadOnArrival())
            {
                return wt;
            }

            double lastWt = double.MinValue;
            if (this.LatestResults != null)
            {
                lastWt = this.LatestResults.ArmBracketDesignOutputs[0].BracketDTO.GalvWeight;
            }

            wt = results.ArmBracketDesignOutputs[0].BracketDTO?.GalvWeight ?? 0.0;

            //double diff = lastWt - wt;

            this.LatestResults = results;
            //this.CurrentFitness = diff;
            this.CurrentFitness = -wt;

            //return diff;
            return -wt;
        }
    }
}