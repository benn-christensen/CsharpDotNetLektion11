namespace EaaaTrackApi.Models;

public record Subscription(
    int Id, 
    int CategoryId, 
    int PaymentMethodId,
    string Name, 
    string Category, 
    decimal Price,
    Period BillingCycle,
    DateOnly NextBillingDate,
    Status Status,
    string Currency = "DDK");