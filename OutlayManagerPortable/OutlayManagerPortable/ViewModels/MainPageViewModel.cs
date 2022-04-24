using OutlayManagerPortable.Models;
using OutlayManagerPortable.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace OutlayManagerPortable.ViewModels
{
    public class MainPageViewModel
    {
        private ITransactionOutlayService _transactionOutlayService;

        public MainPageViewModel()
        {
            _transactionOutlayService = DependencyService.Get<ITransactionOutlayService>() ?? throw new NullReferenceException();
        }

        public ObservableCollection<TransactionOutlay> LoadTransactions(DateTime date)
        {
            ObservableCollection<TransactionOutlay> transactions = new ObservableCollection<TransactionOutlay>(_transactionOutlayService.LoadTransactions(date));
            return transactions;
        }

     

    }
}
