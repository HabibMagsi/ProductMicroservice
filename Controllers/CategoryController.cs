using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Models;

namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ProductDBContext _dbContext;

        public CategoryController(ProductDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            if (_dbContext.Category == null)
            {
                return NotFound();
            }
            return await _dbContext.Category.ToListAsync();
        }


        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            if (_dbContext.Category == null)
            {
                return NotFound();
            }
            var prod = await _dbContext.Category.FindAsync(id);

            if (prod == null)
            {
                return NotFound();
            }

            return prod;
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<ActionResult<Category>> Post(Category category)
        {
            if (_dbContext.Category == null)
            {
                return Problem("Entity is null.");
            }
            _dbContext.Category.Add(category);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = category.Id }, category);
        }


        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(category).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_dbContext.Category == null)
            {
                return NotFound();
            }
            var Category = await _dbContext.Category.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }

            _dbContext.Category.Remove(Category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return (_dbContext.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
