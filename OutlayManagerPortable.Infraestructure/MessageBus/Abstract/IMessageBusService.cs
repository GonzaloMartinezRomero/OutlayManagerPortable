using OutlayManagerPortable.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Infraestructure.MessageBus.Abstract
{
    public interface IMessageBusService: IDisposable
    {
        Task<TransactionMessage> PublishMessageAsync(TransactionMessage transactionMessage);

        Task DeleteMessageAsync(Guid transactionMessageID);

        Task<TransactionMessage> UpdateMessageAsync(TransactionMessage transactionMessage);

        Task<List<TransactionMessage>> MessagesQueuedAsync();
    }
}
