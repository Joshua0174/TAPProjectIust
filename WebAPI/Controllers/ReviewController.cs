using BusinessLayer.Contracts;
using BusinessLayer.Dto.ReviewDto;
using BusinessLayer.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost("{productId:Guid}")]
        public IActionResult Create([FromRoute] Guid productId, CreateReviewDto reviewDto) {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //var review = reviewDto.ToReviewFromCreateDto();
            _reviewService.Create(productId, reviewDto);
            return Ok(reviewDto);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpDelete("{id:Guid}")]
        public IActionResult Delete(Guid id) { 
            _reviewService.Delete(id);
            return Ok();
        }
    }
}
