using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using OutlayManagerPortableMaui.Models.Dto;
using OutlayManagerPortableMaui.Services.Abstract;
using System.Text.Json;

namespace OutlayManagerPortableMaui.Services.Implementation
{
    internal class TransactionAzureService : ITransactionService
    {
        private const string QUEUE_CONNECTION_STRING = "AzureTransactionQueue:ConnectionString";
        private const string QUEUE_NAME = "AzureTransactionQueue:Name";

        private readonly QueueClient queueClient;
        private readonly Dictionary<string, QueueMessage> cachedTransactions = new Dictionary<string, QueueMessage>();

        private QueueClientOptions TransactionMessageOptions 
        {
            get
            {
                return new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };
            } 
        }

        public TransactionAzureService(IConfiguration configuration)
        {
            string connectionString = configuration[QUEUE_CONNECTION_STRING] ?? throw new ArgumentException(nameof(QUEUE_CONNECTION_STRING));
            string queueName = configuration[QUEUE_NAME] ?? throw new ArgumentException(nameof(QUEUE_NAME));

            queueClient = new QueueClient(connectionString, queueName, TransactionMessageOptions);
        }

        public async Task DeleteTransaction(Guid transactionMessageId)
        {
            var messages = await LoadTransactionsQueued();            

            if (messages.TryGetValue(transactionMessageId.ToString(), out QueueMessage value))
            {
                await queueClient.DeleteMessageAsync(value.MessageId, value.PopReceipt);
                ClearTransactionsCached();
            }   
            else
                throw new ApplicationException("Transaction not found for delete");
        }

        public async Task SaveTransaction(TransactionMessage transactionMessage)
        {   
            string transactionMessageJson = JsonSerializer.Serialize(transactionMessage);
            var response = await queueClient.SendMessageAsync(transactionMessageJson);

            TransactionMessage transactionUpdated = transactionMessage;
            transactionUpdated.Id = Guid.Parse(response.Value.MessageId);

            string transactionMessageIdUpdated = JsonSerializer.Serialize(transactionUpdated);
            await queueClient.UpdateMessageAsync(response.Value.MessageId, response.Value.PopReceipt, transactionMessageIdUpdated);

            ClearTransactionsCached();
        }

        public async Task<List<TransactionMessage>> TransactionsQueued()
        {
            List<TransactionMessage> transactions = new List<TransactionMessage>();
            var messages = await LoadTransactionsQueued();

            foreach (var message in messages.Values)
            {
                string transactionJsonMessage = message.Body.ToString();
                TransactionMessage transactionMessage = JsonSerializer.Deserialize<TransactionMessage>(transactionJsonMessage);

                transactions.Add(transactionMessage);   
            }

            return transactions;
        }

        private async Task<Dictionary<string,QueueMessage>> LoadTransactionsQueued()
        {
            if(cachedTransactions.Count == 0)
            {
                var messages = await queueClient.ReceiveMessagesAsync();

                foreach (var message in messages.Value)
                {
                    cachedTransactions.Add(message.MessageId, message);
                }
            }

            return cachedTransactions;
        }

        private void ClearTransactionsCached () => cachedTransactions.Clear();
    }
}
