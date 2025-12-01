using EcommerceAPI.Domain;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
        {
            var categories = await _repository.GetAllAsync();

            var dtos = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });

            return Ok(dtos);
        }



        [HttpPost]
        public async Task<ActionResult<Category>> Create(CategoryRequestDto dto)
        {
            // Mappa DTO till entitet
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var created = await _repository.AddAsync(category);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            // placeholder – implementera GetByIdAsync i repository
            return Ok();
        }
    }
}
