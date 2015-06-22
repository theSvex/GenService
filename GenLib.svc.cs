using GAF;
using GAF.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GenService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    /// <summary>
    /// Class to perform generic algorithm for f6 binary function to resolve max arguments
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class GenLib : IGenLib
    {
        /// <summary>
        /// property to assign max generation count
        /// </summary>
        private int populationCount;
        public int PopulationCount
        {
            get { return populationCount; }
            set { populationCount = value; }
        }
        /// <summary>
        /// property to return fittest values
        /// </summary>
        private FittestValue fittest;
        public FittestValue Fittest
        {
            get { return fittest; }
            set { fittest = value; }
        }
        /// <summary>
        /// constructor to assign default value of max generations
        /// </summary>
        public GenLib()
        {
            PopulationCount = 10000;
        }
        /// <summary>
        /// contructor to asisgn max generation count
        /// </summary>
        /// <param name="popCount"></param>
        public GenLib(int popCount)
        {
            PopulationCount = popCount;
        }
        /// <summary>
        /// Method to evaluate value of parameters for fitness function and what is value of that fitness
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            //get the best solution
            var chrom = e.Population.GetTop(1)[0];

            //decode chromosone

            //get x and y
            var x1 = Convert.ToInt32(chrom.ToBinaryString(0, chrom.Count / 2), 2);
            var y1 = Convert.ToInt32(chrom.ToBinaryString(chrom.Count / 2, chrom.Count / 2), 2);

            //adjust range
            var rangeConst = 200 / (System.Math.Pow(2, chrom.Count / 2) - 1);
            var x = (x1 * rangeConst) - 100;
            var y = (y1 * rangeConst) - 100;

            Console.WriteLine("x:{0} y:{1} Fitness{2}", x, y, e.Population.MaximumFitness);
            Fittest = new FittestValue() { X = x, Y = y, Fit = e.Population.MaximumFitness };
        }
        /// <summary>
        /// Method to check if maximum count of generetions has been reached
        /// </summary>
        /// <param name="population"></param>
        /// <param name="currentGeneration"></param>
        /// <param name="currentEvaluation"></param>
        /// <returns></returns>
        public bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > PopulationCount;
        }
        /// <summary>
        /// Method to check what is fitness value of specific chromosone
        /// </summary>
        /// <param name="chrom"></param>
        /// <returns></returns>
        public double EvaluateFitness(Chromosome chrom)
        {
            double fitnessValue = -1;
            if (chrom != null)
            {
                //constant to keep x/y between -100/100 range
                var rangeConst = 200 / (System.Math.Pow(2, chrom.Count / 2) - 1);

                //get x and y
                var x1 = Convert.ToInt32(chrom.ToBinaryString(0, chrom.Count / 2), 2);
                var y1 = Convert.ToInt32(chrom.ToBinaryString(chrom.Count / 2, chrom.Count / 2), 2);

                //adjust range to -100/100
                var x = (x1 * rangeConst) - 100;
                var y = (y1 * rangeConst) - 100;

                //using binary f6 for fitness
                var tmp1 = System.Math.Sin(System.Math.Sqrt(x * x + y * y));
                var tmp2 = 1 + 0.001 * (x * x + y * y);
                var result = 0.5 + (tmp1 * tmp1 - 0.5) / (tmp2 * tmp2);

                fitnessValue = 1 - result;
            }
            else
            {
                //chrom is null
                throw new ArgumentNullException("chromosome", "The specified Chromosome is null.");
            }

            return fitnessValue;
        }
        /// <summary>
        /// Method to start Generic algorithm - returns fittest value from all generations
        /// arguments: crossover probability, mutation probability, elitism percentage, how many generations is to be reached
        /// </summary>
        /// <param name="crossoverProb"></param>
        /// <param name="mutationProb"></param>
        /// <param name="elitismPerc"></param>
        /// <param name="perPopulation"></param>
        /// <param name="populationCount"></param>
        /// <returns></returns>
        public FittestValue Start(double crossoverProb, double mutationProb, int elitismPerc, int perPopulation, int populationCount)
        {
            this.PopulationCount = populationCount;
            double crossoverProbability = crossoverProb;//0.65;
            double mutationProbability = mutationProb;// 0.08;
            int elitismPercentage = elitismPerc;//5;

            //create population
            var population = new Population(perPopulation, 44, false, false);

            //create the genetic operators
            var elite = new Elite(elitismPercentage);

            var crossover = new Crossover(crossoverProbability, true)
            {
                CrossoverType = CrossoverType.SinglePoint
            };

            var mutation = new BinaryMutate(mutationProbability, true);

            //create GA
            var ga = new GeneticAlgorithm(population, EvaluateFitness);

            //subscribe to the GAs generation complete event
            ga.OnGenerationComplete += ga_OnGenerationComplete;

            //add the operators to the ga process pipeline
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutation);

            //run the GA
            ga.Run(TerminateAlgorithm);

            return Fittest;
        }
    }
}
