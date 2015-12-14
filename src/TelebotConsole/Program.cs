namespace TelebotConsole
{
    public class Program
    {
        #region Public Methods and Operators

        public static void Main(string[] args)
        {
            var runtime = new TelebotConsole();
            runtime.Run().Wait();
        }

        #endregion
    }
}