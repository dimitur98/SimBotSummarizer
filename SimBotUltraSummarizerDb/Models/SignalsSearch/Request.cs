using SimBotUltraSummarizerDb.Models.Search;

namespace SimBotUltraSummarizerDb.Models.SignalsSearch
{
    public class Request : BaseRequest
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long? MCapFrom { get; set; }

        public long? MCapTo { get; set; }

        public int? TotalCallsFrom { get; set; }

        public int? TotalCallsTo { get; set; }
    }
}
