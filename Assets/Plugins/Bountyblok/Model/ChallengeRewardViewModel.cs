using Newtonsoft.Json;

namespace bountyblok.client
{
    /// <summary>
    /// Class ChallengeRewardViewModel represents a challenge reward set on app.bountyblok.io UI Portal for a challenge.
    /// </summary>
    [JsonObject(IsReference = false)]
    public class ChallengeRewardViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public decimal? Value { get; set; }
    }
}
