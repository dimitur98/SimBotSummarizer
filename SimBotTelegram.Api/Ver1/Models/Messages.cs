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
    }
}
