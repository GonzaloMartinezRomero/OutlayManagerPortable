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

        public Task<ObservableCollection<TransactionOutlayModelView>> LoadTransactions()
        {
            ObservableCollection<TransactionOutlayModelView> transactionsView = new ObservableCollection<TransactionOutlayModelView>();

            List<TransactionMessage> transactionMessages = _transactionService.TransactionsQueued().Result;

            if(transactionMessages.Count > 0)
            {
                Dictionary<int, TransactionType> dictTypeTransaction = _transactionService.TransactionTypes().Result
                                                                                          .ToDictionary(key => key.Id, value => value);
                
                Dictionary<int, TransactionCode> dictCodeTransaction = _transactionService.TransactionCodes().Result
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

            return Task.FromResult(transactionsView);
        }
    }
}
