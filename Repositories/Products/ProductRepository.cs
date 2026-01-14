using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Products
{
    internal class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository //constructlar miras yolu ile gelmez. bu nedenle bu clasın constructor yolu ile aldıığımız datayı mmiras aldığımız sınıfın constructoruna gönderiyoruz.
    {
        public Task<List<Product>>GetTopPriceProductAsync(int count)
        {
            return Context.Products.OrderByDescending(x=> x.Price).Take(count).ToListAsync();
        }
    }
}
