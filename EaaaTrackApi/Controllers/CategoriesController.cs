using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EaaaTrackApi.Context;
using EaaaTrackApi.Models;

namespace EaaaTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SubscriptionContext _context;

        public CategoriesController(SubscriptionContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionCategory>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionCategory>> GetSubscriptionCategory(int id)
        {
            var subscriptionCategory = await _context.Categories.FindAsync(id);

            if (subscriptionCategory == null)
            {
                return NotFound();
            }

            return subscriptionCategory;
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<SubscriptionCategory>> PostSubscriptionCategory(string categoryName)
        {
            var subscriptionCategory = new SubscriptionCategory()
            {
                Name = categoryName
            };
            _context.Categories.Add(subscriptionCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscriptionCategory", new { id = subscriptionCategory.Id }, subscriptionCategory);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionCategory(int id)
        {
            var subscriptionCategory = await _context.Categories.FindAsync(id);
            if (subscriptionCategory == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(subscriptionCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionCategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
