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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = _repository.GetAllProducts();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProductById(int id)
        {
            var product = _repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(createProductDto);
            _repository.AddProduct(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(updateProductDto, product);
            _repository.UpdateProduct(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            _repository.DeleteProduct(id);
            return NoContent();
        }
    }
}
