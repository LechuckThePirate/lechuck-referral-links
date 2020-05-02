using System;
using System.Threading;

namespace LeChuck.ReferralLinks.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var timer = new ProcessTimer(true);

            new StartUp(timer)
                .ConfigureApplication()
                .ConfigureServices()
                .LoadServices()
                .Run()
                .GetAwaiter().GetResult();

            System.Console.WriteLine("Initialization times");
            timer.Marks.ForEach(m => System.Console.WriteLine($" - {m.Label}: {m.Elapsed}"));
            System.Console.WriteLine($"Total time: {timer.Total()}");

            Thread.Sleep(int.MaxValue);
        }
    }
}
