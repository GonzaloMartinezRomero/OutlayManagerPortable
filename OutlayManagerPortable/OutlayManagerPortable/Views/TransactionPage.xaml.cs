using OutlayManagerPortable.Models;
using OutlayManagerPortable.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutlayManagerPortable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionPage : ContentPage
    {
        private readonly TransactionViewModel transactionViewModel = new TransactionViewModel();
        private readonly TransactionOutlayModelView transactionOutlayView = new TransactionOutlayModelView() { Date = DateTime.Today.Date };

        public TransactionPage()
        {
            InitializeComponent();
            LoadMasterDataView();
            BindingContext = transactionOutlayView;
            this.DeleteButton.IsEnabled = false;
        }

        public TransactionPage(TransactionOutlayModelView transactionOutlay): this()
        {
            transactionOutlayView = transactionOutlay;
            BindingContext = transactionOutlayView;
            this.DeleteButton.IsEnabled = true;
        }

        private void LoadMasterDataView()
        {
            this.TransactionTypeSelector.ItemsSource = transactionViewModel.LoadTransactionTypes().Result;
            this.TransactionCodeSelector.ItemsSource = transactionViewModel.LoadTransactionCodes().Result;            
        }

        private async void SaveTransactionEvent(object sender, EventArgs e)
        {
            this.ActivityIndicator.IsRunning = true;
            this.Content.IsEnabled = false;

            OperationResponse operationResponse = await transactionViewModel.SaveTransaction(transactionOutlayView);

            this.ActivityIndicator.IsRunning = false;

            switch (operationResponse.OperationStatus)
            {
                case OperationStatus.ERROR:
                    await DisplayAlert("Transaction", $"Error on saving:{operationResponse.Message}", "OK");
                    break;

                case OperationStatus.OK:
                    await DisplayAlert("Transaction", "Transaction saved succesfully!", "OK");                   
                    break;
            }

            await this.Navigation.PopAsync(animated: true);
        }

        private async void DeleteTransactionEvent(object sender, EventArgs e)
        {
            this.ActivityIndicator.IsRunning = true;
            this.Content.IsEnabled = false;

            OperationResponse operationResponse = await transactionViewModel.DeleteTransaction(transactionOutlayView.Id);

            this.ActivityIndicator.IsRunning = false;

            switch (operationResponse.OperationStatus)
            {
                case OperationStatus.ERROR:
                    await DisplayAlert("Delete Transaction", $"Error on delete:{operationResponse.Message}", "OK");
                    break;

                case OperationStatus.OK:
                    await DisplayAlert("Delete Transaction", "Transaction deleted succesfully!", "OK");                   
                    break;
            }

            await this.Navigation.PopAsync(animated: true);
        }
    }
}