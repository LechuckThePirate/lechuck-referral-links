using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LeChuck.ReferralLinks.Lambda.Timer
{
    public class Function
    {
        private readonly StartUp _startup;
        private readonly ProcessTimer _timer;

        public Function()
        {
            Console.WriteLine("Cold start");
            _timer = new ProcessTimer(true);
            _startup = new StartUp(_timer)
                    .ConfigureApplication()
                    .ConfigureServices()
                    .LoadServices();
            _timer.LogMarks();
        }

        public string FunctionHandler(string input, ILambdaContext context)
        { 
            _timer.Start();

            _startup
                .Run();

            _timer.LogMarks();

            return "Ok";
        }
    }
}
