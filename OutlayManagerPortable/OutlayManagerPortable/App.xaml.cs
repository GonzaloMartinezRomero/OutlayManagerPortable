using OutlayManagerPortable.Services.Abstract;
using OutlayManagerPortable.Services.Implementation;
using Xamarin.Forms;

namespace OutlayManagerPortable
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton<ITransactionService>(new TransactionAzureService());

            
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
