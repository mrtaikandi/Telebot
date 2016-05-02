namespace TelebotConsole
{
    using System;

    public class Program
    {
        #region Public Methods and Operators

        public static void Main(string[] args)
        {
            try
            {
                var runtime = new TelebotConsole();
                runtime.RunAsync().Wait();
            }
            catch( Exception ex )
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }

        #endregion
    }
}