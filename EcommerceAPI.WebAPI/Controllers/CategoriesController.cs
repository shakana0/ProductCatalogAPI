using EcommerceAPI.Application.Categories.Dtos;
using EcommerceAPI.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (category == null)
                return NotFound($"No Category Found For Id: {id}");

            return Ok(category);
        }
    }
}
