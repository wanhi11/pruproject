using AutoMapper;
using GameAPI.Dtos;
using GameAPI.Entities;
using GameAPI.Mapper;
//using GameAPI.Mapper;
using GameAPI.Middlewares;
using GameAPI.Repositories;
using GameAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace GameAPI.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ExceptionMiddleware>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<PrugameContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ApplicationMapper());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
        
        services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<UserService>();
        services.AddScoped<LeaderBoardService>();
        
        return services;
    }
}