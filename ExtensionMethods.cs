namespace AlDeep
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extends System.Random with a handy method
        /// </summary>
        public static double NextDouble(this Random random, double min, double max)
        {
            return(random.NextDouble() * (max - min) + min);
        }  
    }
}
