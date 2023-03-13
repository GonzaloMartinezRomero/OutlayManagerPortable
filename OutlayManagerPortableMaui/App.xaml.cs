using OutlayManagerPortableMaui.Services.Abstract;
using OutlayManagerPortableMaui.Services.Implementation;

namespace OutlayManagerPortableMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}