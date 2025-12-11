using EcommerceAPI.Application.Products.Commands.CreateProduct;
using EcommerceAPI.Application.Products.Commands.DeleteProduct;
using EcommerceAPI.Application.Products.Commands.UpdateProduct;
using EcommerceAPI.Application.Products.Dtos;
using EcommerceAPI.Application.Products.Queries.GetProductById;
using EcommerceAPI.Application.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));

            if (product == null)
                return NotFound($"No Product found for Id: {id}");

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductCommand command)
        {
            var product = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product
            );
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
                return BadRequest("Route id and body id must match.");

            var updatedProduct = await _mediator.Send(command);

            if (updatedProduct == null)
                return NotFound($"No Product found for Id: {id}");

            return Ok(updatedProduct);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (!result)
                return NotFound($"No Product found for Id: {id}");

            return NoContent();
        }


    }
}
