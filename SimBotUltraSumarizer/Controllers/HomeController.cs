using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;
using SimBotUltraSummarizer.Helpers.Extensions;
using SimBotUltraSummarizer.ViewModels;
using System.Text;
using System.Text.RegularExpressions;
using SimBotSummarizer.Helpers.Extensions;

namespace SimBotUltraSummarizer.Controllers
{
    public class HomeController : Controller
    {
        protected readonly static Regex _cleanWhitespaceRegex = new("\\s+(?=(?:[^\\'\"]*[\\'\"][^\\'\"]*[\\'\"])*[^\\'\"]*$)"); // detect whitespaces

        public IActionResult Search()
        {
            return View(new HomeModel());
        }

        [HttpPost]
        public IActionResult Search(HomeModel model)
        {
            var b = "";
            try
            {
               var signalsDict = new Dictionary<string, Signal>();

                foreach (var file in model.Files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                        {
                            var htmlDocument = new HtmlDocument();

                            htmlDocument.Load(reader);

                            var signals = htmlDocument.QuerySelectorAll(".message.default.clearfix.joined");

                            foreach (var signal in signals)
                            {
                                var dateRegex = Regex.Match(signal.InnerHtml, @"<div class=""pull_right date details"" title=""(?'day'[0-9]{1,2}).(?'month'[0-9]{1,2}).(?'year'[0-9]{4}) (?'hour'[0-9]{1,2}):(?'minutes'[0-9]{1,2})");
                                DateTime? date = dateRegex.Success ? new DateTime(int.Parse(dateRegex.Groups["year"].Value), int.Parse(dateRegex.Groups["month"].Value), int.Parse(dateRegex.Groups["day"].Value), int.Parse(dateRegex.Groups["hour"].Value), int.Parse(dateRegex.Groups["minutes"].Value), 0) : null;

                                var text = _cleanWhitespaceRegex.Replace(signal.QuerySelector(".text").InnerText, string.Empty);
                                var address = Regex.Match(text, @"contractaddress:(?'contractaddress'[0-9a-zA-Z]{42})", RegexOptions.IgnoreCase).Groups["contractaddress"].Value;

                                if (string.IsNullOrEmpty(address)) { continue; }

                                var name = Regex.Match(text, @"(?'name'\S+)entry", RegexOptions.IgnoreCase).Groups["name"].Value;
                                var mcap = Regex.Match(text, @"marketcap:\$(?'mcap'[0-9,]+)", RegexOptions.IgnoreCase).Success ? long.Parse(Regex.Match(text, @"marketcap:\$(?'mcap'[0-9,]+)", RegexOptions.IgnoreCase).Groups["mcap"].Value.Replace(",", string.Empty)) : 0;
                                var totalCalls = Regex.Match(text, @"totalcalls:(?'totalcalls'[0-9]+)", RegexOptions.IgnoreCase).Success ? int.Parse(Regex.Match(text, @"totalcalls:(?'totalcalls'[0-9]+)", RegexOptions.IgnoreCase).Groups["totalcalls"].Value) : 1;
                                var dextoolsLink = Regex.Match(signal.InnerHtml, @"<a href=""(?'link'\S+)"">Dextools</a>").Groups["link"].Value;
                                if (address == "0x31e6506751437428ae8f2e15ec4a3d2e33d4f8a4")
                                {
                                    ;
                                }
                                if (signalsDict.ContainsKey(address))
                                {
                                    signalsDict[address].TotalCalls = signalsDict[address].TotalCalls < totalCalls ? totalCalls : signalsDict[address].TotalCalls;

                                    if (date.HasValue)
                                    {
                                        signalsDict[address].StartDate = signalsDict[address].StartDate > date.Value ? date.Value : signalsDict[address].StartDate;
                                        signalsDict[address].EndDate = signalsDict[address].EndDate < date.Value ? date.Value : signalsDict[address].EndDate;
                                    }

                                    signalsDict[address].MCapFirst = signalsDict[address].MCapFirst > mcap ? mcap : signalsDict[address].MCapFirst;
                                    signalsDict[address].MCapLast = signalsDict[address].MCapLast < mcap ? mcap : signalsDict[address].MCapLast;
                                    signalsDict[address].FirstCall = signalsDict[address].FirstCall > totalCalls ? totalCalls : signalsDict[address].FirstCall;
                                }
                                else
                                {
                                    signalsDict.Add(address, new Signal
                                    {
                                        Address = address,
                                        Name = name,
                                        StartDate = date,
                                        EndDate = date,
                                        Date = DateTime.Now.ToString(),
                                        TotalCalls = totalCalls,
                                        FirstCall = totalCalls,
                                        MCapFirst = mcap,
                                        MCapLast = mcap,
                                        DexToolsLink = dextoolsLink
                                    });
                                }

                                if (date.HasValue)
                                {
                                    if (!signalsDict[address].MCaps.HasItems()) { signalsDict[address].MCaps = new List<MCap>(); }

                                    signalsDict[address].MCaps.Add(new MCap { Date = date.Value, MCapValue = mcap, TotalCalls = totalCalls });
                                }
                            }
                        }
                    }
                }
                model.Signals = signalsDict;
            }
            catch (Exception e)
            {
                var test = b;

                e.SaveToLog();
            }

            return View(model);
        }
    }
}
