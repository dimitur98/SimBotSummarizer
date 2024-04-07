using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class EthTrackerSignal
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "eth_tracker_wallet_id")]
        public uint EthTrackerWalletId { get; set; }
        public EthTrackerWallet EthTrackerWallet { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "date")]
        public DateTime Date { get; set; }
    }
}
