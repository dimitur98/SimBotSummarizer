using System.Web;

namespace SimBotSummarizer.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }
    }
}
