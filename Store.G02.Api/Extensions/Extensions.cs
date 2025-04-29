
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorsModels;
using Services;
using Presistence;
using Domain.Contracts;
using Store.G02.Api.Middlewares;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Presistence.Identity;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Store.G02.Api.Extensions
{
    public static class Extensions
    {
       

        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration) 
        {

            services.AddBuiltServices();
            services.AddSwaggerServices();
            services.ConfigureServices();
            

            services.AddInfrastructureServices(configuration);

            services.AddIdentityServices();

            services.AddApplicationServices(configuration);

            services.ConfigureJwtServices(configuration);

         
            return services;
        }

        private static IServiceCollection AddBuiltServices(this IServiceCollection services)
        {


            services.AddControllers();


            return services;
        }

        private static IServiceCollection ConfigureJwtServices(this IServiceCollection services, IConfiguration configuration)
        {


            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,


                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))


                };
            });



            return services;
        }



        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {


            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();


            return services;
        }


        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();



            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {



            services.Configure<ApiBehaviorOptions>(config =>
            {

                config.InvalidModelStateResponseFactory = (actionContext) =>
                {

                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                 .Select(m => new ValidationError()
                                 {
                                     Field = m.Key,
                                     Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                 });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };


                    return new BadRequestObjectResult(response);
                };

            });


            return services;
        }



        public static async Task<WebApplication>  ConfigureMiddlewares(this WebApplication app) 
        { 


            await app.InitializeDatabaseAsync();

           app.UseGlobalErrorHanding();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            return app;

        }



        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {


            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // ASK CLR Create Object From DbInitializer
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();


            return app;

        }

        private static WebApplication UseGlobalErrorHanding(this WebApplication app)
        {

            app.UseMiddleware<GlobalErrorHandingMiddleware>();

            return app;

        }

    }
}
