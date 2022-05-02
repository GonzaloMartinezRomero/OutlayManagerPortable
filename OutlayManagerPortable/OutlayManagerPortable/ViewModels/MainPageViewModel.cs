using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Models;
using OutlayManagerPortable.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OutlayManagerPortable.ViewModels
{
    public class MainPageViewModel
    {
        private ITransactionService _transactionService;

        public MainPageViewModel()
        {
            _transactionService = DependencyService.Get<ITransactionService>() ?? throw new NullReferenceException();
        }

        public async Task<List<TransactionOutlayModelView>> LoadTransactions()
        {
            List<TransactionOutlayModelView> transactionsView = new List<TransactionOutlayModelView>();

            List<TransactionMessage> transactionMessages = await _transactionService.TransactionsQueued();

            if(transactionMessages.Count > 0)
            {
                Dictionary<int, TransactionType> dictTypeTransaction = (await _transactionService.TransactionTypes())
                                                                                          .ToDictionary(key => key.Id, value => value);
                
                Dictionary<int, TransactionCode> dictCodeTransaction = (await _transactionService.TransactionCodes())
                                                                                          .ToDictionary(key => key.Id, value => value);

                IOrderedEnumerable<TransactionMessage> orderderTransactionMessages = transactionMessages.OrderBy(x => x.Date);

                foreach (TransactionMessage transactionMessageAux in orderderTransactionMessages)
                {
                    if (!dictTypeTransaction.TryGetValue(transactionMessageAux.TypeID, out TransactionType transactionType))
                        throw new Exception($"Transaction type ID:{transactionMessageAux.TypeID} not found");

                    if (!dictCodeTransaction.TryGetValue(transactionMessageAux.CodeID, out TransactionCode transactionCode))
                        throw new Exception($"Transaction code ID:{transactionMessageAux.CodeID} not found");

                    TransactionOutlayModelView transactionOutlayModelView = new TransactionOutlayModelView()
                    {
                        Amount = transactionMessageAux.Amount,
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
