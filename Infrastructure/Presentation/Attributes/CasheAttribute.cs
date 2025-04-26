using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Presentation.Attributes
{
    public class CasheAttribute(int durationInSec) : Attribute, IAsyncActionFilter
    {
      
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var casheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CasheService;

            var casheKey = GenerateCasheKey(context.HttpContext.Request);

            var result = await casheService.GetCasheValueAsync(casheKey);
            if (!string.IsNullOrEmpty(result)) 
            {
                // Return Response
                context.Result = new ContentResult() 
                {
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = result

                };

                return;
            }

            // Execute The Endpoint
            var contextResult = await next.Invoke();
            if (contextResult.Result is OkObjectResult okObject) 
            {
                await casheService.SetCasheValueAsync(casheKey, okObject.Value, TimeSpan.FromSeconds(durationInSec));

            }
        
        
        }


        private string GenerateCasheKey(HttpRequest request) 
        {
            var Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                Key.Append($"|{item.Key} - {item.Value}");

            }

            return Key.ToString();

        }



    }
}
