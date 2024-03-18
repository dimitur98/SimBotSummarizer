using System.Collections.Specialized;
using System.Text;

namespace SimBotSummarizer.Helpers.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static NameValueCollection AddOrSet(this NameValueCollection collection, string keyName, string value, bool ignoreCase = false)
        {
            if (!ignoreCase && collection.AllKeys.Contains(keyName)) { collection[keyName] = value; }
            //else if (ignoreCase && collection.AllKeys.Any(k=>k.ToLower() == keyName.ToLower())) { collection[keyName] = value; }
            else if (ignoreCase) { collection.RemoveIfExists(keyName, ignoreCase: ignoreCase).Add(keyName, value); }
            else { collection.Add(keyName, value); }

            return collection;
        }

        public static NameValueCollection RemoveIfExists(this NameValueCollection collection, string keyName, bool ignoreCase = false)
        {
            if (!ignoreCase && collection.AllKeys.Contains(keyName))
            {
                collection.Remove(keyName);
            }
            else if (ignoreCase && collection.AllKeys.Any(k => k?.ToLower() == keyName.ToLower()))
            {
                var keyNames = collection.AllKeys.Where(k => k?.ToLower() == keyName.ToLower()).ToArray();

                foreach (var key in keyNames)
                {
                    collection.Remove(key);
                }
            }

            return collection;
        }

        public static string ToQueryString(this NameValueCollection collection)
        {
            var parameters = new StringBuilder();

            if (collection != null)
            {
                foreach (var key in collection.AllKeys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        var values = collection.GetValues(key);

                        if (!values.HasItems()) { continue; }

                        foreach (var value in values)
                        {
                            parameters.Append(parameters.Length == 0 ? "?" : "&");

                            parameters.Append(key);
                            parameters.Append("=");
                            parameters.Append(value.UrlEncode());
                        }
                    }
                }
            }

            return parameters.ToString();
        }
    }
}
