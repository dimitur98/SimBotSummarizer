﻿using SimBotSummarizer.Helpers.Net.WebClients;
using SimBotTelegram.Api.Ver1.Models;
using System.Collections.Specialized;

namespace SimBotTelegram.Api.Ver1
{
    public class SimBotTelegramApi : JsonWebClient
    {
        const string URL = "http://127.0.0.2:5051/v1/";

        public Messages GetMessages(DateTime lastDateSimBot, DateTime lastDateHypeBot, DateTime lastDateIToken, DateTime lastDateEthTracker1, DateTime lastDateEthTracker2)
        {
            var queryParams = new NameValueCollection {
                { "last_date_sim_bot", lastDateSimBot.ToString() },
                { "last_date_hype_bot", lastDateHypeBot.ToString() },
                { "last_date_itoken_bot", lastDateIToken.ToString() },
                { "last_date_eth_traker_bot_1", lastDateEthTracker1.ToString() },
                { "last_date_eth_traker_bot_2", lastDateEthTracker2.ToString() }
            };

            var response = this.MakeRequest<Messages>(URL + "get_messages", queryParams: queryParams, method: HttpMethod.Post.Method, timeout: int.MaxValue);

            return response;
        }

        public List<SimBotMessasge> GetSimBotMessages(DateTime lastDate)
        {
            var queryParams = new NameValueCollection {
                { "date", lastDate.ToString() }
            };

            var response = this.MakeRequest<List<SimBotMessasge>>(URL+"get_sim_bot_messages", queryParams: queryParams, method: HttpMethod.Post.Method,timeout: int.MaxValue);

            return response;
        }

        public List<HypeMessage> GetHypeMessages(DateTime lastDate)
        {
            var queryParams = new NameValueCollection {
                { "date", lastDate.ToString() }
            };

            var response = this.MakeRequest<List<HypeMessage>>(URL + "get_hype_messages", queryParams: queryParams, method: HttpMethod.Post.Method, timeout: int.MaxValue);

            return response;
        }

        public List<ITokenMessage> GetITokenMessages(DateTime lastDate)
        {
            var queryParams = new NameValueCollection {
                { "date", lastDate.ToString() }
            };

            var response = this.MakeRequest<List<ITokenMessage>>(URL + "get_itoken_messages", queryParams: queryParams, method: HttpMethod.Post.Method, timeout: int.MaxValue);

            return response;
        }
    }
}
