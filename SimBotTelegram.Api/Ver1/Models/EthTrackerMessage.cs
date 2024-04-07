using Newtonsoft.Json;

namespace SimBotTelegram.Api.Ver1.Models
{
    public class EthTrackerMessage
    {
        [JsonProperty("wallet_data")]
        public string WalletData { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
