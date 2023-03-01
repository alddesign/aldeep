namespace AlDeep
{
    public class Dataset
    {
        public int[] results;
        public double[][] entries;

        public Dataset(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            this.results = new int[lines.Length];
            this.entries = new double[lines.Length][];

            double f = 0.99/255.0;
            for(int lineNo = 0; lineNo < lines.Length; lineNo++)
            {
                string[] lineParts = lines[lineNo].Split(",");

                this.entries[lineNo] = new double[Program.dataLength];
                this.results[lineNo] = int.Parse(lineParts[0]);

                for(int i = 0; i < Program.dataLength; i++)
                {
                    this.entries[lineNo][i] = double.Parse(lineParts[i + 1]) * f + 0.01;
                }
            }

            return;
        }

    }
}
