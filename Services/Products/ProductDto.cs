using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{


    //İki productu karşılaştırmak için record yapmalıyız. 12.video 
    public record ProductDto(int Id,string Name, decimal Price, int Stock);

    //{
    //    public int Id { get; init; } //Datada değişiklik yapılmasını engellemek içiçn set methotları yerine init yazdık.
    //    public string Name { get; init; }
    //    public decimal Price { get; init; }
    //    public int Stock { get; init; }
    //}
}
