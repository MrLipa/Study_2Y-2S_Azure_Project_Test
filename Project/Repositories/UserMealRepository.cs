using Project.Data;
using Project.Interfaces;
using Project.Models;


namespace Project.Repositories
{
    public class UserMealRepository : IUserMealRepository
    {
        private readonly DataContext _context;

        public UserMealRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<UserMeal> GetUserMeals(int userId)
        {
            return _context.UserMeals
                           .Where(um => um.UserId == userId)
                           .ToList();
        }

        public void AddUserMeal(UserMeal userMeal)
        {
            _context.UserMeals.Add(userMeal);
            _context.SaveChanges();
        }

        public void DeleteUserMeal(int userId, int mealId)
        {
            var userMeal = _context.UserMeals
                                   .FirstOrDefault(um => um.UserId == userId && um.MealId == mealId);
            if (userMeal != null)
            {
                _context.UserMeals.Remove(userMeal);
                _context.SaveChanges();
            }
        }
    }
}
