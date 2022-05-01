using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Infraestructure.MessageBus.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Infraestructure.MessageBus.Implementation
{
    internal class MessageBusServiceAzure : IMessageBusService
    {
        private const string CONNECTION_KEY = "StringConnectionServiceBus";
        private const string QUEUE_KEY = "QueueServiceBus";
        private const string MAX_MESSAGES_KEY = "MaxMessagesServiceBus";

        private readonly string CONNECTION_SERVICE_BUS;
        private readonly string QUEUE_NAME;
        private readonly int MAX_MESSAGES;

        private readonly ServiceBusClient serviceBusClient;

        private MessageBusServiceAzure() { }

        public MessageBusServiceAzure(IConfiguration configuration)
        {
            CONNECTION_SERVICE_BUS = configuration.GetSection(CONNECTION_KEY)?.Value ?? throw new Exception($"Configuration {CONNECTION_KEY} not defined!");
            QUEUE_NAME = configuration.GetSection(QUEUE_KEY)?.Value ?? throw new Exception($"Configuration {QUEUE_KEY} not defined!");

            string maxMessagesValue = configuration.GetSection(MAX_MESSAGES_KEY)?.Value ?? throw new Exception($"Configuration {MAX_MESSAGES_KEY} not defined!");
            if (!Int32.TryParse(maxMessagesValue, out MAX_MESSAGES))
                throw new Exception($"Value for{MAX_MESSAGES_KEY} must be Int32");

            serviceBusClient = new ServiceBusClient(CONNECTION_SERVICE_BUS);
        }

        public async Task<List<TransactionMessage>> MessagesQueuedAsync()
        {
            ServiceBusReceiver receiver = null;

            try
            {
                receiver = serviceBusClient.CreateReceiver(QUEUE_NAME, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });

                IReadOnlyList<ServiceBusReceivedMessage> messagesReceived = await receiver.PeekMessagesAsync(MAX_MESSAGES);

                List<TransactionMessage> transactionsMessages = new();

                foreach (ServiceBusReceivedMessage messageAux in messagesReceived)
                {
                    TransactionMessage transactionMessageParsed = messageAux.Body.ToObjectFromJson<TransactionMessage>();
                    transactionsMessages.Add(transactionMessageParsed);
                }

                return transactionsMessages;
            }
            finally
            {
                receiver?.DisposeAsync();
            }
        }

        public async Task DeleteMessageAsync(Guid transactionMessageID)
        {
            ServiceBusReceiver receiver = null;

            try
            {
                receiver = serviceBusClient.CreateReceiver(QUEUE_NAME,new ServiceBusReceiverOptions() { ReceiveMode=ServiceBusReceiveMode.PeekLock});

                IReadOnlyList<ServiceBusReceivedMessage> messagesReceived = await receiver.ReceiveMessagesAsync(MAX_MESSAGES);

                ServiceBusReceivedMessage messageForDelete = messagesReceived.Where(x =>
                                                                              {
                                                                                  TransactionMessage transactionMessageAux = x.Body.ToObjectFromJson<TransactionMessage>();
                                                                                  return transactionMessageAux.Id == transactionMessageID;
                                                                              })
                                                                             .FirstOrDefault() ?? throw new KeyNotFoundException($"TransactionMessageID:{transactionMessageID} not found");

                await receiver.CompleteMessageAsync(messageForDelete);
            }
            finally
            {
                receiver?.DisposeAsync();
            }
        }

        public async Task<TransactionMessage> UpdateMessageAsync(TransactionMessage transactionMessage)
        {
            await DeleteMessageAsync(transactionMessage.Id);
            TransactionMessage transactionAdded = await PublishMessageAsync(transactionMessage);
            
            return transactionAdded;
        }

        public async Task<TransactionMessage> PublishMessageAsync(TransactionMessage transactionMessage)
        {
            ServiceBusSender sender = null;

            transactionMessage.Id = Guid.NewGuid();

            try
            {
                sender = serviceBusClient.CreateSender(QUEUE_NAME);

                ServiceBusMessage message = BuildServiceBusMessage(transactionMessage);

                await sender.SendMessageAsync(message);

                return transactionMessage;
            }
            finally
            {
                sender?.DisposeAsync();
            }        
        }

        private ServiceBusMessage BuildServiceBusMessage(TransactionMessage transactionMessage)
        {
            if (transactionMessage == null)
                throw new ArgumentNullException($"Error building message:{nameof(TransactionMessage)} is null");

            string transactionSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(transactionMessage);

            ServiceBusMessage message = new ServiceBusMessage(transactionSerialized);

            return message;
        }

        public void Dispose()
        {
            serviceBusClient?.DisposeAsync();
        }
    }
}
