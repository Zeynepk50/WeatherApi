using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace App.Repositories.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{ //Uyarı kısmından implement interface diyerek config ekledik
     public void Configure(EntityTypeBuilder<Product> builder)
     {
         //id yi primary key olarak atama
         builder.HasKey(x => x.Id);
         builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
         builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)"); //virgülün solunda ve sağında toplam max. 18 basamak olabilir. Virgülden sonra max. 2 basamak olabilir.
         builder.Property(x => x.Stock).IsRequired();       //mutlaka değer olmalı. Null olamaz.
     }
}

