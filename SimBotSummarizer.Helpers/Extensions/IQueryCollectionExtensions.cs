using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;

namespace SimBotSummarizer.Helpers.Extensions
{
    public static class IQueryCollectionExtensions
    {
        public static NameValueCollection ToNameValueCollection(this IQueryCollection query, bool addEmptyValues = false)
        {
            var col = new NameValueCollection();

            foreach (var key in query.Keys)
            {
                var values = query[key];

                if (values.Count == 1)
                {
                    if (!addEmptyValues && values == string.Empty) { continue; }

                    col.Add(key, query[key]);

                    continue;
                }

                for (int i = 0; i < values.Count; i++)
                {
                    col.Add($"{key}[{i}]", values[i]);
                }
            }

            return col;
        }
    }
}
