using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class Filter
    {
        [Column(Name = "user_id")]
        public long UserId { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }
    }
}
