using DapperMySqlMapper;

namespace SimBotUltraSummarizerDb.Models
{
    public class Signal
    {
        [Column(Name = "id")]
        public uint Id { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "pair")]
        public string Pair { get; set; }

        public List<SignalData> SignalData { get; set; }

        public List<HypeSignal> HypeSignals { get; set; }

        public List<IToken> ITokens { get; set; }
    }
}
