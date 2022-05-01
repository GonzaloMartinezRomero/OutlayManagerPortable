using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutlayManagerPortable.Infraestructure.Data.Abstraction;
using OutlayManagerPortable.Infraestructure.Data.Implementation;
using OutlayManagerPortable.Infraestructure.MessageBus.Abstract;
using OutlayManagerPortable.Infraestructure.MessageBus.Implementation;
using System;

namespace OutlayManagerPortable.Infraestructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfraestructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IMessageBusService, MessageBusServiceAzure>(service => new MessageBusServiceAzure(configuration));
            serviceCollection.AddTransient<IMasterData, MasterDataAzureSQL>(service => new MasterDataAzureSQL(configuration));

            return serviceCollection;
        }
    }
}
