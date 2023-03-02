namespace AlDeep
{
    public class Speed
    {
        private DateTime started; 

        public Speed()
        {

        }

        public void Start()
        {
            this.started = DateTime.Now;
        }

        public double End(int decimals = 3)
        {
            return (double)Math.Round((DateTime.Now - this.started).TotalMilliseconds, decimals);
        }
    }
}
