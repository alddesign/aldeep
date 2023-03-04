namespace AlDeep
{
    public class SaveManager
    {
        private const string saveFileDir = @".\save\";

        private MemoryStream savedNetwork = null;

        /// <summary>
        /// Saves the network in memory.
        /// </summary>
        public void Save(NeuralNetwork network)
        {
            savedNetwork = new MemoryStream();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(NeuralNetwork));

            serializer.Serialize(savedNetwork, network);
            savedNetwork.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Writes the network saved in memory to a file.
        /// </summary>
        public string ToFile(string filePath = "")
        {
            if(savedNetwork == null)
            {
                throw new Exception("No neural network saved or loaded from file.");
            }

            filePath = String.IsNullOrEmpty(filePath) ? GetSaveFilePath() : filePath;

            savedNetwork.Seek(0, SeekOrigin.Begin);
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
            savedNetwork.WriteTo(fileStream);
            fileStream.Close();
            savedNetwork.Seek(0, SeekOrigin.Begin);

            return filePath;
        }

        /// <summary>
        /// Gets the networked saved in memory.
        /// </summary>
        public NeuralNetwork Load()
        {
            if(savedNetwork == null)
            {
                throw new Exception("No neural network saved or loaded from file.");
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(NeuralNetwork));
            
            savedNetwork.Seek(0, SeekOrigin.Begin);
            NeuralNetwork network = (NeuralNetwork)serializer.Deserialize(savedNetwork);
            savedNetwork.Seek(0, SeekOrigin.Begin);

            return network;
        }

        /// <summary>
        /// Loads the network from a file into memory.
        /// </summary>
        public void FromFile(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            savedNetwork = new MemoryStream();

            savedNetwork.Seek(0, SeekOrigin.Begin);
            fileStream.CopyTo(savedNetwork);
            fileStream.Close();
            savedNetwork.Seek(0, SeekOrigin.Begin);
        }

        public void Clear()
        {
            if(savedNetwork != null)
            {
                savedNetwork.Close();
                savedNetwork = null;
            }
        }

        #region Helpers
        private string GetSaveFilePath()
        {
            return saveFileDir + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.fff") + ".aldeep";
        }
        #endregion
    }
}
