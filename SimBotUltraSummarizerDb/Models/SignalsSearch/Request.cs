using SimBotUltraSummarizerDb.Models.Search;

namespace SimBotUltraSummarizerDb.Models.SignalsSearch
{
    public class Request : BaseRequest
    {
        public uint? EthTrackerWalletId { get; set; }

        public string Address { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long? MCapFrom { get; set; }

        public long? MCapTo { get; set; }

        public int? TotalCallsFrom { get; set; }

        public int? TotalCallsTo { get; set; }

        public bool? HasHypeAlarmSignal { get; set; }

        public bool? HasITokenSignal { get; set; }

        public bool? HasEthTrackerSignal { get; set; }

        public bool? IsScam { get; set; }

        public uint? UserId { get; set; }
    }
}
