namespace Project.Models
{
    public class UserMealDto
    {
        public int UserId { get; set; }
        public int MealId { get; set; }
        public DateTime ConsumedAt { get; set; }
    }
}
