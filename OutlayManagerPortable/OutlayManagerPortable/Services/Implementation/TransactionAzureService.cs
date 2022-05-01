using Newtonsoft.Json;
using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Models;
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

        public Task<List<TransactionType>> TransactionTypes()
        {
            if (transactionTypesCached.Count == 0)
            {
                const string transactionTypeURI = "https://outlaymanagerportableapi.azurewebsites.net/DataMaster/TransactionType";

                List<TransactionType> response = SendApiGetRequest<List<TransactionType>>(transactionTypeURI);

                transactionTypesCached.AddRange(response);
            }

            return Task.FromResult(transactionTypesCached);
        }

        public Task<List<TransactionCode>> TransactionCodes()
        {
            if(transactionCodesCached.Count == 0)
            {
                const string transactionCodeURI = "https://outlaymanagerportableapi.azurewebsites.net/DataMaster/TransactionCode";

                List<TransactionCode> response = SendApiGetRequest<List<TransactionCode>>(transactionCodeURI);

                transactionCodesCached.AddRange(response);
            }

            return Task.FromResult(transactionCodesCached);
        }

        public Task<List<TransactionMessage>> TransactionsQueued()
        {
            const string transactionCodeURI = "https://outlaymanagerportableapi.azurewebsites.net/TransactionOutlay";

            if (transactionMessagesCached.Count == 0)
            { 
                List<TransactionMessage> response = SendApiGetRequest<List<TransactionMessage>>(transactionCodeURI);

                foreach (TransactionMessage transactionMessageAux in response)
                {
                    if (TransactionMessageIsValid(transactionMessageAux))
                        transactionMessagesCached.Add(transactionMessageAux);
                }
            }

            return Task.FromResult(transactionMessagesCached);
        }

        public Task<OperationResponse> SaveTransaction(TransactionMessage transactionMessage)
        {
            const string URI = "https://outlaymanagerportableapi.azurewebsites.net/TransactionOutlay";

            string bodyContent = JsonConvert.SerializeObject(transactionMessage);

            HttpResponseMessage response;

            if (transactionMessage.Id == Guid.Empty)
                response = SendApiPostRequest(URI, bodyContent);
            else
                response = SendApiPutRequest(URI, bodyContent);

            OperationResponse operationResponse = new OperationResponse();

            if (response.IsSuccessStatusCode)
            {
                operationResponse.OperationStatus = OperationStatus.OK;
            }
            else
            {
                operationResponse.OperationStatus = OperationStatus.ERROR;
                operationResponse.Message = response.Content.ReadAsStringAsync().Result;
            }

            ClearTransactionsCache();

            return Task.FromResult<OperationResponse>(operationResponse);
        }

        public Task<OperationResponse> DeleteTransaction(Guid transactionMessageId)
        {
            if (transactionMessageId == Guid.Empty)
                return Task.FromResult(new OperationResponse()
                {
                    OperationStatus = OperationStatus.ERROR,
                    Message = "Transaction id is null"
                });

            const string URI = "https://outlaymanagerportableapi.azurewebsites.net/TransactionOutlay";

            HttpResponseMessage response = SendApiDeleteRequest(URI, transactionMessageId);

            OperationResponse operationResponse = new OperationResponse();

            if (response.IsSuccessStatusCode)
            {
                operationResponse.OperationStatus = OperationStatus.OK;
            }
            else
            {
                operationResponse.OperationStatus = OperationStatus.ERROR;
                operationResponse.Message = response.Content.ReadAsStringAsync().Result;
            }

            ClearTransactionsCache();

            return Task.FromResult<OperationResponse>(operationResponse);
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

        private T SendApiGetRequest<T>(string uri) where T:new ()
        {
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage httpResponse = httpClient.GetAsync(uri).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                string content = httpResponse.Content.ReadAsStringAsync().Result;

                T deserializedResult = JsonConvert.DeserializeObject<T>(content);

                return deserializedResult;
            }

            return default;
        }

        private HttpResponseMessage SendApiPostRequest(string uri, string body)
        {
            HttpClient httpClient = new HttpClient();

            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = httpClient.PostAsync(uri, httpContent).Result;

            return httpResponse;
        }

        private HttpResponseMessage SendApiDeleteRequest(string uri, Guid messageId)
        {
            HttpClient httpClient = new HttpClient();

            string deleteUri = String.Join("/", new string[]{ uri, messageId.ToString()});

            HttpResponseMessage httpResponse = httpClient.DeleteAsync(deleteUri).Result;

            return httpResponse;
        }

        private HttpResponseMessage SendApiPutRequest(string uri, string body)
        {
            HttpClient httpClient = new HttpClient();

            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = httpClient.PutAsync(uri, httpContent).Result;

            return httpResponse;
        }

        private void ClearTransactionsCache() => transactionMessagesCached.Clear();
    }
}
