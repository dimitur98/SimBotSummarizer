using SimBotUltraSummarizerDb.Models;
using SimBotUltraSummarizerDb.Models.EthTrackerWalletsSearch;
using System.ComponentModel;

namespace SimBotUltraSummarizer.ViewModels
{
    public class EthTrackerWalletsSearchFormModel : BaseSearchFormModel
    {
        public uint? Id { get; set; }

        [DisplayName("Telegram Profile")]
        public uint? EthTrackerTypeId { get; set; }
        public List<EthTrackerType> EthTrackerTypes { get; set; }

        public Request ToSearchRequest()
        {
            var request = new Request
            {
                Id = this.Id,
                EthTrackerTypeId = EthTrackerTypeId,
            };

            this.SetSearchRequest(request);

            return request;
        }
    }

    public class EthTrackerWalletsSearchModel : BaseSearchModel<EthTrackerWalletsSearchFormModel, Response, SimBotUltraSummarizerDb.Models.EthTrackerWallet>
    {
        public Dictionary<uint, int> EthTrackerWalletSignalsCountDict { get; set; }

        public EthTrackerWalletsSearchModel(EthTrackerWalletsSearchFormModel searchForm) : base(searchForm) { }
    }
}
