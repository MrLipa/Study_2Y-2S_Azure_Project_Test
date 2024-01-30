using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using Project.Models;


namespace Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealRepository _repository;
        private readonly IMapper _mapper;

        public MealController(IMealRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MealDto>> GetAllMeals()
        {
            var meals = _repository.GetAllMeals();
            var mealDtos = _mapper.Map<IEnumerable<MealDto>>(meals);
            return Ok(mealDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<MealDto> GetMealById(int id)
        {
            var meal = _repository.GetMealById(id);
            if (meal == null)
            {
                return NotFound();
            }

            var mealDto = _mapper.Map<MealDto>(meal);
            return Ok(mealDto);
        }

        [HttpPost]
        public IActionResult CreateMeal([FromBody] MealDto createMealDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meal = _mapper.Map<Meal>(createMealDto);
            _repository.AddMeal(meal);

            return CreatedAtAction(nameof(GetMealById), new { id = meal.MealId }, meal);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMeal(int id, [FromBody] MealDto updateMealDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meal = _repository.GetMealById(id);
            if (meal == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMealDto, meal);
            _repository.UpdateMeal(meal);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMeal(int id)
        {
            var meal = _repository.GetMealById(id);
            if (meal == null)
            {
                return NotFound();
            }

            _repository.DeleteMeal(id);
            return NoContent();
        }

        [HttpGet("ProductsForMeal/{mealName}")]
        public ActionResult<IEnumerable<ProductInfo>> GetProductsForMeal(string mealName)
        {
            var products = _repository.GetProductsForMeal(mealName);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("GetNutritionalSummary/{mealName}")]
        public ActionResult GetNutritionalSummary(string mealName)
        {
            var nutritionalSummary = _repository.GetNutritionalSummaryForMeal(mealName);

            if (nutritionalSummary == null)
            {
                return NotFound($"Nie znaleziono posiłku o nazwie {mealName}.");
            }

            return Ok(nutritionalSummary);
        }
    }
}
