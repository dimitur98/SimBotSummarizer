using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimBotSummarizer.Helpers.Extensions;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SimBotSummarizer.Helpers.Net.WebClients
{
    public abstract class JsonWebClient
    {
        protected WebHeaderCollection ResponseHeaders { get; set; }

        protected virtual T MakeRequest<T>(string url, string method = "GET", object data = null, NameValueCollection queryParams = null, WebHeaderCollection headers = null, bool readResponseOnException = false, int? timeout = null)
        {
            var response = this.MakeRequest(url, method: method, data: data, queryParams: queryParams, headers: headers, readResponseOnException: readResponseOnException, timeout: timeout);
            var responseJson = Encoding.UTF8.GetString(response);

            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        protected virtual (TResponse Result, TError Error) MakeRequest<TResponse, TError>(string url, string method = "GET", object data = null, NameValueCollection queryParams = null, WebHeaderCollection headers = null, int? timeout = null)
        {
            try
            {
                var response = this.MakeRequest(url, method: method, data: data, queryParams: queryParams, headers: headers, readResponseOnException: false, timeout: timeout);
                var responseJson = Encoding.UTF8.GetString(response);

                return (Result: JsonConvert.DeserializeObject<TResponse>(responseJson), Error: default);
            }
            catch (WebException ex)
            {
                if (ex.Response == null && typeof(TError) == typeof(string))
                {
                    return (Result: default, Error: (TError)System.Convert.ChangeType(ex.Message, typeof(TError)));
                }

                if (ex.Response == null) { throw; }

                using (var ms = new MemoryStream())
                {
                    ex.Response.GetResponseStream().CopyTo(ms);

                    var response = ms.ToArray();
                    var responseJson = Encoding.UTF8.GetString(response);

                    if (typeof(TError) == typeof(string))
                    {
                        return (Result: default, Error: (TError)System.Convert.ChangeType(responseJson, typeof(TError)));
                    }

                    return (Result: default, Error: JsonConvert.DeserializeObject<TError>(responseJson));
                }
            }
        }

        protected byte[] MakeRequest(string url, string method = "GET", object data = null, NameValueCollection queryParams = null, WebHeaderCollection headers = null, bool readResponseOnException = false, int? timeout = null)
        {
            using (var client = new SuperWebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.UseAutomaticDecompression = true;
                client.Timeout = timeout; //  10 * 60 * 1000; // 10min

                client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=UTF-8");
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");

                if (headers != null)
                {
                    foreach (var key in headers.AllKeys)
                    {
                        if (client.Headers[key] != null)
                        {
                            client.Headers[key] = headers[key];
                        }
                        else
                        {
                            client.Headers.Add(key, headers[key]);
                        }
                    }
                }

                if (queryParams != null)
                {
                    // encode the params
                    foreach (var key in queryParams.AllKeys)
                    {
                        queryParams[key] = queryParams[key].UrlEncode();
                    }

                    client.QueryString.Add(queryParams);
                }

                // make the request
                try
                {
                    var response = default(byte[]);

                    if (data != null || method?.ToUpper() == "POST" || method?.ToUpper() == "DELETE")
                    {
                        var postParams = data is string ? (string)data : JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                        response = client.UploadData(url, method, Encoding.UTF8.GetBytes(postParams));
                    }
                    else
                    {
                        response = client.DownloadData(url);
                    }

                    this.ResponseHeaders = client.ResponseHeaders;

                    return response;
                }
                catch (WebException ex)
                {
                    this.ResponseHeaders = ex.Response?.Headers;

                    if (!readResponseOnException || ex.Response == null) { throw; }

                    using (var ms = new MemoryStream())
                    {
                        ex.Response.GetResponseStream().CopyTo(ms);

                        return ms.ToArray();
                    }
                }
            }
        }
    }
}
