using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Helper;
using Project.Interfaces;
using Project.Models;

namespace Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserMealController : ControllerBase
    {
        private readonly IUserMealRepository _repository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMapper _mapper;
        private readonly EventGridHelper _eventGridHelper;

        public UserMealController(IUserMealRepository repository, IMapper mapper, IConfiguration configuration, IAppUserRepository appUserRepository, IMealRepository mealRepository)
        {
            _repository = repository;
            _appUserRepository = appUserRepository;
            _mealRepository = mealRepository;
            _mapper = mapper;
            _eventGridHelper = new EventGridHelper(configuration);
        }

        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<UserMealDto>> GetUserMeals(int userId)
        {
            var userMeals = _repository.GetUserMeals(userId);
            var userMealDtos = _mapper.Map<IEnumerable<UserMealDto>>(userMeals);
            return Ok(userMealDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserMeal([FromBody] UserMealDto createUserMealDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userMeal = _mapper.Map<UserMeal>(createUserMealDto);
            _repository.AddUserMeal(userMeal);

            var userId = createUserMealDto.UserId;
            var user = _appUserRepository.GetAppUserById(userId);
            if (user == null)
            {
                return NotFound($"Nie znaleziono użytkownika o ID {userId}.");
            }

            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(1).AddTicks(-1);
            var meals = _appUserRepository.GetMealsByUserAndDateRange(userId, startDate, endDate);
            if (meals == null || !meals.Any())
            {
                return NotFound($"Nie znaleziono posiłków dla użytkownika o ID {userId} w dzisiejszym dniu {startDate} - {endDate}.");
            }

            double totalCalories = 0;
            double totalProtein = 0;
            double totalFat = 0;
            double totalCarbohydrates = 0;
            double totalWeight = 0;
            foreach (var meal in meals)
            {
                var mealNutritionalSummary = _mealRepository.GetNutritionalSummaryForMeal(meal.Name);
                if (mealNutritionalSummary != null)
                {
                    totalCalories += mealNutritionalSummary.TotalCalories;
                    totalProtein += mealNutritionalSummary.TotalProtein;
                    totalFat += mealNutritionalSummary.TotalFat;
                    totalCarbohydrates += mealNutritionalSummary.TotalCarbohydrates;
                    totalWeight += mealNutritionalSummary.TotalWeight;
                }
            }

            string message = $"Czesc {user.Username},\n\n";
            bool exceeded = false;
            if (totalCalories > user.DailyCalorieGoal)
            {
                double excessCalories = totalCalories - (user.DailyCalorieGoal ?? 0);
                message += $"Przekroczyłes swój dzienny limit kalorii o {Math.Round(excessCalories, 2)} kalorii (Limit: {user.DailyCalorieGoal}, Zjedzone: {Math.Round(totalCalories, 2)}).\n";
                exceeded = true;
            }
            if (totalProtein > user.DailyProteinGoal)
            {
                double excessProtein = totalProtein - (user.DailyProteinGoal ?? 0);
                message += $"Przekroczyłeś swój dzienny limit białka o {Math.Round(excessProtein, 2)}g (Limit: {user.DailyProteinGoal}g, Zjedzone: {Math.Round(totalProtein, 2)}g).\n";
                exceeded = true;
            }
            if (totalFat > user.DailyFatGoal)
            {
                double excessFat = totalFat - (user.DailyFatGoal ?? 0);
                message += $"Przekroczyłeś swój dzienny limit tłuszczów o {Math.Round(excessFat, 2)}g (Limit: {user.DailyFatGoal}g, Zjedzone: {Math.Round(totalFat, 2)}g).\n";
                exceeded = true;
            }
            if (totalCarbohydrates > user.DailyCarbohydratesGoal)
            {
                double excessCarbohydrates = totalCarbohydrates - (user.DailyCarbohydratesGoal ?? 0);
                message += $"Przekroczyłeś swój dzienny limit węglowodanów o {Math.Round(excessCarbohydrates, 2)}g (Limit: {user.DailyCarbohydratesGoal}g, Zjedzone: {Math.Round(totalCarbohydrates, 2)}g).\n";
                exceeded = true;
            }
            if (exceeded)
            {
                var payload = new { email = user.Email, subject = "Przekroczony Limit", message = message };
                string subject = "ExceedLimit";
                string eventType = "Your.Fixed.EventType";
                await _eventGridHelper.SendEventToEventGrid(subject, eventType, payload);
            }
            return Ok(new { StatusMessage = "Posiłek zjedzony", DetailedMessage = message });
        }

        [HttpDelete("{userId}/{mealId}")]
        public IActionResult DeleteUserMeal(int userId, int mealId)
        {
            var userMeals = _repository.GetUserMeals(userId);
            var userMeal = userMeals.FirstOrDefault(um => um.UserId == userId && um.MealId == mealId);

            if (userMeal == null)
            {
                return NotFound();
            }

            _repository.DeleteUserMeal(userId, mealId);
            return NoContent();
        }
    }
}
