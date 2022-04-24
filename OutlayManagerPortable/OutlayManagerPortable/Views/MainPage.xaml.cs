using OutlayManagerPortable.Models;
using OutlayManagerPortable.ViewModels;
using OutlayManagerPortable.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OutlayManagerPortable
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel mainPageViewModel = new MainPageViewModel();
        private ObservableCollection<TransactionOutlay> transactions;

        public MainPage()
        {
            InitializeComponent();
            transactions = mainPageViewModel.LoadTransactions(DateTime.Now);
            this.TransactionListView.ItemsSource = transactions; 
           
        }

        private void DateSelectedChanged(object sender, DateChangedEventArgs e)
        {
            transactions = mainPageViewModel.LoadTransactions(DateTime.Now);
        }

        private async void ItemSelectedEvent(object sender, ItemTappedEventArgs e)
        {
            TransactionOutlay transactionSelected = (TransactionOutlay)e?.Item ?? new TransactionOutlay();

            await this.Navigation.PushAsync(new TransactionPage(transactionSelected));
        }

        private async void AddNewTransaction(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new TransactionPage());
        }
    }
}
