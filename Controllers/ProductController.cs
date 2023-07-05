using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDBContext _dbContext;

        public ProductController(ProductDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            if (_dbContext.Product == null)
            {
                return NotFound();
            }
            return await _dbContext.Product.ToListAsync();
        }


        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            if (_dbContext.Product == null)
            {
                return NotFound();
            }
            var prod = await _dbContext.Product.FindAsync(id);

            if (prod == null)
            {
                return NotFound();
            }

            return prod;
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            if (_dbContext.Product == null)
            {
                return Problem("Entity is null.");
            }
            _dbContext.Product.Add(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = product.Id }, product);
        }


        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(product).State = EntityState.Modified;

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

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_dbContext.Product == null)
            {
                return NotFound();
            }
            var product = await _dbContext.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return (_dbContext.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
