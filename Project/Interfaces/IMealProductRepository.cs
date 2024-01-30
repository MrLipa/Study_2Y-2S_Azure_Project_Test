using Project.Models;

namespace Project.Interfaces
{
    public interface IMealProductRepository
    {
        ICollection<MealProduct> GetMealProducts(int mealId);
        MealProduct GetMealProduct(int mealId, int productId);
        void AddMealProduct(MealProduct mealProduct);
        void DeleteMealProduct(int mealId, int productId);
        MealProduct FindMealProduct(int mealId, int productId);
        void UpdateMealProduct(MealProduct mealProduct);
    }
}
