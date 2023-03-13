using OutlayManagerPortableMaui.Models;
using OutlayManagerPortableMaui.Models.Dto;
using OutlayManagerPortableMaui.Models.TransactionModelView;
using OutlayManagerPortableMaui.Services.Abstract;

namespace OutlayManagerPortableMaui.ViewModels
{
    public class MainPageViewModel
    {
        private readonly ITransactionService _transactionService;

        public MainPageViewModel()
        {

#if ANDROID
            _transactionService = MauiApplication.Current.Services.GetService<ITransactionService>();
#else
             throw new ApplicationException($"Unable to initialize {nameof(ITransactionService)}: Only available for Android");
# endif
        }

        public async Task<List<TransactionOutlayModelView>> LoadTransactions()
        {
            List<TransactionOutlayModelView> transactionsView = new List<TransactionOutlayModelView>();

            List<TransactionMessage> transactionMessages = await _transactionService.TransactionsQueued();

            if(transactionMessages.Count > 0)
            {
                IOrderedEnumerable<TransactionMessage> orderderTransactionMessages = transactionMessages.OrderBy(x => x.Date);

                foreach (TransactionMessage transactionMessageAux in orderderTransactionMessages)
                {
                    if (!TransactionMasterValues.TransactionTypes.TryGetValue(transactionMessageAux.TypeID, out TransactionType transactionType))
                        throw new Exception($"Transaction type ID:{transactionMessageAux.TypeID} not found");

                    if (!TransactionMasterValues.TransactionCodes.TryGetValue(transactionMessageAux.CodeID, out TransactionCode transactionCode))
                        throw new Exception($"Transaction code ID:{transactionMessageAux.CodeID} not found");

                    TransactionOutlayModelView transactionOutlayModelView = new TransactionOutlayModelView()
                    {
                        Amount = transactionMessageAux.Amount.ToString(),
                        Date = transactionMessageAux.Date,
                        Description = transactionMessageAux.Description,
                        Id = transactionMessageAux.Id,
                        Type = new TransactionTypeModelView(transactionType),
                        Code = new TransactionCodeModelView(transactionCode)
                    };

                    transactionsView.Add(transactionOutlayModelView);
                }
            }

            return transactionsView;
        }
    }
}
