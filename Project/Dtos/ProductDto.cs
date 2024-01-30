namespace Project.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbohydrates { get; set; }
    }
}
