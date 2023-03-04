using System.Threading;

namespace AlDeep
{
    public class Program
    {
        public const int dataLength = 784;
        static void Main(string[] args)
        {
            //Extract the .7z files first:
            string trainCsvPath = @".\data\mnist_train.csv"; //60k
            string testCsvPath = @".\data\mnist_test.csv"; //10k

            Dataset set = new Dataset(trainCsvPath);

            NeuralNetwork network = new NeuralNetwork(2);
            network.DefineLayer(0, dataLength);
            network.DefineLayer(1, 9);
            network.minBias = -10.0;
            network.maxBias = 0;
            network.Initialize();
            network.RandomizeWeightsAndBiases();
            
            RandomTrainer trainer = new RandomTrainer(network, set);
            trainer.Train(12, 200, true, 10);

            /*
            SaveManager saveManager = new SaveManager();
            saveManager.FromFile(@".\save\15,18_percent_638135551900753486.aldeep");
            NeuralNetwork network = saveManager.Load();

            int correct = network.Run(set);
            double correctPercent = Math.Round(((double)correct/(double)set.entries.Length) * 100, 2);
            Console.WriteLine(String.Format("Correct: {0}%. {1}/{2}", correctPercent, correct, set.entries.Length));
            */
            return;
        }
    }
}

