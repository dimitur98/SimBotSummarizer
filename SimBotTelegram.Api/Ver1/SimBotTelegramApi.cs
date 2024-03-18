using SimBotSummarizer.Helpers.Net.WebClients;
using SimBotTelegram.Api.Ver1.Models;

namespace SimBotTelegram.Api.Ver1
{
    public class SimBotTelegramApi : JsonWebClient
    {
        const string URL = "http://127.0.0.1:5051/v1/";

        public List<Signal> GetSignals()
        {
            var response = this.MakeRequest<List<Signal>>(URL+"get_messages", method: HttpMethod.Post.Method,timeout: int.MaxValue);

            return response;
        }

        public List<HypeSignal> GetHypeSignals()
        {
            var response = this.MakeRequest<List<HypeSignal>>(URL + "get_hype_messages", method: HttpMethod.Post.Method, timeout: int.MaxValue);

            return response;
        }
    }
}
