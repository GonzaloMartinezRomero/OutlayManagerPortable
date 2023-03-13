using OutlayManagerPortableMaui.Models;
using OutlayManagerPortableMaui.Models.Dto;
using OutlayManagerPortableMaui.Models.TransactionModelView;
using OutlayManagerPortableMaui.Services.Abstract;
using System.Globalization;

namespace OutlayManagerPortableMaui.ViewModels
{
    internal class TransactionViewModel
    {
        private readonly ITransactionService _transactionService;

        private List<TransactionTypeModelView> transactionTypeModelViews = new List<TransactionTypeModelView>();
        private List<TransactionCodeModelView> transactionCodeModelViews = new List<TransactionCodeModelView>();

        public TransactionViewModel()
        {

#if ANDROID
            _transactionService = MauiApplication.Current.Services.GetService<ITransactionService>();
#else
             throw new ApplicationException($"Unable to initialize {nameof(ITransactionService)}: Only available for Android");
#endif
        }

        public List<TransactionTypeModelView> TransactionTypesViewModel()
        {
            if(transactionTypeModelViews.Count == 0)
                foreach(var transactionTypeAux in TransactionMasterValues.TransactionTypes)
                {
                    transactionTypeModelViews.Add(new TransactionTypeModelView(transactionTypeAux.Value));
                }

            return transactionTypeModelViews;
        }

        public List<TransactionCodeModelView> TransactionCodesViewModel()
        {
            if (transactionCodeModelViews.Count == 0)
                foreach (var transactionCodeAux in TransactionMasterValues.TransactionCodes)
                {
                    transactionCodeModelViews.Add(new TransactionCodeModelView(transactionCodeAux.Value));
                }

            return transactionCodeModelViews;
        }

        public async Task SaveTransactionAsync(TransactionOutlayModelView transactionOutlayView)
        {
            if (transactionOutlayView == null)
                throw new ArgumentNullException($"{nameof(SaveTransactionAsync)}: Error on save null transaction");

            double amountParsed = ParseStringToDouble(transactionOutlayView.Amount);

            TransactionMessage transactionMessage = new TransactionMessage()
            {
                Id = transactionOutlayView.Id,
                Amount = amountParsed,
                CodeID= transactionOutlayView.Code?.Id ?? throw new NullReferenceException($"{nameof(TransactionOutlayModelView.Code)} is null"),
                TypeID = transactionOutlayView.Type?.Id ?? throw new NullReferenceException($"{nameof(TransactionOutlayModelView.Type)} is null"),
                Date = transactionOutlayView.Date,
                Description = transactionOutlayView.Description
            };

            await _transactionService.SaveTransaction(transactionMessage);
        }

        private double ParseStringToDouble(string amount)
        {
            string amountNormalized = amount.Replace(',', '.');

            NumberFormatInfo numberFormatInfo = new NumberFormatInfo() 
            { 
                NumberDecimalSeparator = "."
            };

            if (!Double.TryParse(amountNormalized, NumberStyles.Float, numberFormatInfo, out double amountParsed))
                throw new Exception($"Unable to cast {amount} to double");

            return amountParsed;
        }

        public async Task DeleteTransaction(Guid transactionID)
        {
            await _transactionService.DeleteTransaction(transactionID);
        }
    }
}
