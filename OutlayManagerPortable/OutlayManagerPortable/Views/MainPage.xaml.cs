
using OutlayManagerPortable.Models;
using OutlayManagerPortable.ViewModels;
using OutlayManagerPortable.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OutlayManagerPortable
{

    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel mainPageViewModel = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();             
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadTransactionsView();
        }

        private async void LoadTransactionsView()
        {
            this.TransactionListView.IsVisible = false;
            this.ActivityIndicator.IsRunning = true;
            this.ActivityIndicator.IsVisible = true;

            ObservableCollection<TransactionOutlayModelView> transactions = await mainPageViewModel.LoadTransactions();

            this.TransactionListView.ItemsSource = transactions;
            
            if (transactions.Count == 0)
                this.AdvertisementText.Text = "No transactions availables";

            this.ActivityIndicator.IsRunning = false;
            this.TransactionListView.IsVisible = true;
            this.ActivityIndicator.IsVisible = false;
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
    }
}
