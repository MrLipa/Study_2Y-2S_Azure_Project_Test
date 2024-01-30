using Project.Data;
using Project.Interfaces;
using Project.Models;


namespace Project.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly DataContext _context;

        public MealRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Meal> GetAllMeals()
        {
            return _context.Meals.ToList();
        }

        public Meal GetMealById(int mealId)
        {
            var meal = _context.Meals.Find(mealId);
            if (meal == null)
            {
                throw new Exception("Meal not found.");
            }

            return meal;
        }

        public void AddMeal(Meal meal)
        {
            _context.Meals.Add(meal);
            _context.SaveChanges();
        }

        public void UpdateMeal(Meal meal)
        {
            _context.Meals.Update(meal);
            _context.SaveChanges();
        }

        public void DeleteMeal(int mealId)
        {
            var meal = _context.Meals.Find(mealId);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
                _context.SaveChanges();
            }
        }
        public ICollection<ProductInfo> GetProductsForMeal(string mealName)
        {
            var products = _context.MealProducts
                .Where(mp => mp.Meal.Name == mealName)
                .Select(mp => new ProductInfo
                {
                    ProductName = mp.Product.Name,
                    Calories = mp.Product.Calories,
                    Protein = mp.Product.Protein,
                    Fat = mp.Product.Fat,
                    Carbohydrates = mp.Product.Carbohydrates,
                    QuantityInGrams = mp.QuantityInGrams
                })
                .ToList();

            return products;
        }

        public MealNutritionalSummary GetNutritionalSummaryForMeal(string mealName)
        {
            var summary = _context.MealProducts
                .Where(mp => mp.Meal.Name == mealName)
                .GroupBy(mp => mp.Meal.Name)
                .Select(g => new MealNutritionalSummary
                {
                    MealName = g.Key,
                    TotalCalories = g.Sum(mp => mp.Product.Calories * mp.QuantityInGrams / 100),
                    TotalProtein = g.Sum(mp => mp.Product.Protein * mp.QuantityInGrams / 100),
                    TotalFat = g.Sum(mp => mp.Product.Fat * mp.QuantityInGrams / 100),
                    TotalCarbohydrates = g.Sum(mp => mp.Product.Carbohydrates * mp.QuantityInGrams / 100),
                    TotalWeight = g.Sum(mp => mp.QuantityInGrams)
                })
                .FirstOrDefault();

            return summary ?? new MealNutritionalSummary();
        }
    }
}