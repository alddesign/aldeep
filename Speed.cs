namespace AlDeep
{
    public class Speed
    {
        private DateTime started; 

        public Speed(bool start = false)
        {
            if(start)
            {
                this.Start();
            }
        }

        public void Start()
        {
            this.started = DateTime.Now;
        }

        public double End(int decimals = 2)
        {
            return (double)Math.Round((DateTime.Now - this.started).TotalMilliseconds, decimals);
        }
    }
}
