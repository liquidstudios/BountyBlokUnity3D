using Newtonsoft.Json;
using System;

namespace bountyblok.client
{
    /// <summary>
    /// Class LogAppRequest builds an object that logs a task by App ID to bountyblok APIs.
    /// </summary>
    [JsonObject(IsReference = false)]
    public class LogAppRequest
    {
        /// <summary>
        /// Gets or sets the challenge id for the challenge this task should verify
        /// </summary>
        [JsonProperty("app_id")]
        public Guid AppID { get; set; }

        /// <summary>
        /// Gets or sets the account name of the user who is performing this task.
        /// </summary>
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the metadata key/value param for the task being logged.
        /// </summary>
        [JsonProperty("param")]
        public string Param { get; set; }

        /// <summary>
        /// Gets or sets the quantity of times this tasks should be logged. Usually set to 1.
        /// </summary>
        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the with progress to specify if the response should include the challenge progress
        /// </summary>
        [JsonProperty("with_progress")]
        public bool WithProgress { get; set; }
    }
}
