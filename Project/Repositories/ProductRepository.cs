using Project.Data;
using Project.Interfaces;
using Project.Models;


namespace Project.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            return product;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
