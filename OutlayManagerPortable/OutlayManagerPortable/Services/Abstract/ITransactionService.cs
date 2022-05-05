using OutlayManagerPortable.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Services.Abstract
{
    public interface ITransactionService
    {
        Task SaveTransaction(TransactionMessage transactionMessage);

        Task DeleteTransaction(Guid transactionMessageId);

        Task<List<TransactionMessage>> TransactionsQueued();

        Task<List<TransactionCode>> TransactionCodes();

        Task<List<TransactionType>> TransactionTypes();
    }
}
