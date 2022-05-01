using OutlayManagerPortable.DTO;
using OutlayManagerPortable.Models;
using OutlayManagerPortable.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OutlayManagerPortable.ViewModels
{
    internal class TransactionViewModel
    {
        private ITransactionService _transactionService;

        public TransactionViewModel()
        {
            _transactionService = DependencyService.Get<ITransactionService>() ?? throw new NullReferenceException();
        }

        public Task<List<TransactionTypeModelView>> LoadTransactionTypes()
        {
            List<TransactionTypeModelView> transactionTypeModelViews = new List<TransactionTypeModelView>();

            List<TransactionType> transactionTypes = _transactionService.TransactionTypes().Result;

            transactionTypes.ForEach(x => transactionTypeModelViews.Add(new TransactionTypeModelView(x)));

            return Task.FromResult(transactionTypeModelViews);
        }

        public Task<List<TransactionCodeModelView>> LoadTransactionCodes()
        {
            List<TransactionCodeModelView> transactionCodeModelViews = new List<TransactionCodeModelView>();

            List<TransactionCode> transactionCodes = _transactionService.TransactionCodes().Result;

            transactionCodes.ForEach(x => transactionCodeModelViews.Add(new TransactionCodeModelView(x)));

            return Task.FromResult(transactionCodeModelViews);
        }

        public Task<OperationResponse> SaveTransaction(TransactionOutlayModelView transactionOutlayView)
        {
            TransactionMessage transactionMessage = new TransactionMessage()
            {
                Id = transactionOutlayView.Id,
                Amount = transactionOutlayView.Amount,
                CodeID= transactionOutlayView.Code.Id,
                TypeID = transactionOutlayView.Type.Id,
                Date = transactionOutlayView.Date,
                Description = transactionOutlayView.Description
            };

            return _transactionService.SaveTransaction(transactionMessage);
        }

        public Task<OperationResponse> DeleteTransaction(Guid transactionID)
        {
            return _transactionService.DeleteTransaction(transactionID);
        }
    }
}
