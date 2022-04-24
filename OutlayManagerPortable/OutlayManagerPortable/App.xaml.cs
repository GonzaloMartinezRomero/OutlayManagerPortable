using OutlayManagerPortable.Services.Abstract;
using OutlayManagerPortable.Services.Implementation;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OutlayManagerPortable
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<ITransactionOutlayService, TransactionOutlayAPIService>();

            MainPage = new NavigationPage(new MainPage());
            
        }

        protected override void OnStart()
        {
            //Check internet connectivity
            //var a = Connectivity.NetworkAccess;
            //if (a == NetworkAccess.Internet)
            //{
                    
            //}
            //else
            //{
            //    //Error
            //}

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
