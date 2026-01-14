using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace App.Repositories.Extension
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services,IConfiguration configuration) //static void yerine IServiceCollection dönmesini istedik.
        {
            //Bu kod builder.Services de olsa Repositories ile ilgili bir kod olduğu için bu kodu extention method edip oraya taşımamız gerek. Bu yüzden repo kısmında eksik olan frameworkü  edit project file a tıklayarak App.Repositories e yazdık
            services.AddDbContext<AppDbContext>(options =>    //<AppDbContext> i ekledik.
            {
                var connectionStrings = 
                    configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();   //(ConnectionStringOption.Key).Get<ConnectionStringOption>();  böyle yazmamalıyız. Statik olarak bir sabitten gelmesi lazım. 

                options.UseSqlServer(connectionStrings!.SqlServer, sqlServerOptionsAction =>    //Datanın kesin olarak geleceğini ifade etmek için connectionStrings! yazdık. ! işareti bunu ifade ediyor.
                {
                    sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName); //Migration olarak DbCotext in olduğu yeri al
                });
                
            });


            services.AddScoped<IProductRepository, ProductRepository>();  //IProductRepository i gördüğün zaman ProductReository den nesen üretecek. Singleton yazmadık.
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;   //services döndersin.



        }
    }
}
