using App.Services.Products;
using App.Services.Weather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Services.Extension
{
    public static class ServiceExtension 
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddHttpClient();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IWeatherService, WeatherService>();

            return services;
        }
    }
}

