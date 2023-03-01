namespace AlDeep
{
    public class Speed
    {
        private static DateTime started; 
        public static void Start()
        {
            Speed.started = DateTime.Now;
        }

        public static double End(int decimals = -1)
        {
            double durationMs = (DateTime.Now - Speed.started).TotalMilliseconds;
            return decimals >= 0 ? (double)Math.Round(durationMs, decimals) : durationMs;
        }
    }
}
