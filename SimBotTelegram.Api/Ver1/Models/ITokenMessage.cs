using Newtonsoft.Json;

namespace SimBotTelegram.Api.Ver1.Models
{
    public class ITokenMessage
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("scan")]
        public string Scan { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
