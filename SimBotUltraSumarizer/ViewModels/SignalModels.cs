using SimBotUltraSummarizerDb.Models;
using SimBotUltraSummarizerDb.Models.SignalsSearch;
using System.ComponentModel;


namespace SimBotUltraSummarizer.ViewModels
{
    public class SignalsSearchFormModel : BaseSearchFormModel
    {
        [DisplayName("Wallet")]
        public uint? EthTrackerWalletId { get; set; }
        public List<EthTrackerWallet> EthTrackerWallets { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

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

        [DisplayName("Has Eth Tracker Signal")]
        public bool? HasEthTrackerSignal { get; set; }

        [DisplayName("Scam")]
        public bool? IsScam { get; set; }

        [DisplayName("User")]
        public uint? UserId { get; set; }
        public List<Filter> Filters { get; set; }

        public Request ToSearchRequest()
        {
            var request = new Request 
            {
                EthTrackerWalletId = this.EthTrackerWalletId,
                Address = this.Address,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                MCapFrom = this.MCapFrom,
                MCapTo = this.MCapTo,
                TotalCallsFrom = this.TotalCallsFrom,
                TotalCallsTo = this.TotalCallsTo,
                HasHypeAlarmSignal = this.HasHypeAlarmSignal,
                HasITokenSignal = this.HasITokenSignal,
                HasEthTrackerSignal = this.HasEthTrackerSignal,
                IsScam = this.IsScam,
                UserId = this.UserId
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
