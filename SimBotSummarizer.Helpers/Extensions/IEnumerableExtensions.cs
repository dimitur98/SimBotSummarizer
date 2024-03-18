namespace SimBotSummarizer.Helpers.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool HasItems<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.Any() == true;
        }

        public static string ToUrlParams<T>(this IEnumerable<T> source, string paramName)
        {
            if (!source.HasItems()) { return ""; }

            return $"{paramName}=" + string.Join($"&{paramName}=", source.Select(i => i.ToString().UrlEncode()));
        }
    }
}
