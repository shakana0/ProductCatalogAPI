using System.Diagnostics;
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
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
        public async Task<ActionResult<PagedResult<ProductDto>>> GetAll(
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 20,
     [FromQuery] int? categoryId = null)
        {
            var sw = Stopwatch.StartNew();

            var query = new GetAllProductsQuery
            {
                Page = page,
                PageSize = pageSize,
                CategoryId = categoryId
            };

            var products = await _mediator.Send(query);

            sw.Stop();

            // Log structured info
            _logger.LogApiRequest(
                endpoint: "/products",
                query: $"page={page}&pageSize={pageSize}",
                statusCode: 200,
                responseTimeMs: sw.ElapsedMilliseconds,
                cacheStatus: "unknown",
                subscription: "unknown",
                resultCount: products.Items.Count()
            );

            return Ok(products);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductCommand command)
        {
            var product = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product
            );
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
                return BadRequest("Route id and body id must match.");

            var updatedProduct = await _mediator.Send(command);

            if (updatedProduct == null)
                return NotFound($"No Product found for Id: {id}");

            return Ok(updatedProduct);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (!result)
                return NotFound($"No Product found for Id: {id}");

            return NoContent();
        }

    }
}
