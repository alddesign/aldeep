namespace AlDeep
{
    public class NeuralNetwork
    {
        Layer[] layers = new Layer[0];
        public double minBias = -10.0;
        public double maxBias = 0.0;
        public double minWeight = -1.0;
        public double maxWeight = 1.0;

        MemoryStream savedLayers = null;

        public NeuralNetwork(int noOfLayers)
        {
            this.layers = new Layer[noOfLayers];
        }

        public void SaveLayers()
        {
            this.savedLayers = new MemoryStream();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Layer[]));

            serializer.Serialize(this.savedLayers, this.layers);
            this.savedLayers.Seek(0, SeekOrigin.Begin);
        }

        public void LoadLayers()
        {
            if(this.savedLayers == null)
            {
                throw new Exception("Cannot load layers. No layers saved.");
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Layer[]));
            this.layers = (Layer[])serializer.Deserialize(this.savedLayers);

        }

        public void DefineLayer(int no, int noOfNodes)
        {
            this.layers[no] = new Layer(noOfNodes);
        }

        public void Initialize()
        {
            Layer layer;
            Layer childLayer;
            int l = this.layers.Length - 1;

            for(int i = 0; i < l; i++)
            {
                layer = this.layers[i]; 
                childLayer = this.layers[i + 1];

                for(int i2 = 0; i2 < layer.nodes.Length; i2++)
                {                  
                    layer.nodes[i2].weights = this.initArray(childLayer.nodes.Length, 0.0);
                }
            }
        } 

        public void RandomizeWeightsAndBiases()
        {
            int l = 0;
            Layer layer;
            Random random = new Random();

            //Weights for layers 0..n-1
            l = this.layers.Length - 1;
            for(int i = 0; i < l; i++)
            {
                layer = this.layers[i]; 
                for(int i2 = 0; i2 < layer.nodes.Length; i2++)
                {
    	            //initialize weights
                    for(int i3 = 0; i3 < layer.nodes[i2].weights.Length; i3++)
                    {
                        layer.nodes[i2].weights[i3] = random.NextDouble(this.minWeight, this.maxWeight);
                    }
                }
            }

            //Biases for layer 1..n
            l = this.layers.Length;
            for(int i = 1; i < l; i++)
            {
                layer = this.layers[i]; 
                for(int i2 = 0; i2 < layer.nodes.Length; i2++)
                {
                    layer.nodes[i2].bias = random.NextDouble(this.minBias, this.minWeight);                    
                }
            }
        }

        
        public Result Run(Dataset set)
        {
            Result result = new Result();
            result.total = set.entries.Length;

            int runResult = 0;
            int correct = 0;
            result.StartTimer();
            for(int i = 0; i < set.entries.Length; i++)
            {
                runResult = this.RunEntry(set.entries[i]);
                correct = set.results[i];
                result.correct += runResult == correct ? 1 : 0;
            }
            result.StopTimer();

            return result;
        }

        private int RunEntry(double[] input)
        {
            int l = Program.dataLength;

            for(int i = 0; i < l; i++)
            {
                this.layers[0].nodes[i].value = input[i];
            }

            for(int i = 1; i < this.layers.Length; i++)
            {
                this.CalcLayer(i);
            }

            //Get the brightes node from the last layer
            Layer lastLayer = this.layers[this.layers.Length - 1];
            double max = -9999999;
            int result = 0;
            for(int i = 0; i < lastLayer.nodes.Length; i++)
            {
                if(lastLayer.nodes[i].value > max)
                {
                    max = lastLayer.nodes[i].value;
                    result = i + 1;
                }
            }

            return result;
        }

        private void CalcLayer(int layerId)
        {
            Layer layer = this.layers[layerId];
            Layer parentLayer = this.layers[layerId - 1];

            double value = 0.0;

            for(int i = 0; i < layer.nodes.Length; i++)
            {
                value = 0.0;
                for(int i2 = 0; i2 < parentLayer.nodes.Length; i2++)
                {
                    value += parentLayer.nodes[i2].weights[i] * parentLayer.nodes[i2].value;
                }

                layer.nodes[i].value = this.LogSigmoid(value + layer.nodes[i].bias);
                value = value;
            }    
        }

        public double LogSigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        private double[] initArray(int length, double value)
        {
            double[] array = new double[length];
            for(int i = 0; i < length; i++)
            {
                array[i] = value;
            }

            return array;
        }
    }
}
