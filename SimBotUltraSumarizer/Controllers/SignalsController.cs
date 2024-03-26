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
            searchForm.SetDefaultSort("s.id", sortDesc: true);

            var response = Signals.Search(searchForm.ToSearchRequest());

            Signals.LoadSignalData(response.Records);
            Signals.LoadHypeSignals(response.Records);
            Signals.LoadITokenSignals(response.Records);

            var model = new SignalsSearchModel(searchForm)
            {
                Response = response
            };

            return model;
        }
    }
}
