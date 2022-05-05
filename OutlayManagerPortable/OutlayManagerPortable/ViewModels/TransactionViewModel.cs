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

        public async Task<List<TransactionTypeModelView>> LoadTransactionTypesAsync()
        {
            List<TransactionTypeModelView> transactionTypeModelViews = new List<TransactionTypeModelView>();

            List<TransactionType> transactionTypes = await _transactionService.TransactionTypes();

            transactionTypes.ForEach(x => transactionTypeModelViews.Add(new TransactionTypeModelView(x)));

            return transactionTypeModelViews;
        }

        public async Task<List<TransactionCodeModelView>> LoadTransactionCodesAsync()
        {
            List<TransactionCodeModelView> transactionCodeModelViews = new List<TransactionCodeModelView>();

            List<TransactionCode> transactionCodes = await _transactionService.TransactionCodes();

            transactionCodes.ForEach(x => transactionCodeModelViews.Add(new TransactionCodeModelView(x)));

            return transactionCodeModelViews;
        }

        public async Task SaveTransactionAsync(TransactionOutlayModelView transactionOutlayView)
        {
            if (transactionOutlayView == null)
                throw new ArgumentNullException($"{nameof(SaveTransactionAsync)}: Error on save null transaction");

            TransactionMessage transactionMessage = new TransactionMessage()
            {
                Id = transactionOutlayView.Id,
                Amount = transactionOutlayView.Amount,
                CodeID= transactionOutlayView.Code?.Id ?? throw new NullReferenceException($"{nameof(TransactionOutlayModelView.Code)} is null"),
                TypeID = transactionOutlayView.Type?.Id ?? throw new NullReferenceException($"{nameof(TransactionOutlayModelView.Type)} is null"),
                Date = transactionOutlayView.Date,
                Description = transactionOutlayView.Description
            };

            await _transactionService.SaveTransaction(transactionMessage);
        }

        public async Task DeleteTransaction(Guid transactionID)
        {
            await _transactionService.DeleteTransaction(transactionID);
        }
    }
}
