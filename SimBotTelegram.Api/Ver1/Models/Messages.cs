using Newtonsoft.Json;

namespace SimBotTelegram.Api.Ver1.Models
{
    public class Messages
    {
        [JsonProperty("sim_bot_messages")]
        public List<SimBotMessasge> SimBotMessasges { get; set; }

        [JsonProperty("hype_messages")]
        public List<HypeMessage> HypeMessages { get; set; }

        [JsonProperty("itoken_messages")]
        public List<ITokenMessage> ITokenMessages { get; set; }

        [JsonProperty("eth_tracker_meesages_1")]
        public List<EthTrackerMessage> EthTrackerMessages1 { get; set; }

        [JsonProperty("eth_tracker_meesages_2")]
        public List<EthTrackerMessage> EthTrackerMessages2 { get; set; }
    }
}
