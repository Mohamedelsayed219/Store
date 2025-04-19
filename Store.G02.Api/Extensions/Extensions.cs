
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorsModels;
using Services;
using Presistence;
using Domain.Contracts;
using Store.G02.Api.Middlewares;

namespace Store.G02.Api.Extensions
{
    public static class Extensions
    {
       

        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration) 
        {

            services.AddBuiltServices();
            services.AddSwaggerServices();
            

            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();

            services.ConfigureServices();


            return services;
        }

        private static IServiceCollection AddBuiltServices(this IServiceCollection services)
        {


            services.AddControllers();


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

            app.UseAuthorization();


            app.MapControllers();

            return app;

        }



        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {


            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // ASK CLR Create Object From DbInitializer
            await dbInitializer.InitializeAsync();


            return app;

        }

        private static WebApplication UseGlobalErrorHanding(this WebApplication app)
        {

            app.UseMiddleware<GlobalErrorHandingMiddleware>();

            return app;

        }

    }
}
