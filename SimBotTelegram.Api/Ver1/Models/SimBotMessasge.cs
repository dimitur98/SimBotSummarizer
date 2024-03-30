using Newtonsoft.Json;

namespace SimBotTelegram.Api.Ver1.Models
{
    public class SimBotMessasge
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pair")]
        public string Pair { get; set; }

        [JsonProperty("signal_data")]
        public IEnumerable<SignalData> SignalData { get; set; }
    }
}
