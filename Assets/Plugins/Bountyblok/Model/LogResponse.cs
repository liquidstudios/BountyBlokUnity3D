using Newtonsoft.Json;
using System;

namespace bountyblok.client
{
    /// <summary>
    /// Class LogResponse represents the data that can be obtained after logging a task or receiving a silent post via web hook.
    /// </summary>
    [JsonObject(IsReference = false)]
    public class LogResponse
    {
        [JsonProperty("logged_tx")]
        public string LoggedTx { get; set; }

        [JsonProperty("task_completed_tx")]
        public string TaskCompletedTx { get; set; }

        [JsonProperty("challenge_completed_tx")]
        public string ChallengeCompletedTx { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("task_progress")]
        public TaskProgressViewModel TaskProgress { get; set; }

        [JsonProperty("challenge_progress")]
        public ChallengeProgressViewModel ChallengeProgress { get; set; }
    }
}
