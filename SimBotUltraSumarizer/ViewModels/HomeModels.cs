using SimBotUltraSummarizer.ViewModels;
using SimBotUltraSummarizerDb.Models.SignalsSearch;

namespace SimBotUltraSummarizer.ViewModels
{
    public class HomeModel
    {
        public IEnumerable<IFormFile> Files { get; set; }

        public Dictionary<string, Signal> Signals { get; set; }
    }

    public class Signal
    {
        public string Address { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string Date { get; set; }
        public int TotalCalls { get; set; }

        public int FirstCall { get; set; }

        public long MCapFirst { get; set; }

        public long MCapLast { get; set; }

        public string DexToolsLink { get; set; }

        public List<MCap> MCaps { get; set; }
    }

    public class MCap
    {
        public DateTime Date { get; set; }

        public long MCapValue { get; set; }

        public int TotalCalls { get; set ; }
    }

    
}
