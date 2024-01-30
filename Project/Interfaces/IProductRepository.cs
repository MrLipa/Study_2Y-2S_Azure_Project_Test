using Project.Models;

namespace Project.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetAllProducts();
        Product GetProductById(int productId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);
    }
}
