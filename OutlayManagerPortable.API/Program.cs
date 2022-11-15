using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace OutlayManagerPortable.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => 
                {
                    //If is production load settings from KeyVault
                    if (context.HostingEnvironment.IsProduction())
                    {
                        string keyVaultEndpoint = GetKeyVaultEndpoint();                                            

                        if (String.IsNullOrEmpty(keyVaultEndpoint))
                            throw new ArgumentNullException("Key vault endopoint not exist");
                                     
                        var secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());
                        
                        config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());                       
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {   
                    webBuilder.UseStartup<Startup>()
                              .ConfigureLogging(config=> 
                              {
                                  config.ClearProviders();
                                  config.AddConsole();
                                  config.AddAzureWebAppDiagnostics();
                              });
                });

        static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");
    }
}
