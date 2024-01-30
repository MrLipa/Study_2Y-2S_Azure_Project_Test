using Project.Models;

namespace Project.Interfaces
{
    public interface IUserMealRepository
    {
        ICollection<UserMeal> GetUserMeals(int userId);
        void AddUserMeal(UserMeal userMeal);
        void DeleteUserMeal(int userId, int mealId);
    }
}
