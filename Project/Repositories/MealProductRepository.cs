using Project.Data;
using Project.Interfaces;
using Project.Models;


namespace Project.Repositories
{
    public class MealProductRepository : IMealProductRepository
    {
        private readonly DataContext _context;

        public MealProductRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<MealProduct> GetMealProducts(int mealId)
        {
            return _context.MealProducts
                           .Where(mp => mp.MealId == mealId)
                           .ToList();
        }

        public MealProduct GetMealProduct(int mealId, int productId)
        {
            var mealProduct = _context.MealProducts
                                      .FirstOrDefault(mp => mp.MealId == mealId && mp.ProductId == productId);

            return mealProduct ?? new MealProduct();
        }

        public void AddMealProduct(MealProduct mealProduct)
        {
            _context.MealProducts.Add(mealProduct);
            _context.SaveChanges();
        }

        public void DeleteMealProduct(int mealId, int productId)
        {
            var mealProduct = _context.MealProducts
                                     .FirstOrDefault(mp => mp.MealId == mealId && mp.ProductId == productId);
            if (mealProduct != null)
            {
                _context.MealProducts.Remove(mealProduct);
                _context.SaveChanges();
            }
        }

        public MealProduct FindMealProduct(int mealId, int productId)
        {
            var mealProduct = _context.MealProducts.SingleOrDefault(mp => mp.MealId == mealId && mp.ProductId == productId);

            if (mealProduct == null)
            {
                throw new KeyNotFoundException("MealProduct not found.");
            }

            return mealProduct;
        }

        public void UpdateMealProduct(MealProduct mealProduct)
        {
            _context.MealProducts.Update(mealProduct);
            _context.SaveChanges();
        }
    }
}
