using OutlayManagerPortable.Models;
using OutlayManagerPortable.ViewModels;
using OutlayManagerPortable.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace OutlayManagerPortable
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel mainPageViewModel = new MainPageViewModel();
        private readonly ObservableCollection<TransactionOutlayModelView> transctionView = new ObservableCollection<TransactionOutlayModelView>();

        public MainPage()
        {
            InitializeComponent();
            this.TransactionListView.ItemsSource = transctionView;
            this.MainPageContent.Appearing += LoadTransactionInViewAsync;
        }

        private async void LoadTransactionInViewAsync(object sender, EventArgs e)
        {
            ShowLoadingView();

            List<TransactionOutlayModelView> transactions = await mainPageViewModel.LoadTransactions();

            transctionView.Clear();

            foreach (var transactionAux in transactions)
                transctionView.Add(transactionAux);

            if (transctionView.Count == 0)
            {
                this.NotificationLabel.IsVisible = true;
                this.NotificationLabel.Text = "No transactions availables";
            }
            else
            {
                this.NotificationLabel.IsVisible = false;
            }

            HideLoadingView();
        }

        private async void ItemSelectedEvent(object sender, ItemTappedEventArgs e)
        {
            TransactionOutlayModelView transactionSelected = (TransactionOutlayModelView)e?.Item ?? new TransactionOutlayModelView();

            await this.Navigation.PushAsync(new TransactionPage(transactionSelected));
        }

        private async void AddNewTransactionEvent(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new TransactionPage(), animated: true);
        }

        private void ShowLoadingView()
        {
            this.loadingView.IsVisible = true;
            this.loadingIndicador.IsRunning = true;
            saveButtonView.IsVisible = false;
            transactionListViewContainer.IsVisible = false;
        }

        private void HideLoadingView()
        {
            this.loadingView.IsVisible = false;
            this.loadingIndicador.IsRunning = false;
            saveButtonView.IsVisible = true;
            transactionListViewContainer.IsVisible = true;
        }
    }
}
