using System.Net;
using System.Net.Security;

namespace SimBotSummarizer.Helpers.Net
{
    public class SuperWebClient : WebClient
    {
        /// <summary>
        /// Gets or sets the time-out value in milliseconds for the System.Net.HttpWebRequest.GetResponse()
        /// and System.Net.HttpWebRequest.GetRequestStream() methods.
        /// </summary>        
        public int? Timeout { get; set; }

        /// <summary>
        /// Should it use decompression
        /// </summary>
        public bool UseAutomaticDecompression { get; set; } = true;


        /// <summary>
        /// Should it auto redirect
        /// </summary>
        public bool? AllowAutoRedirect { get; set; }

        public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

        Uri _responseUri;

        public Uri ResponseUri
        {
            get { return _responseUri; }
        }

        public void UseTls12()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // (SecurityProtocolType)3072; //TLS 1.2; SecurityProtocolType.Tls12 is missing in .NET 4
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);

            _responseUri = response.ResponseUri;

            return response;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);

            if (this.UseAutomaticDecompression)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            if (this.Timeout.HasValue)
            {
                request.Timeout = this.Timeout.Value;
            }

            if (this.AllowAutoRedirect.HasValue)
            {
                request.AllowAutoRedirect = this.AllowAutoRedirect.Value;
            }

            if (this.ServerCertificateValidationCallback != default)
            {
                request.ServerCertificateValidationCallback = this.ServerCertificateValidationCallback;
            }

            return request;
        }

        /// <summary>
        /// The URI to which <paramref name="address"/> redirects
        /// </summary>
        /// <param name="address">The URI from which to check for redirect.</param>
        /// <returns></returns>
        public static string GetRedirectLocation(string address)
        {
            using (var client = new SuperWebClient())
            {
                client.AllowAutoRedirect = false;

                var response = client.DownloadData(address);

                if (client.ResponseHeaders?.AllKeys.Contains("Location") == true)
                {
                    return client.ResponseHeaders["Location"];
                }
            }

            return null;
        }
    }
}