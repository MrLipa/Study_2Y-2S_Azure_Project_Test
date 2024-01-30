namespace Project.Models
{
    public class UserMeal
    {
        public int UserMealId { get; set; }
        public int UserId { get; set; }
        public int MealId { get; set; }
        public DateTime ConsumedAt { get; set; }

        public AppUser AppUser { get; set; }
        public Meal Meal { get; set; }
    }
}
