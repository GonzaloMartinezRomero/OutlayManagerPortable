using OutlayManagerPortable.Models;
using OutlayManagerPortable.ViewModels;
using System;
using System.Threading.Tasks;
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

            this.DeleteButton.IsEnabled = false;

            this.MainContentPage.Appearing += LoadMasterDataInViewAsync;
        }

        private async void LoadMasterDataInViewAsync(object sender, EventArgs e)
        {
            ShowLoadingView();
                        
            this.TransactionTypeSelector.ItemsSource = await transactionViewModel.LoadTransactionTypesAsync();
            this.TransactionCodeSelector.ItemsSource = await transactionViewModel.LoadTransactionCodesAsync();

            BindingContext = transactionOutlayView;

            HideLoadingView();
        }    

        public TransactionPage(TransactionOutlayModelView transactionOutlay): this()
        {
            transactionOutlayView = transactionOutlay;
           
            this.DeleteButton.IsEnabled = true;
        }

        private async void SaveTransactionEvent(object sender, EventArgs e)
        {
            ShowLoadingView();

            OperationResponse operationResponse = await transactionViewModel.SaveTransactionAsync(transactionOutlayView);

            HideLoadingView();

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
            ShowLoadingView();

            OperationResponse operationResponse = await transactionViewModel.DeleteTransaction(transactionOutlayView.Id);

            HideLoadingView();

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

        private void ShowLoadingView()
        {
            mainStackLayout.IsVisible = false;
            loadingScreen.IsVisible = true;
            loadingIndicador.IsRunning = true;
            loadingIndicador.IsVisible = true;
        }

        private void HideLoadingView()
        {
            mainStackLayout.IsVisible = true;
            loadingScreen.IsVisible = false;
            loadingIndicador.IsRunning = false;
            loadingIndicador.IsVisible = false;
        }
    }
}