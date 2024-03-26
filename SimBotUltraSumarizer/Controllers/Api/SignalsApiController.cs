using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimBotSummarizer.Helpers.Extensions;
using SimBotTelegram.Api.Ver1;
using SimBotUltraSummarizerDb.Dal;
using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizer.Controllers.Api
{
    [ApiController]
    [Route("/api/Signals/[action]")]
    public class SignalsApiController : ControllerBase
    {
        private readonly SimBotTelegramApi _simBotTelegramApi;
        private readonly Microsoft.Extensions.Hosting.IHostingEnvironment _hostingEnvironment;
        public SignalsApiController(SimBotTelegramApi simBotTelegramApi, Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this._simBotTelegramApi = simBotTelegramApi;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult GetSignals()
        {
            var response = _simBotTelegramApi.GetSignals();

            foreach (var item in response)
            {
                var signal = new Signal
                {
                    Name = item.Name,
                    Address = item.Address,
                    Pair = item.Pair,
                };

                Signals.Insert(signal);

                if (item.SignalData.HasItems())
                {
                    foreach (var item2 in item.SignalData)
                    {
                        var signalData = new SignalData
                        {
                            Address = item.Address,
                            MCap = item2.MCap,
                            TotalCalls = item2.TotalCalls,
                            SellTax = item2.SellTax,
                            BuyTax = item2.BuyTax,
                            Date = item2.Date
                        };

                        SignalDatas.Insert(signalData);
                    }
                }
            }

            return this.Ok();
        }

        public IActionResult LoadSignals()
        {
            var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

            var fullPath = Path.Combine(rootPath, "response.json"); //combine the root path with that of our json file inside mydata directory

            var jsonData = System.IO.File.ReadAllText(fullPath); //read all the content inside the file

            if (string.IsNullOrWhiteSpace(jsonData)) return null; //if no data is present then return null or error if you wish

            var signals = JsonConvert.DeserializeObject<List<SimBotTelegram.Api.Ver1.Models.Signal>>(jsonData);

            foreach (var item in signals)
            {
                var signal = new Signal
                {
                    Name = item.Name,
                    Address = item.Address,
                    Pair = item.Pair,
                };

                Signals.Insert(signal);

                if (item.SignalData.HasItems())
                {
                    foreach (var item2 in item.SignalData)
                    {
                        var signalData = new SignalData
                        {
                            Address = item.Address,
                            MCap = item2.MCap,
                            TotalCalls = item2.TotalCalls,
                            SellTax = item2.SellTax,
                            BuyTax = item2.BuyTax,
                            Date = item2.Date
                        };

                        SignalDatas.Insert(signalData);
                    }
                }
            }
            return this.Ok(new { });
        }

        public IActionResult LoadHypeSignals()
        {
            var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

            var fullPath = Path.Combine(rootPath, "response_hype.json"); //combine the root path with that of our json file inside mydata directory

            var jsonData = System.IO.File.ReadAllText(fullPath); //read all the content inside the file

            if (string.IsNullOrWhiteSpace(jsonData)) return null; //if no data is present then return null or error if you wish

            var hypeSignals = JsonConvert.DeserializeObject<List<SimBotTelegram.Api.Ver1.Models.HypeSignal>>(jsonData);

            foreach (var item in hypeSignals)
            {
                var hypeSignal = new HypeSignal
                {
                    Address = item.Address,
                    MCap = item.MCap,
                    AlarmType = item.AlarmType,
                    Date = item.Date,
                };

                HypeSignals.Insert(hypeSignal);
            }

            return this.Ok(new {});
        }

        public IActionResult LoadITokenSignals()
        {
            var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

            var fullPath = Path.Combine(rootPath, "response_itoken.json"); //combine the root path with that of our json file inside mydata directory

            var jsonData = System.IO.File.ReadAllText(fullPath); //read all the content inside the file

            if (string.IsNullOrWhiteSpace(jsonData)) return null; //if no data is present then return null or error if you wish

            var itokens = JsonConvert.DeserializeObject<List<SimBotTelegram.Api.Ver1.Models.IToken>>(jsonData);

            foreach (var item in itokens)
            {
                var itoken = new IToken
                {
                    Address = item.Address,
                    Scan = item.Scan,
                    Date = item.Date
                };

                ITokens.Insert(itoken);
            }

            return this.Ok(new { });
        }
    }
}
