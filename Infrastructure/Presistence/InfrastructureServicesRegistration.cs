using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presistence.Data;
using Presistence.Identity;
using Presistence.Repositories;
using StackExchange.Redis;

namespace Presistence
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) 
        {


            services.AddDbContext<StoreDbContext>(Options =>
            {

                //Options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                Options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(Options =>
            {

                Options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddScoped<IDbInitializer, DbInitializer>(); // Allow DI For DbInitializer

            services.AddScoped<IUnitofwork, Unitofwork>();

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped<ICasheRepository, CasheRepository>();

            services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) => 
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
            });

            return services;
        }
    }
}
