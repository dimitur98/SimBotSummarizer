using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public IActionResult GetSignals()
        {
            var lastDateSimBot = SignalDatas.GetLastDate();
            var lastDateHypeBot = HypeSignals.GetLastDate();
            var lastDateIToken = ITokens.GetLastDate();

            var signals = _simBotTelegramApi.GetMessages(lastDateSimBot, lastDateHypeBot, lastDateIToken);

            this.InsertSimBotSignals(signals.SimBotMessasges);
            this.InsertHypeSignals(signals.HypeMessages);
            this.InsertITokenSignals(signals.ITokenMessages);

            return this.Ok(new {});
        }

        public IActionResult GetSimBotSignals()
        {
            var lastDate = SignalDatas.GetLastDate();

            var response = _simBotTelegramApi.GetSimBotMessages(lastDate);

            this.InsertSimBotSignals(response);

            return this.Ok();
        }

        public IActionResult GetHypeSignals()
        {
            var lastDate = HypeSignals.GetLastDate();

            var response = _simBotTelegramApi.GetHypeMessages(lastDate);

            this.InsertHypeSignals(response);

            return this.Ok();
        }

        public IActionResult GetITokenSignals()
        {
            var lastDate = ITokens.GetLastDate();

            var response = _simBotTelegramApi.GetITokenMessages(lastDate);

            this.InsertITokenSignals(response);

            return this.Ok();
        }

        public IActionResult LoadSimBotSignals()
        {
            var signals = this.LoadJson<List<SimBotTelegram.Api.Ver1.Models.SimBotMessasge>>("response.json");

            this.InsertSimBotSignals(signals);

            return this.Ok(new { });
        }

        public IActionResult LoadHypeSignals()
        {
            var hypeSignals = this.LoadJson<List<SimBotTelegram.Api.Ver1.Models.HypeMessage>>("response_hype.json");

            this.InsertHypeSignals(hypeSignals);

            return this.Ok(new { });
        }

        public IActionResult LoadITokenSignals()
        {
            var itokenSignals = this.LoadJson<List<SimBotTelegram.Api.Ver1.Models.ITokenMessage>>("response_itoken.json");

            this.InsertITokenSignals(itokenSignals);

            return this.Ok(new { });
        }

        private void InsertSimBotSignals(List<SimBotTelegram.Api.Ver1.Models.SimBotMessasge> response)
        {
            if (!response.HasItems()) { return; }

            var addresses = response.Select(s => s.Address);
            var signalsDict = Signals.GetByAddress(addresses).ToDictionary(x => x.Address, x => x.Id);

            foreach (var item in response)
            {
                if (!signalsDict.ContainsKey(item.Address.Trim()))
                {
                    var signal = new Signal
                    {
                        Name = item.Name,
                        Address = item.Address,
                        Pair = item.Pair,
                    };

                    Signals.Insert(signal);

                    signalsDict.Add(signal.Address, signal.Id);
                }

                if (item.SignalData.HasItems())
                {
                    foreach (var item2 in item.SignalData)
                    {
                        var signalData = new SignalData
                        {
                            SignalId = signalsDict[item.Address],
                            Address = item.Address,
                            MCap = item2.MCap,
                            TotalCalls = item2.TotalCalls,
                            SellTax = item2.SellTax,
                            BuyTax = item2.BuyTax,
                            Price = item2.Price,
                            Date = item2.Date
                        };

                        SignalDatas.Insert(signalData);
                    }
                }
            }
        }

        private void InsertHypeSignals(List<SimBotTelegram.Api.Ver1.Models.HypeMessage> response)
        {
            if (!response.HasItems()) { return; }

            foreach (var item in response)
            {
                var hypeSingal = new HypeSignal
                {
                    Address = item.Address,
                    MCap = item.MCap,
                    AlarmType = item.AlarmType,
                    Date = item.Date
                };

                HypeSignals.Insert(hypeSingal);
            }
        }

        private void InsertITokenSignals(List<SimBotTelegram.Api.Ver1.Models.ITokenMessage> response)
        {
            if (!response.HasItems()) { return; }

            foreach (var item in response)
            {
                var itokenMessage = new IToken
                {
                    Address = item.Address,
                    Scan = item.Scan,
                    Date = item.Date
                };

                ITokens.Insert(itokenMessage);
            }
        }

        private T LoadJson<T>(string fileName)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;

            var fullPath = Path.Combine(rootPath, fileName);

            var jsonData = System.IO.File.ReadAllText(fullPath);

            if (string.IsNullOrWhiteSpace(jsonData)) { return default; }

            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
