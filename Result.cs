namespace AlDeep
{
    public class Result
    {
        public int total = 0;
        public int correct = 0;
        private double totalDurationMs = 0.0;
        private DateTime started;
        public Result()
        {
            
        }

        public void Print()
        {
            Console.WriteLine("Total: {0}", this.total);
            Console.WriteLine("Correct: {0} ({1}%)", this.correct, this.getPercentCorrect());
            Console.WriteLine("Avg duration: {0}ms", this.getAvgRunDurationMs());
            Console.WriteLine("Total duration: {0}ms", Math.Round(this.totalDurationMs, 3));
        }

        public void StartTimer()
        {
            this.started = DateTime.Now;
        }

        public void StopTimer()
        {
            this.totalDurationMs = (DateTime.Now - started).TotalMilliseconds;
        }

        public double getPercentCorrect()
        {
            return total <= 0 ? 0.0 : (double)Math.Round(((double)this.correct / (double)this.total) * 100.0, 3);
        }

        public double getAvgRunDurationMs()
        {
            return this.total <= 0.0 ? 0.0 : (double)Math.Round((double)this.totalDurationMs / (double)this.total, 3);
        }
    }
}
