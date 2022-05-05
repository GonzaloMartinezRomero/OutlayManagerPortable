using Newtonsoft.Json;
using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Services.Implementation
{
    internal class TransactionAzureService : ITransactionService
    {
        private readonly List<TransactionType> transactionTypesCached= new List<TransactionType>();
        private readonly List<TransactionCode> transactionCodesCached = new List<TransactionCode>();
        private readonly List<TransactionMessage> transactionMessagesCached = new List<TransactionMessage>();

        public async Task<List<TransactionType>> TransactionTypes()
        {
            if (transactionTypesCached.Count == 0)
            {
                const string transactionTypeURI = "https://outlaymanagerportableapi.azurewebsites.net/DataMaster/TransactionType";

                List<TransactionType> response = await SendApiGetRequestAsync<List<TransactionType>>(transactionTypeURI);

                transactionTypesCached.AddRange(response);
            }

            return transactionTypesCached;
        }

        public async Task<List<TransactionCode>> TransactionCodes()
        {
            if(transactionCodesCached.Count == 0)
            {
                const string transactionCodeURI = "https://outlaymanagerportableapi.azurewebsites.net/DataMaster/TransactionCode";

                List<TransactionCode> response = await SendApiGetRequestAsync<List<TransactionCode>>(transactionCodeURI);

                transactionCodesCached.AddRange(response);
            }

            return transactionCodesCached;
        }

        public async Task<List<TransactionMessage>> TransactionsQueued()
        {
            const string transactionCodeURI = "https://outlaymanagerportableapi.azurewebsites.net/TransactionOutlay";

            if (transactionMessagesCached.Count == 0)
            { 
                List<TransactionMessage> response = await SendApiGetRequestAsync<List<TransactionMessage>>(transactionCodeURI);

                foreach (TransactionMessage transactionMessageAux in response)
                {
                    if (TransactionMessageIsValid(transactionMessageAux))
                        transactionMessagesCached.Add(transactionMessageAux);
                }
            }

            return transactionMessagesCached;
        }

        public async Task SaveTransaction(TransactionMessage transactionMessage)
        {
            const string URI = "https://outlaymanagerportableapi.azurewebsites.net/TransactionOutlay";

            string bodyContent = JsonConvert.SerializeObject(transactionMessage);

            if (transactionMessage.Id == Guid.Empty)
                _ = await SendApiPostRequestAsync(URI, bodyContent);
            else
                _ = await SendApiPutRequestAsync(URI, bodyContent);

            ClearTransactionsCache();
        }

        public async Task DeleteTransaction(Guid transactionMessageId)
        {
            if (transactionMessageId == Guid.Empty)
                throw new NullReferenceException($"{nameof(DeleteTransaction)}: {nameof(transactionMessageId)} is null or empty");

            const string URI = "https://outlaymanagerportableapi.azurewebsites.net/TransactionOutlay";

            _ = await SendApiDeleteRequestAsync(URI, transactionMessageId);
                        
            ClearTransactionsCache();
        }

        private bool TransactionMessageIsValid(TransactionMessage transactionMessageAux)
        {
            if (transactionMessageAux.Id == Guid.Empty)
                return false;
            
            if (transactionMessageAux.Date == default(DateTime))
                return false;

            if (transactionMessageAux.CodeID <= 0)
                return false;

            if (transactionMessageAux.TypeID <= 0)
                return false;

            return true;
        }

        private async Task<T> SendApiGetRequestAsync<T>(string uri) where T:new ()
        {
            HttpClient httpClient = new HttpClient();

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

        private async Task<HttpResponseMessage> SendApiPostRequestAsync(string uri, string body)
        {
            HttpClient httpClient = new HttpClient();

            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await httpClient.PostAsync(uri, httpContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception($"{nameof(SendApiPostRequestAsync)}: {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
            }

            return httpResponse;
        }

        private async Task<HttpResponseMessage> SendApiDeleteRequestAsync(string uri, Guid messageId)
        {
            HttpClient httpClient = new HttpClient();

            string deleteUri = String.Join("/", new string[]{ uri, messageId.ToString()});

            HttpResponseMessage httpResponse = await httpClient.DeleteAsync(deleteUri);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception($"{nameof(SendApiDeleteRequestAsync)}: {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
            }

            return httpResponse;
        }

        private async Task<HttpResponseMessage> SendApiPutRequestAsync(string uri, string body)
        {
            HttpClient httpClient = new HttpClient();

            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await httpClient.PutAsync(uri, httpContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception($"{nameof(SendApiPutRequestAsync)}: {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}");
            }

            return httpResponse;
        }

        private void ClearTransactionsCache() => transactionMessagesCached.Clear();
    }
}
