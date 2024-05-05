using Microsoft.AspNetCore.Mvc;
using SimBotUltraSummarizer.ViewModels;
using SimBotUltraSummarizerDb.Dal;

namespace SimBotUltraSummarizer.Controllers
{
    public class SignalsController : Controller
    {
        public IActionResult Search(SignalsSearchFormModel searchForm)
        {
            var model = this.ExecuteSearch(searchForm);

            return this.View(model);
        }


        protected SignalsSearchModel ExecuteSearch(SignalsSearchFormModel searchForm)
        {
            searchForm.SetDefaultSort("s.edited_at", sortDesc: true);
            searchForm.EthTrackerWallets = EthTrackerWallets.GetAll();
            searchForm.Filters = Filters.GetAll();

            var response = Signals.Search(searchForm.ToSearchRequest());

            Signals.LoadSignalData(response.Records);
            Signals.LoadHypeSignals(response.Records);
            Signals.LoadITokenSignals(response.Records);
            Signals.LoadEthTrackerSignals(response.Records);
            EthTrackerSignals.LoadWallets(response.Records.SelectMany(r => r.EthTrackerSignals));
            EthTrackerWallets.LoadTypes(response.Records.SelectMany(r => r.EthTrackerSignals).Select(x => x.EthTrackerWallet));

            var model = new SignalsSearchModel(searchForm)
            {
                Response = response
            };

            return model;
        }
    }
}
