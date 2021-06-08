using DevIO.App.Extensions;
using DevIO.Business.Interfaces;
using DevIO.Business.Notifications;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repositories;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();

            services.AddScoped<INotifier, Notifier>();

            services.AddSingleton<IValidationAttributeAdapterProvider, CoinValidationAttributeAdapterProvider>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProviderService, ProviderService>();

            return services;
        }
    }
}
