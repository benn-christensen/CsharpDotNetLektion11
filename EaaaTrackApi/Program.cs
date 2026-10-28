using System.Text.Json.Serialization;
using EaaaTrackApi.Context;
using EaaaTrackApi.Models;
using Scalar.AspNetCore;

namespace EaaaTrackApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
        // Add services to the container.

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                var enumConverter = new JsonStringEnumConverter<Period>();
                options.JsonSerializerOptions.Converters.Add(enumConverter);
            });
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.AddNpgsqlDbContext<SubscriptionContext>("subscriptiondb");
        
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        app.MapDefaultEndpoints();
        app.Run();
    }
}