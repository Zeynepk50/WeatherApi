using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace App.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) //Miras aldığımız sınıfın constructoruna bu optionsu gönderiyoruz. API tarafındaki AppSettings te tanımladığımız ConnectionString i buraya geçeceğiz çünkü.
    {
        public DbSet<Product> Products { get; set; } = default!; //boş olmayacak diyoruz.



        //product u configure etmek istiyoruz. Normalde OnModelCreate methodunu ezersek yani override yaparsak konfigüre edebiliriz.
        //Ancak DbContext ana nesnelerimizden biri olduğu için bunu farklı sınıfta yapmalıyyız. Burayı çok doldurmamak için.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //bu repositoriy i kastederek bu repository içerisindeki implement edilen tüm sınıfları al diyoruz.

            base.OnModelCreating(modelBuilder);
        }


    }
}
