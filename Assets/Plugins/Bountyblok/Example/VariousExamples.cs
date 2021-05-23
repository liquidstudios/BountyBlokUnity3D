using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bountyblok.client.Example
{
    public class VariousExamples
    {
        private static void Main()
        {
            Execute().Wait();
        }

        static async Task Execute()
        {
            // Retrieve the API key from the environment variables or web.config or appsettings
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_BOUNTYBLOK_KEY");
            var apiKey = "<< YOU_API_KEY >>";
            var client = new BountyblokClient(apiKey);
            var logTask = new LogTaskRequest();

            logTask.AccountName = "dimi1";

            // obtained on app.bountyblok.io challenge page
            logTask.ChallengeID = Guid.Parse("0e2e2793-cdc1-4135-b301-d55674a587e2");

            var paramDict = new Dictionary<string, string>
            {
                { "orderside" , "Buy" },
                { "exchange", "US" }
            };

            logTask.Param = JsonConvert.SerializeObject(paramDict);
            logTask.Quantity = 1;

            var response = await client.LogTaskAsync(logTask);
        }
    }
}
