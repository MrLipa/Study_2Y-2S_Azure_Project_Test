using Project.Models;

namespace Project.Interfaces
{
    public interface IMealRepository
    {
        ICollection<Meal> GetAllMeals();
        Meal GetMealById(int mealId);
        void AddMeal(Meal meal);
        void UpdateMeal(Meal meal);
        void DeleteMeal(int mealId);

        ICollection<ProductInfo> GetProductsForMeal(string mealName);
        MealNutritionalSummary GetNutritionalSummaryForMeal(string mealName);
    }
}
