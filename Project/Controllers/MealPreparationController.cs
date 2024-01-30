using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using Project.Models;
using Project.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MealPreparationController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IProductRepository _productRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMealProductRepository _mealProductRepository;
        private readonly IConfiguration _configuration;


        public MealPreparationController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IProductRepository productRepository, IMealRepository mealRepository, IMealProductRepository mealProductRepository)
        {
            _httpClient = httpClientFactory.CreateClient();
            _productRepository = productRepository;
            _mealRepository = mealRepository;
            _mealProductRepository = mealProductRepository;
            _configuration = configuration;
        }

        [HttpPost("prepare")]
        public async Task<IActionResult> PrepareMeal(MealPreparationRequest request)
        {
            var products = request.ProductIds
                .Select(id => _productRepository.GetProductById(id))
                .Where(p => p != null)
                .ToList();

            var payload = new { products, limits = request.Limits };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var url = _configuration["ExternalApi:BaseUrl"];

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var productQuantities = JsonConvert.DeserializeObject<List<MealProductInfo>>(responseContent);

                var meal = new Meal { Name = request.MealName };
                _mealRepository.AddMeal(meal);

                foreach (var productQuantity in productQuantities)
                {
                    var mealProduct = new MealProduct
                    {
                        MealId = meal.MealId,
                        ProductId = productQuantity.ProductId,
                        QuantityInGrams = productQuantity.QuantityInGrams
                    };
                    _mealProductRepository.AddMealProduct(mealProduct);
                }

                return Ok("Posiłek został przygotowany pomyślnie.");
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error calling the external API");
            }
        }
    }
}
