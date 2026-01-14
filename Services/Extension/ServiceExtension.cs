using App.Repositories;
using App.Repositories.Products;
using App.Services.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Services.Extension
{
    public static class ServiceExtension //Extention method yazdığımız için sınıfımız static
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) //static void yerine IServiceCollection dönmesini istedik.
        {

            services.AddScoped<IProductService, ProductService>();

            
            
            return services;   //services döndersin.



        }

    }
}
