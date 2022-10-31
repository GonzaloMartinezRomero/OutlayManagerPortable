using Newtonsoft.Json;
using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Services.Implementation
{
    internal class TransactionAzureService : ITransactionService
    {
        private const string HOST = "";
        private const string HEADER_ACCESS_KEY = "Ocp-Apim-Subscription-Key";
        private const string ACCESS_KEY = "";

        private readonly List<TransactionType> transactionTypesCached= new List<TransactionType>();
        private readonly List<TransactionCode> transactionCodesCached = new List<TransactionCode>();
        private readonly List<TransactionMessage> transactionMessagesCached = new List<TransactionMessage>();        

        public async Task<List<TransactionType>> TransactionTypes()
        {
            if (transactionTypesCached.Count == 0)
            {
                string transactionTypeURI = $"{HOST}/DataMaster/TransactionType";

                HttpService httpService = new HttpService();

                Dictionary<string, string> headers = new Dictionary<string, string>() { { HEADER_ACCESS_KEY, ACCESS_KEY } };

                List<TransactionType> response = await httpService.SendApiGetRequestAsync<List<TransactionType>>(transactionTypeURI, headers);

                transactionTypesCached.AddRange(response);
            }

            return transactionTypesCached;
        }

        public async Task<List<TransactionCode>> TransactionCodes()
        {
            if(transactionCodesCached.Count == 0)
            {
                string transactionCodeURI = $"{HOST}/DataMaster/TransactionCode";

                HttpService httpService = new HttpService();

                Dictionary<string, string> headers = new Dictionary<string, string>() { { HEADER_ACCESS_KEY, ACCESS_KEY } };

                List<TransactionCode> response = await httpService.SendApiGetRequestAsync<List<TransactionCode>>(transactionCodeURI,headers);

                transactionCodesCached.AddRange(response);
            }

            return transactionCodesCached;
        }

        public async Task<List<TransactionMessage>> TransactionsQueued()
        {
            string transactionCodeURI = $"{HOST}/TransactionOutlay";

            if (transactionMessagesCached.Count == 0)
            {
                HttpService httpService = new HttpService();

                Dictionary<string, string> headers = new Dictionary<string, string>() { { HEADER_ACCESS_KEY, ACCESS_KEY } };

                List<TransactionMessage> response = await httpService.SendApiGetRequestAsync<List<TransactionMessage>>(transactionCodeURI,headers);

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
            string URI = $"{HOST}/TransactionOutlay";

            string bodyContent = JsonConvert.SerializeObject(transactionMessage);

            HttpService httpService = new HttpService();

            Dictionary<string, string> headers = new Dictionary<string, string>() { { HEADER_ACCESS_KEY, ACCESS_KEY } };

            if (transactionMessage.Id == Guid.Empty)
                _ = await httpService.SendApiPostRequestAsync(URI, bodyContent,headers);
            else
                _ = await httpService.SendApiPutRequestAsync(URI, bodyContent, headers);

            ClearTransactionsCache();
        }

        public async Task DeleteTransaction(Guid transactionMessageId)
        {
            if (transactionMessageId == Guid.Empty)
                throw new NullReferenceException($"{nameof(DeleteTransaction)}: {nameof(transactionMessageId)} is null or empty");

            string URI = $"{HOST}/TransactionOutlay";

            HttpService httpService = new HttpService();

            Dictionary<string, string> headers = new Dictionary<string, string>() { { HEADER_ACCESS_KEY, ACCESS_KEY } };

            _ = await httpService.SendApiDeleteRequestAsync(URI, transactionMessageId, headers );
                        
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

        private void ClearTransactionsCache() => transactionMessagesCached.Clear();
    }
}
