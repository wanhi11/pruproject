using GameAPI.Entities;
using GameAPI.Extensions;
using GameAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructure(builder.Configuration);


        builder.Services.AddCors(option =>
            option.AddPolicy("CORS", builder =>
                builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((host) => true)));

        var app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PrugameContext>();
                await dbContext.Database.MigrateAsync();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("CORS");

        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionMiddleware>();


        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}