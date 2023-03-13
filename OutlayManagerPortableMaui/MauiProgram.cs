using Microsoft.Extensions.Configuration;
using OutlayManagerPortableMaui.Services.Abstract;
using OutlayManagerPortableMaui.Services.Implementation;
using System.Reflection;

namespace OutlayManagerPortableMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()                
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Add configuration file
            const string CONFIGURATION_FILE = "OutlayManagerPortableMaui.appsettings.json";            
            using var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream(CONFIGURATION_FILE);

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);

            //Add services
            builder.Services.AddSingleton<ITransactionService, TransactionAzureService>();

            return builder.Build();
        }
    }
}