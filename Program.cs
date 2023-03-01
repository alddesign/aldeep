namespace AlDeep
{
    public class Program
    {
        public const int dataLength = 784;
        static void Main(string[] args)
        {
            //Extract the .7z files first:
            string trainCsvPath = @".\data\mnist_train.csv"; //100k
            string testCsvPath = @".\data\mnist_test.csv"; //10k

            Dataset testSet = new Dataset(testCsvPath);

            NeuralNetwork network = new NeuralNetwork(2);
            network.DefineLayer(0, dataLength);
            network.DefineLayer(1, 9);
            network.Initialize();
            network.RandomizeWeightsAndBiases();

            string saveFilePath = network.SaveToFile(true);
            network.RandomizeWeightsAndBiases();
            network.LoadFromFile(saveFilePath);

            Result result = network.Run(testSet);

            result.Print();

            return;
        }
    }
}

