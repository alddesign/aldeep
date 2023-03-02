namespace AlDeep
{
    public class SaveManager
    {
        private const string saveFileDir = @".\save\";

        private MemoryStream savedNeuralNetwork = null;

        public void Save(NeuralNetwork network)
        {
            savedNeuralNetwork = new MemoryStream();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(NeuralNetwork));

            serializer.Serialize(savedNeuralNetwork, network);
            savedNeuralNetwork.Seek(0, SeekOrigin.Begin);
        }

        public string ToFile(string filePath = "")
        {
            if(savedNeuralNetwork == null)
            {
                throw new Exception("No neural network saved or loaded from file.");
            }

            filePath = String.IsNullOrEmpty(filePath) ? GetSaveFilePath() : filePath;

            savedNeuralNetwork.Seek(0, SeekOrigin.Begin);
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
            savedNeuralNetwork.WriteTo(fileStream);
            fileStream.Close();
            savedNeuralNetwork.Seek(0, SeekOrigin.Begin);

            return filePath;
        }

        public NeuralNetwork Load()
        {
            if(savedNeuralNetwork == null)
            {
                throw new Exception("No neural network saved or loaded from file.");
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(NeuralNetwork));
            
            savedNeuralNetwork.Seek(0, SeekOrigin.Begin);
            NeuralNetwork network = (NeuralNetwork)serializer.Deserialize(savedNeuralNetwork);
            savedNeuralNetwork.Seek(0, SeekOrigin.Begin);

            return network;
        }

        /// <summary>
        /// Loads the network from file into memory.
        /// </summary>
        public void FromFile(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            savedNeuralNetwork = new MemoryStream();

            savedNeuralNetwork.Seek(0, SeekOrigin.Begin);
            fileStream.CopyTo(savedNeuralNetwork);
            fileStream.Close();
            savedNeuralNetwork.Seek(0, SeekOrigin.Begin);
        }

        public void Clear()
        {
            if(savedNeuralNetwork != null)
            {
                savedNeuralNetwork.Close();
                savedNeuralNetwork = null;
            }
        }

        #region Helpers
        private string GetSaveFilePath()
        {
            return saveFileDir + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss.fff") + ".aldeep";
        }
        #endregion
    }
}
