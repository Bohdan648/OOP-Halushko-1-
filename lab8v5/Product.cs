namespace lab8v5
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public Product(string name, string category, decimal price)
        {
            Name = name;
            Category = category;
            Price = price;
        }
    }
}