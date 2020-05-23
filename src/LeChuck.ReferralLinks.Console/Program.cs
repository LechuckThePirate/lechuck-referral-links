#region using directives

using System.Threading;
using LeChuck.ReferralLinks.Crosscutting.Classes;

#endregion

namespace LeChuck.ReferralLinks.Console
{
    class Program
    {
        static void Main()
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