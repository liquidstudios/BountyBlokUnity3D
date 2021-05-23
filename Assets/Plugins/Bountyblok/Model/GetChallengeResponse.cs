using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bountyblok.client
{
    /// <summary>
    /// Class GetChallengeResponse returns a challenge progress.
    /// </summary>
    [JsonObject(IsReference = false)]
    public class GetChallengeResponse
    {
        [JsonProperty("challenge_id")]
        public Guid ChallengeID { get; set; }

        [JsonProperty("challenge_name")]
        public string ChallengeName { get; set; }

        [JsonProperty("challenge_start")]
        public string StartDate { get; set; }

        [JsonProperty("challenge_end")]
        public string EndDate { get; set; }

        [JsonProperty("challenge_description")]
        public string Description { get; set; }

        [JsonProperty("challenge_rewards")]
        public List<ChallengeRewardViewModel> ChallengeRewards { get; set; }

        [JsonProperty("tasks")]
        public List<TaskProgressViewModel> TaskProgresses { get; set; }

        [JsonProperty("tasks_required")]
        public int TasksRequired { get; set; }

        [JsonProperty("tasks_completed")]
        public int TasksCompleted { get; set; }
    }
}
