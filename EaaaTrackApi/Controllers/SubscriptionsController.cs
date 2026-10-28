using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EaaaTrackApi.Context;
using EaaaTrackApi.Dto;
using EaaaTrackApi.Models;

namespace EaaaTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionContext _context;

        public SubscriptionsController(SubscriptionContext context)
        {
            _context = context;
        }

        // GET: api/Subscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubScriptions()
        {
            return await _context.Subscriptions
                .Include(s => s.PaymentMethod)
                .Include(s => s.SubscriptionCategory)
                .ToListAsync();
        }

        // GET: api/Subscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var subscription = await _context.Subscriptions
                .Include(s => s.PaymentMethod)
                .Include(s => s.SubscriptionCategory)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // POST: api/Subscriptions
        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSubscription(SubscriptionDto subscription)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(subscription.PaymentMethodId);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(subscription.SubscriptionCategoryId);
            if (category == null)
            {
                return NotFound();
            }

            var nextBillingDate = FindNextBillingDate(subscription.StartDate, subscription.BillingCycle);
            var newSubscription = new Subscription
            {
                SubscriptionCategoryId = category.Id,
                SubscriptionCategory = category,
                PaymentMethodId = paymentMethod.Id,
                PaymentMethod = paymentMethod,
                Name = subscription.Name,
                Price = subscription.Price,
                BillingCycle = subscription.BillingCycle,
                NextBillingDate = nextBillingDate,
                Currency = subscription.Currency,
            };
            _context.Subscriptions.Add(newSubscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = newSubscription.Id }, newSubscription);
        }

        private DateOnly FindNextBillingDate(DateOnly startDate, Period billingCycle) =>
            billingCycle switch
            {
                Period.Monthly => startDate.AddMonths(1),
                Period.Quarterly => startDate.AddMonths(3),
                Period.Annual => startDate.AddMonths(12),
                _ => startDate
            };
        

        // DELETE: api/Subscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }
    }
}
