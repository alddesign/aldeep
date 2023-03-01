namespace AlDeep
{
    public class Layer
    {

        public Node[] nodes = null;
        public Layer(int noOfNodes)
        {
            this.nodes = new Node[noOfNodes];

            for(int i = 0; i < noOfNodes; i++)
            {
                this.nodes[i] = new Node();
            }
        }

        public Layer()
        {
            //Serialization needs an empty constructor
        }
    }
}
