using System.Text.Json;
using API.Middleware;
using DataAccess;
using DataAccess.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service;
using Service.Validators;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptionsWithValidateOnStart<AppOptions>()
            .Bind(builder.Configuration.GetSection(nameof(AppOptions)))
            .ValidateDataAnnotations()
            .Validate(options => new AppOptionsValidator().Validate(options).IsValid,
                $"{nameof(AppOptions)} validation failed");
        builder.Services.AddDbContext<DMShopContext>((serviceProvider, options) =>
        {
            var appOptions = serviceProvider.GetRequiredService<IOptions<AppOptions>>().Value;
            options.UseNpgsql(Environment.GetEnvironmentVariable("DbConnectionString") 
                              ?? appOptions.DbConnectionString);
            Console.WriteLine(appOptions);
            options.EnableSensitiveDataLogging();
        });
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateOrderStatusValidator>());
        builder.Services.AddScoped<IDMShopRepository, DMShopRepository>();
        builder.Services.AddScoped<IDMShopService, DMShopService>();
        
        builder.Services.AddControllers();
        builder.Services.AddOpenApiDocument();

        var app = builder.Build();

        var options = app.Services.GetRequiredService<IOptions<AppOptions>>().Value;
        Console.WriteLine("APP OPTIONS: " + JsonSerializer.Serialize(options));

        app.UseHttpsRedirection();

        app.UseRouting();


        app.UseOpenApi();
        app.UseSwaggerUi();
        app.UseStatusCodePages();

        app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        app.UseMiddleware<RequestResponseLoggingMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DMShopContext>();
            context.Database.EnsureCreated();
        }

        app.Run();
    }
    
}