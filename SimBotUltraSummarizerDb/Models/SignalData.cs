using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class SignalData
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "signal_id")]
        public uint SignalId { get; set; }

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

        [Column(Name = "price")]
        public double? Price { get; set; }

        [Column(Name = "method_ids_hash_launched")]
        public int? MethodIdsHashLaunched { get; set; }

        [Column(Name = "method_ids_hash_rugged")]
        public int? MethodIdsHashRugged { get; set; }

        public double MethodIdsPercent => this.MethodIdsHashLaunched == 0 || !this.MethodIdsHashRugged.HasValue || !this.MethodIdsHashLaunched.HasValue ? 0 :((double)this.MethodIdsHashRugged.Value / (double)this.MethodIdsHashLaunched.Value);

        [Column(Name = "functions_text_launched")]
        public int? FunctionsTextLaunched { get; set; }

        [Column(Name = "functions_text_rugged")]
        public int? FunctionsTextRugged { get; set; }

        public double FunctionsTextPercent => this.FunctionsTextLaunched == 0 || !this.FunctionsTextRugged.HasValue || !this.FunctionsTextLaunched.HasValue ? 0 : ((double)this.FunctionsTextRugged.Value / (double)this.FunctionsTextLaunched.Value);

        [Column(Name = "clog")]
        public double? Clog { get; set; }

        [Column(Name = "is_trending")]
        public bool IsTrending { get; set; }

        [Column(Name = "date")]
        public DateTime Date { get; set; }

        [Column(Name = "edited_at")]
        public DateTime? EditedAt { get; set; }
    }
}
