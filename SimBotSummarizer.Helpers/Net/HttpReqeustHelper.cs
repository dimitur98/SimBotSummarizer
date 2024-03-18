using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SimBotSummarizer.Helpers.Net
{
    public static class HttpWebResponseExtensions
    {
        public static string GetContent(this HttpWebResponse response, Encoding encoding)
        {
            var responseStream = new StreamReader(response.GetResponseStream(), encoding);
            var content = responseStream.ReadToEnd();

            responseStream.Close();

            return content;
        }
    }

    public static class HttpRequestHelper
    {
        public static string GetPageHTML(string pageUrl)
        {
            return GetPageHTML(pageUrl, null, CredentialCache.DefaultCredentials);
        }

        public static string GetPageHTML(string pageUrl, int requestTimeout)
        {
            return GetPageHTML(pageUrl, null, CredentialCache.DefaultCredentials, requestTimeout);
        }

        public static string GetPageHTML(string pageUrl, ICredentials credentials)
        {
            return GetPageHTML(pageUrl, null, credentials);
        }

        public static string GetPageHTML(string pageUrl, NameValueCollection postParams, ICredentials credentials)
        {
            return GetPageHTML(pageUrl, postParams, credentials, 10000); // 10 secs
        }

        public static string GetPageHTML(string pageUrl, NameValueCollection postParams, int requestTimeout)
        {
            return GetPageHTML(pageUrl, postParams, CredentialCache.DefaultCredentials, requestTimeout);
        }

        public static string GetPageHTML(string pageUrl, NameValueCollection postParams, ICredentials credentials, int requestTimeout)
        {
            var httpWebResponse = HttpRequestHelper.GetHttpWebResponse(pageUrl, postParams, credentials, requestTimeout);
            var html = httpWebResponse.GetContent(Encoding.UTF8);

            httpWebResponse.Close();

            return html;
        }

        public static HttpWebResponse GetHttpWebResponse(string pageUrl, NameValueCollection postParams, int requestTimeout)
        {
            return GetHttpWebResponse(pageUrl, postParams, CredentialCache.DefaultCredentials, requestTimeout);
        }

        public static HttpWebResponse GetHttpWebResponse(string pageUrl, NameValueCollection postParams, ICredentials credentials, int requestTimeout)
        {
            return GetHttpWebResponse(pageUrl, postParams, credentials, requestTimeout, null);
        }

        public static HttpWebResponse GetHttpWebResponse(string pageUrl, NameValueCollection postParams, int requestTimeout, CookieContainer cookieContainer)
        {
            return GetHttpWebResponse(pageUrl, postParams, CredentialCache.DefaultCredentials, requestTimeout, cookieContainer);
        }

        public static HttpWebResponse GetHttpWebResponse(string pageUrl, NameValueCollection postParams, ICredentials credentials, int requestTimeout, CookieContainer cookieContainer)
        {
            HttpWebRequest httpWebRequest = CreateHttpWebRequest(pageUrl, postParams, requestTimeout, credentials, cookieContainer, null);

            // get the response
            return (HttpWebResponse)httpWebRequest.GetResponse();
        }

        public static HttpWebRequest CreateHttpWebRequest(
            string pageUrl,
            NameValueCollection postParams,
            int requestTimeout,
            ICredentials credentials,
            CookieContainer cookieContainer,
            string referer)
        {
            //Establish the request
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(pageUrl);

            //authenticate (example: against an NTLM scheme (windows authentication))
            if (credentials != null) { httpWebRequest.Credentials = credentials; } // CredentialCache.DefaultCredentials;        

            //set cookies
            if (cookieContainer != null) { httpWebRequest.CookieContainer = cookieContainer; }
            if (referer != null) { httpWebRequest.Referer = referer; }

            httpWebRequest.Timeout = requestTimeout;
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; %username%; %username%)";

            //add post data if needed
            if (postParams != null && postParams.Count > 0)
            {
                StringBuilder parameters = new StringBuilder();
                for (int i = 0; i < postParams.Count; i++)
                {
                    EncodeAndAddParam(ref parameters, postParams.GetKey(i), postParams[i]);
                }

                string postData = parameters.ToString();

                httpWebRequest.Method = "post";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = postData.Length;

                using (var writeStream = httpWebRequest.GetRequestStream())
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(postData);
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            return httpWebRequest;
        }

        /// <summary>
        /// Encodes an item and ads it to the string.
        /// </summary>
        /// <param name="baseRequest">The previously encoded data.</param>
        /// <param name="dataItem">The data to encode.</param>
        /// <returns>A string containing the old data and the previously encoded data.</returns>
        public static void EncodeAndAddParam(ref StringBuilder baseRequest, string key, string dataParam)
        {
            if (baseRequest == null)
            {
                baseRequest = new StringBuilder();
            }
            if (baseRequest.Length != 0)
            {
                baseRequest.Append("&");
            }
            baseRequest.Append(key);
            baseRequest.Append("=");
            baseRequest.Append(System.Web.HttpUtility.UrlEncode(dataParam));
        }

        public static string BuildUrl(string baseUrl, string key, object value)
        {
            return BuildUrl(baseUrl, new string[] { key }, new string[] { value.ToString() });
        }

        public static string BuildUrl(string baseUrl, string[] keys, string[] values)
        {
            NameValueCollection getParams = new NameValueCollection();

            for (int i = 0; i < keys.Length; i++)
            {
                getParams.Add(keys[i], values[i]);
            }

            return BuildUrl(baseUrl, getParams);
        }

        public static string BuildUrl(string baseUrl, NameValueCollection getParams)
        {
            string getData = String.Empty;

            if (getParams != null && getParams.Count > 0)
            {
                StringBuilder parameters = new StringBuilder();
                for (int i = 0; i < getParams.Count; i++)
                {
                    EncodeAndAddParam(ref parameters, getParams.GetKey(i), getParams[i]);
                }

                getData = parameters.ToString();
            }

            string separator = (!String.IsNullOrEmpty(baseUrl) && baseUrl.IndexOf("?") <= -1 ? "?" : "&");

            return baseUrl + (!String.IsNullOrEmpty(getData) ? separator + getData : String.Empty);
        }

        public static void UploadImage(string imageUrl, string filePath, ICredentials credentials)
        {
            using (var webClient = new WebClient())
            {
                webClient.Credentials = credentials;

                webClient.DownloadFile(imageUrl, filePath);
            }
        }
    }
}
