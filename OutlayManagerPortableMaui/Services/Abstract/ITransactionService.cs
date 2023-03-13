using OutlayManagerPortableMaui.Models.Dto;

namespace OutlayManagerPortableMaui.Services.Abstract
{
    public interface ITransactionService
    {
        Task SaveTransaction(TransactionMessage transactionMessage);

        Task DeleteTransaction(Guid transactionMessageId);

        Task<List<TransactionMessage>> TransactionsQueued();
    }
}
