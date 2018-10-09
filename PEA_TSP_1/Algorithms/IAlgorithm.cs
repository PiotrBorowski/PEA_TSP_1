namespace PEA_TSP_1.Algorithms
{
    internal interface IAlgorithm
    {
        AlgorithmResult Result { get; }
        void Invoke();
    }
}