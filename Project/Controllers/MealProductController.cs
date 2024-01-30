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
    public class MealProductController : ControllerBase
    {
        private readonly IMealProductRepository _repository;
        private readonly IMapper _mapper;

        public MealProductController(IMealProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{mealId}")]
        public ActionResult<IEnumerable<MealProductDto>> GetMealProducts(int mealId)
        {
            var mealProducts = _repository.GetMealProducts(mealId);
            var mealProductDtos = _mapper.Map<IEnumerable<MealProductDto>>(mealProducts);
            return Ok(mealProductDtos);
        }

        [HttpPost]
        public IActionResult CreateMealProduct([FromBody] MealProductDto createMealProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mealProduct = _mapper.Map<MealProduct>(createMealProductDto);
            _repository.AddMealProduct(mealProduct);

            return CreatedAtAction(nameof(GetMealProducts), new { mealId = mealProduct.MealId }, mealProduct);
        }

        [HttpPut("{mealId}/{productId}")]
        public IActionResult UpdateMealProduct(int mealId, int productId, [FromBody] MealProductDto updateMealProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mealProduct = _repository.FindMealProduct(mealId, productId);
            if (mealProduct == null)
            {
                return NotFound();
            }

            _mapper.Map(updateMealProductDto, mealProduct);
            _repository.UpdateMealProduct(mealProduct);

            return NoContent();
        }

        [HttpDelete("{mealId}/{productId}")]
        public IActionResult DeleteMealProduct(int mealId, int productId)
        {
            var mealProduct = _repository.GetMealProduct(mealId, productId);
            if (mealProduct == null)
            {
                return NotFound();
            }

            _repository.DeleteMealProduct(mealId, productId);
            return NoContent();
        }
    }
}
