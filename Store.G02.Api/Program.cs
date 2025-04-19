
using Domain.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Presistence;
using Presistence.Data;
using Services;
using Services.Abstractions;
using Shared.ErrorsModels;
using Store.G02.Api.Extensions;
using Store.G02.Api.Middlewares;


namespace Store.G02.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.RegisterAllServices(builder.Configuration);



            var app = builder.Build();



            await app.ConfigureMiddlewares();


            app.Run();
        }
    }
}
