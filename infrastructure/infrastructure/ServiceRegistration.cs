using CoreLibrary.Interface.Repositories;
using Domain.Querys.Interface;
using Infraestructure.Queries;
using infrastructure.Repositories;
using infrastructure.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("Missing DefaultConnection");

        services.AddDbContext<ServiceContext>(options =>
            options.UseNpgsql(conn, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));

        return services;
    }

    public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("Missing DefaultConnection");

        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(conn));

        services.AddScoped<IQueryService, QueryService>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }
}