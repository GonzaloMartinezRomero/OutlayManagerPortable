using OutlayManagerPortable.Models;
using OutlayManagerPortable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutlayManagerPortable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionPage : ContentPage
    {
        private readonly TransactionViewModel transactionViewModel = new TransactionViewModel();

        public TransactionPage()
        {
            InitializeComponent();

            this.TransactionTypeSelector.ItemsSource = transactionViewModel.LoadTransactionOperations();
            this.TransactionCodeSelector.ItemsSource = transactionViewModel.LoadTransactionCodes();

            if (BindingContext == null)
                BindingContext = new TransactionOutlay();
        }

        public TransactionPage(TransactionOutlay transactionOutlay):this()
        {   
            BindingContext = transactionOutlay ?? new TransactionOutlay();
        }

        private async void AddNewTransaction(object sender, EventArgs e)
        {
            //Transaction t = (Transaction)this.BindingContext;

            //newTransactionModel.AddNewTransaction(t);

            //this.ActivityIndicator.IsRunning = true;
            //await Task.Delay(5000);
            //this.ActivityIndicator.IsRunning = false;
            //await DisplayAlert("Transaction", "Transaction saved succesfully!", "OK");
        }
    }
}