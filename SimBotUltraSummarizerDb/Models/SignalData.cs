using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class SignalData
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "mcap")]
        public double MCap { get; set; }

        [Column(Name = "total_calls")]
        public int TotalCalls { get; set; }

        [Column(Name = "sell_tax")]
        public double? SellTax { get; set; }

        [Column(Name = "buy_tax")]
        public double? BuyTax { get; set; }

        [Column(Name = "date")]
        public DateTime Date { get; set; }
    }
}
