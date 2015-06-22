using GAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GenService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    /// <summary>
    /// interface with most valuable methods
    /// </summary>
    [ServiceContract]
    public interface IGenLib
    {
        //[OperationContract]
        void ga_OnGenerationComplete(object sender, GaEventArgs e);

        [OperationContract]
        bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation);

        [OperationContract]
        double EvaluateFitness(Chromosome chrom);

        [OperationContract]
        FittestValue Start(double crossoverProb, double mutationProb, int elitismPerc, int perPopulation, int populationCount);
    }
}
