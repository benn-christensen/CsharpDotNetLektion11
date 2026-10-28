using System.Text.Json.Serialization;
using EaaaTrackApi.JsonConverters;
using EaaaTrackApi.Models;

namespace EaaaTrackApi.Dto;

public class SubscriptionDto
{
    public int SubscriptionCategoryId { get; set; }
    public int PaymentMethodId { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public Period BillingCycle { get; set; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly StartDate  { get; set; }
    public required string Currency  { get; set; }
}
