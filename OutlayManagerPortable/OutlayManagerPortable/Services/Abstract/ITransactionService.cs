using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutlayManagerPortable.Services.Abstract
{
    public interface ITransactionService
    {
        Task<OperationResponse> SaveTransaction(TransactionMessage transactionMessage);

        Task<OperationResponse> DeleteTransaction(Guid transactionMessageId);

        Task<List<TransactionMessage>> TransactionsQueued();

        Task<List<TransactionCode>> TransactionCodes();

        Task<List<TransactionType>> TransactionTypes();
    }
}
