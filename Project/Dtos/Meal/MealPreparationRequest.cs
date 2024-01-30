using Project.Models;

namespace Project.Models
{
    public class MealPreparationRequest
    {
        public string MealName { get; set; }
        public List<int> ProductIds { get; set; }
        public NutritionalLimits Limits { get; set; }
    }
}
