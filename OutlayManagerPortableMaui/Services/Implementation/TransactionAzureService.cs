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
        private const int MAX_MESSAGES_RECEIVED = 30;

        private readonly QueueClient queueClient;
        private readonly Dictionary<Guid, AzureTransactionMessage> cachedTransactions = new Dictionary<Guid, AzureTransactionMessage>();

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

            if (messages.TryGetValue(transactionMessageId, out AzureTransactionMessage value))
            {
                await queueClient.DeleteMessageAsync(value.Id.ToString(), value.PopReceipt);
                cachedTransactions.Remove(transactionMessageId);
            }   
            else
                throw new ApplicationException("Transaction not found for delete");
        }

        public async Task SaveTransaction(TransactionMessage transactionMessage)
        {   
            if(cachedTransactions.ContainsKey(transactionMessage.Id))
                await DeleteTransaction(transactionMessage.Id);

            string transactionMessageJson = JsonSerializer.Serialize(transactionMessage);
            var response = await queueClient.SendMessageAsync(transactionMessageJson,
                                                              visibilityTimeout:TimeSpan.FromSeconds(0),
                                                              timeToLive:TimeSpan.FromSeconds(-1));

            AzureTransactionMessage transactionUpdated = new AzureTransactionMessage(transactionMessage);
            transactionUpdated.Id = Guid.Parse(response.Value.MessageId);
            transactionUpdated.PopReceipt = response.Value.PopReceipt;

            cachedTransactions.Add(Guid.Parse(response.Value.MessageId), transactionUpdated);

            if (cachedTransactions.Count >= MAX_MESSAGES_RECEIVED)
                throw new ApplicationException("More messages than queue view can support");
        }

        public async Task<List<TransactionMessage>> TransactionsQueued()
        {
            List<TransactionMessage> transactions = new List<TransactionMessage>();
            var messages = await LoadTransactionsQueued();

            foreach (var message in messages.Values)
                transactions.Add(message);   

            return transactions;
        }

        private async Task<Dictionary<Guid, AzureTransactionMessage>> LoadTransactionsQueued()
        {
            if(cachedTransactions.Count == 0)
            {
                var messages = await queueClient.ReceiveMessagesAsync(MAX_MESSAGES_RECEIVED, TimeSpan.FromSeconds(1));

                foreach (var message in messages.Value)
                {
                    AzureTransactionMessage azureTransactionMessage = BuildAzureMessage(message);

                    cachedTransactions.Add(azureTransactionMessage.Id, azureTransactionMessage);
                }
            }

            return cachedTransactions;
        }

        private AzureTransactionMessage BuildAzureMessage(QueueMessage queueMessage)
        {
            TransactionMessage transactionMessage = JsonSerializer.Deserialize<TransactionMessage>(queueMessage.Body.ToString());

            AzureTransactionMessage azureTransactionMessage = new AzureTransactionMessage(transactionMessage);

            azureTransactionMessage.Id = Guid.Parse(queueMessage.MessageId);
            azureTransactionMessage.PopReceipt = queueMessage.PopReceipt;

            return azureTransactionMessage;
        }
    }
}
