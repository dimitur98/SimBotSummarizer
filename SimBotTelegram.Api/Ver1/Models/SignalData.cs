using Newtonsoft.Json;

namespace SimBotTelegram.Api.Ver1.Models
{
    public class SignalData
    {
        [JsonProperty("mcap")]
        public double MCap { get; set; }

        [JsonProperty("total_calls")]
        public int TotalCalls { get; set; }

        [JsonProperty("sell_tax")]
        public double? SellTax { get; set; }

        [JsonProperty("buy_tax")]
        public double? BuyTax { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
