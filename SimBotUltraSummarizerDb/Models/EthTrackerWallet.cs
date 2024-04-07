using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class EthTrackerWallet
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "eth_tracker_type_id")]
        public uint EthTrackerTypeId { get; set; }
        public EthTrackerType EthTrackerType { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "is_active")]
        public bool IsActive { get; set; }
    }
}
