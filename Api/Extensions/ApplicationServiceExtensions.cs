using System;
using Api.Data;
using Api.Interfaces;
using Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddCors();
        services.AddDbContext<DataContext>(options => options.UseSqlite(config.GetConnectionString("DefaultConnectionString")));
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
