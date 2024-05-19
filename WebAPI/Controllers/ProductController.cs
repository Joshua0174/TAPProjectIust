using BusinessLayer.Contracts;
using BusinessLayer.Dto.ProductDto;
using BusinessLayer.Helpers;
using BusinessLayer.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllProducts")]
        public IActionResult Get([FromQuery]QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = _productService.GetAll(query);
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {  
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound("Not found");
            }
            return Ok(product);
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return BadRequest();
            }
            _productService.Delete(id);
            return Ok(product);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto productDto)
        {   
            if(!ModelState.IsValid) {
                return BadRequest();
            }
            var product = productDto.ToProductFromCreateDto();
            _productService.Create(product);
            return Ok(product);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{productId:guid}")]
        public IActionResult Update([FromRoute] Guid productId, [FromBody] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
             var product= productDto.ToProductFromUpdateDto();
             var updatedProduct= _productService.Update(productId,product);
             if (updatedProduct == null) {
                return NotFound();
             }
             return Ok(updatedProduct);
        }

        
    }
}

