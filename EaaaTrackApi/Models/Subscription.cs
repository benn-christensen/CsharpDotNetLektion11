namespace EaaaTrackApi.Models;

public class Subscription
{
    public int Id { get; set; }
    public int SubscriptionCategoryId { get; set; }
    public required SubscriptionCategory SubscriptionCategory { get; set; }
    public int PaymentMethodId { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public Period BillingCycle { get; set; }
    public DateOnly NextBillingDate { get; set; }
    public string Currency { get; set; } = "DDK";
}
