namespace PEA_TSP_1.Algorithms
{
    internal interface IAlgorithm
    {
        AlgorithmResult Result { get; }
        string Name { get; set; }
        void Invoke();
    }
}