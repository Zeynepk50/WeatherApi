namespace App.Repositories.Products
{
    public class Product
    {
        public  int Id { get; set; }
        public string Name { get; set; } = default!;  //String in null değer alıp alamayacağını belirtmen lazım.
        public  decimal  Price { get; set; }   //Virgülden sonra kaç basamak olacağı DBcontext te
        public int Stock { get; set; }
    }
}
