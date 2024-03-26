using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class IToken
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "scan")]
        public string Scan { get; set; }

        [Column(Name = "date")]
        public DateTime Date { get; set; }
    }
}
