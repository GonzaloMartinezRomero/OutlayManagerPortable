using OutlayManagerPortable.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace OutlayManagerPortable.ViewModels
{
    public class TransactionViewModel
    {
        private ITransactionOutlayService _transactionOutlayService;

        public TransactionViewModel()
        {
            _transactionOutlayService = DependencyService.Get<ITransactionOutlayService>() ?? throw new NullReferenceException();
        }

        public List<string> LoadTransactionCodes()
        {
            return _transactionOutlayService.Codes().ToList();
        }

        public List<string> LoadTransactionOperations()
        {
            return _transactionOutlayService.Operations().ToList();
        }
    }
}
