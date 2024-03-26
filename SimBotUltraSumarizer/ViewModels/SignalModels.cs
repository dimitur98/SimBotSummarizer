using SimBotUltraSummarizerDb.Models.SignalsSearch;
using System.ComponentModel;


namespace SimBotUltraSummarizer.ViewModels
{
    public class SignalsSearchFormModel : BaseSearchFormModel
    {
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("MCap From")]
        public long? MCapFrom { get; set; }

        [DisplayName("MCap To")]
        public long? MCapTo { get; set; }

        [DisplayName("Total Calls From")]
        public int? TotalCallsFrom { get; set; }

        [DisplayName("Total Calls To")]
        public int? TotalCallsTo { get; set; }

        [DisplayName("Has Hype Alarm")]
        public bool? HasHypeAlarmSignal { get; set; }

        [DisplayName("Has IToken")]
        public bool? HasITokenSignal { get; set; }

        public Request ToSearchRequest()
        {
            var request = new Request 
            {
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                MCapFrom = this.MCapFrom,
                MCapTo = this.MCapTo,
                TotalCallsFrom = this.TotalCallsFrom,
                TotalCallsTo = this.TotalCallsTo,
                HasHypeAlarmSignal = this.HasHypeAlarmSignal,
                HasITokenSignal = this.HasITokenSignal
            };

            this.SetSearchRequest(request);

            return request;
        }
    }

    public class SignalsSearchModel : BaseSearchModel<SignalsSearchFormModel, Response, SimBotUltraSummarizerDb.Models.Signal>
    {
        public SignalsSearchModel(SignalsSearchFormModel searchForm) : base(searchForm) { }
    }
}
