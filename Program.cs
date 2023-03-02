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

            int i = 1000;

            Thread tr1 = new Thread(Work);
            tr1.Start(i);

            Thread tr2 = new Thread(Work);
            tr2.Start(i);

            while(true)
            {
                Thread.Sleep(1000);
                Console.WriteLine(String.Format("tr1: {0}, tr2: {1}", tr1.ThreadState.ToString(), tr2.ThreadState.ToString()));
            }

            /*
            Dataset trainingSet = new Dataset(trainCsvPath);

            NeuralNetwork network = new NeuralNetwork(2);
            network.DefineLayer(0, dataLength);
            network.DefineLayer(1, 9);
            network.minBias = -10.0;
            network.maxBias = 0;
            network.Initialize();
            network.RandomizeWeightsAndBiases();
            
            network.TrainRandom(trainingSet, 1000, true);
            SaveManager.ToFile();
            */

            /*
            Dataset testSet = new Dataset(testCsvPath);

            SaveManager.FromFile(@".\save\2023-03-02_09-02-11.459.save.aldeep.xml");
            NeuralNetwork network = SaveManager.Load();

            RunsResult result = network.Run(testSet);
            result.Print();
            */

            return;
        }

        static void Work(object input)
        {
            int l = (int)input;

            NeuralNetwork network = new NeuralNetwork(2);
            network.DefineLayer(0, dataLength);
            network.DefineLayer(1, 9);
            network.Initialize();
            network.RandomizeWeightsAndBiases();

            for(int i = 0; i < l; i++)
            {
                SaveManager.Save(network);
            }

        }
    }
}

