using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Services.Implementation
{
    internal sealed class HttpService
    {
        public async Task<T> SendApiGetRequestAsync<T>(string uri, Dictionary<string,string> headers=null) where T : new()
        {
            HttpClient httpClient = new HttpClient();

            AddHeaders(ref httpClient, headers);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);

            string content = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.IsSuccessStatusCode)
            {
                T deserializedResult = JsonConvert.DeserializeObject<T>(content);

                return deserializedResult;
            }
            else
            {
                throw new Exception($"{nameof(SendApiGetRequestAsync)}: {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
            }
        }

        public async Task<HttpResponseMessage> SendApiPostRequestAsync(string uri, string body,Dictionary<string,string> headers=null)
        {
            HttpClient httpClient = new HttpClient();
            
            AddHeaders(ref httpClient, headers);

            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await httpClient.PostAsync(uri, httpContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception($"{nameof(SendApiPostRequestAsync)}: {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
            }

            return httpResponse;
        }

        public async Task<HttpResponseMessage> SendApiDeleteRequestAsync(string uri, Guid messageId,Dictionary<string,string> headers)
        {
            HttpClient httpClient = new HttpClient();

            AddHeaders(ref httpClient, headers);

            string deleteUri = String.Join("/", new string[] { uri, messageId.ToString() });

            HttpResponseMessage httpResponse = await httpClient.DeleteAsync(deleteUri);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception($"{nameof(SendApiDeleteRequestAsync)}: {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
            }

            return httpResponse;
        }

        public async Task<HttpResponseMessage> SendApiPutRequestAsync(string uri, string body, Dictionary<string,string> headers)
        {
            HttpClient httpClient = new HttpClient();
            AddHeaders(ref httpClient, headers);

            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await httpClient.PutAsync(uri, httpContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception($"{nameof(SendApiPutRequestAsync)}: {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
            }

            return httpResponse;
        }

        private void AddHeaders(ref HttpClient httpClient,Dictionary<string,string> headers=null)
        {
            if (headers?.Count > 0)
            {
                foreach (var kvp in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
