using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class HypeSignal
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "mcap")]
        public decimal? MCap { get; set; }

        [Column(Name = "alarm_type")]
        public string AlarmType { get; set; }

        [Column(Name = "date")]
        public DateTime Date { get; set; }
    }
}
