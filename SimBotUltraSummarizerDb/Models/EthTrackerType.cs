using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class EthTrackerType
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }
    }
}
