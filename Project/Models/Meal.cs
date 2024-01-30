namespace Project.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        public string Name { get; set; }

        public ICollection<UserMeal> UserMeals { get; set; }
        public ICollection<MealProduct> MealProducts { get; set; }
    }
}
