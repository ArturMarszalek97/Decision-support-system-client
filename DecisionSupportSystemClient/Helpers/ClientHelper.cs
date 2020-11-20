using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DecisionSupportSystemClient.Helpers
{
    public static class ClientHelper
    {
        private static HttpClient client;

        public static void InitHttpClient()
        {
            client = new HttpClient();

            // trust any certificate
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClient GetClient()
        {
            return client;
        }
    }
}
