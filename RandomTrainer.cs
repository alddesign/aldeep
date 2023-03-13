using System.Threading;

namespace AlDeep
{
    public class RandomTrainer
    {
        private Thread[] workers;
        private SaveManager[] workerSaveManagers;
        private WorkerData[] workerData;
        private NeuralNetwork[] workerNetworks;
        private NeuralNetwork network;
        private Dataset set;

        public RandomTrainer(NeuralNetwork network, Dataset set)
        {
            this.network = network;
            this.set = set;
        }

        #region Train
        public void Train(int noOfThreads, int totalIterations,  bool saveBestNetwork, int updateIntervalSec = 10)
        {
            int setSize = this.set.entries.Length;
            int iterationsPerThread = totalIterations / noOfThreads;
            int restIterations = totalIterations - iterationsPerThread * noOfThreads;
            this.workers = new Thread[noOfThreads];
            this.workerNetworks = new NeuralNetwork[noOfThreads];
            this.workerSaveManagers = new SaveManager[noOfThreads];
            this.workerData = new WorkerData[noOfThreads];

            //Create Threads
            SaveManager saveManager = new SaveManager();
            saveManager.Save(this.network);
            for(int tid = 0; tid < noOfThreads; tid++)
            {                
                this.workerNetworks[tid] = saveManager.Load();
                this.workerSaveManagers[tid] = new SaveManager();
                this.workerData[tid] = new WorkerData();

                this.workers[tid] = new Thread(Worker);
                this.workers[tid].Start(new WorkerArgs()
                { 
                    iterations = tid == 0 ? iterationsPerThread + restIterations : iterationsPerThread,   
                    saveBestNetwork = saveBestNetwork, 
                    tid = tid
                });
            }
            saveManager.Clear();
            saveManager = null;

            //Run Training
            Console.WriteLine("### Start random training on {0} threads, {1} iterations, set size: {2}", noOfThreads, totalIterations, setSize);
            
            bool running = true;
            double bestCorrectPercent = 0.0;
            int bestCorrect = 0;
            int bestTid = 0;
            int iterationsDone = 0;
            while(running)
            {
                Thread.Sleep(updateIntervalSec * 1000);
                
                running = false;
                iterationsDone = 0;
                for(int tid = 0; tid < noOfThreads; tid++)
                {
                    if(this.workers[tid].ThreadState == ThreadState.Running)
                    {
                        running = true;
                    }
            
                    if(this.workerData[tid].bestCorrect > bestCorrect)
                    {
                        bestCorrect = this.workerData[tid].bestCorrect;
                        bestTid = tid;
                    }

                    iterationsDone += this.workerData[tid].iteration;
                }
                

                bestCorrectPercent = Math.Round((double)bestCorrect/(double)setSize * 100, 2);
                Console.WriteLine(String.Format("Iteration: {0}/{1}. Best correct: {2}%", iterationsDone, totalIterations, bestCorrectPercent));
            }

            //Save best Network          
            string saveFilePath = "<not saved>";
            if(saveBestNetwork)
            {
                saveFilePath = this.workerSaveManagers[bestTid].ToFile(String.Format(@".\save\{0}_percent_{1}.aldeep",bestCorrectPercent, DateTime.Now.Ticks));
            }

            double totalAllThreadsDuration = 0.0;
            for(int tid = 0; tid < noOfThreads; tid++)
            {
                totalAllThreadsDuration += this.workerData[tid].totalDuration;
            }
            double avgIterationDuration = (totalAllThreadsDuration / (double)noOfThreads) / (double)totalIterations; 
    
            Console.WriteLine("### Random training finished.");
            Console.WriteLine(String.Format("### Best correct: {0}%.", bestCorrectPercent));
            Console.WriteLine(String.Format("### Average iteration duration: {0}ms.", Math.Round(avgIterationDuration, 2)));
            Console.WriteLine(String.Format("### Save file path: {0}.", saveFilePath));
        }

        private void Worker(object input)
        {
            WorkerArgs args = (WorkerArgs)input;
            int iterations = args.iterations;
            int tid = args.tid;
            bool saveBestNetwork = args.saveBestNetwork;
            int correct = 0;
            int bestCorrect = 0;

            Speed speed = new Speed();
            speed.Start();
            for(int i = 1; i <= iterations; i++)
            {
                this.workerData[tid].iteration = i;

                //Change the network:
                this.workerNetworks[tid].RandomizeWeightsAndBiases();

                //Run the dataset:
                correct = this.workerNetworks[tid].Run(set);

                //Check new best correct
                if(correct > bestCorrect)
                {
                    bestCorrect = correct;
                    this.workerData[tid].bestCorrect = bestCorrect;

                    if(saveBestNetwork)
                    {
                        this.workerSaveManagers[tid].Save(this.workerNetworks[tid]);
                    }
                }
            }

            this.workerData[tid].totalDuration = speed.End();
        }

        private struct WorkerArgs
        {
            public int iterations;
            public int tid;
            public bool saveBestNetwork;
        }

        private struct WorkerData
        {
            public int iteration;
            public int bestCorrect;
            public double totalDuration;
        }
        #endregion
    }
}
