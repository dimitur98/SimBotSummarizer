using Microsoft.AspNetCore.Mvc;
using SimBotUltraSummarizer.ViewModels;
using SimBotUltraSummarizerDb.Dal;

namespace SimBotUltraSummarizer.Controllers
{
    public class EthTrackerWalletsController : Controller
    {
        public IActionResult Search(EthTrackerWalletsSearchFormModel searchForm)
        {
            var model = this.ExecuteSearch(searchForm);

            return this.View(model);
        }

        protected EthTrackerWalletsSearchModel ExecuteSearch(EthTrackerWalletsSearchFormModel searchForm)
        {
            searchForm.SetDefaultSort("etw.id", sortDesc: true);
            searchForm.EthTrackerTypes = EthTrackerTypes.GetAll();

            var response = EthTrackerWallets.Search(searchForm.ToSearchRequest());

            EthTrackerWallets.LoadTypes(response.Records);

            var model = new EthTrackerWalletsSearchModel(searchForm)
            {
                Response = response,
                EthTrackerWalletSignalsCountDict = EthTrackerSignals.GetWalletSignalsCount(response.Records.Select(r => r.Address))
            };

            return model;
        }
    }
}
