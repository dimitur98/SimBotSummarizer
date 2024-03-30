using Newtonsoft.Json;

namespace SimBotTelegram.Api.Ver1.Models
{
    public class HypeMessage
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("mcap")]
        public decimal? MCap { get; set; }

        [JsonProperty("alarm_type")]
        public string AlarmType { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
