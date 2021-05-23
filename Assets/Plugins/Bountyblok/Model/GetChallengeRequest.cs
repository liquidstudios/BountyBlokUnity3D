using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bountyblok.client
{
    /// <summary>
    /// Class GetChallengeRequest returns a challenge progress.
    /// </summary>
    [JsonObject(IsReference = false)]
    public class GetChallengeRequest
    {
        /// <summary>
        /// Gets or sets the challenge id to obtain the challenge details and progress
        /// </summary>
        [JsonProperty("challenge_id")]
        public Guid ChallengeID { get; set; }

        /// <summary>
        /// Gets or sets the account name of the user. If specified, their progress will be returned in the response.
        /// </summary>
        [JsonProperty("account_name")]
        public string AccountName { get; set; }
    }
}
