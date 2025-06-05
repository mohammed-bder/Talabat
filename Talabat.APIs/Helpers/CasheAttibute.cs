using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
    public class CasheAttibute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSecond;

        public CasheAttibute(int TimeToLiveInSecond)
        {
            _timeToLiveInSecond = TimeToLiveInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // i want to ask CLR to Creating object that implement interface of "ICasheService" Explicitly
            var responseCasheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCasheService>();

            var casheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);

            var response = await responseCasheService.GetResponseCasheAsync(casheKey);

            if(!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;
                return;
            }

            var actionExecutedContext = await next.Invoke();   
            
            if(actionExecutedContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
                await responseCasheService.SetResponseCasheAsync(casheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSecond));
            }
        }

        private string GenerateCasheKeyFromRequest(HttpRequest request)
        {
            // {{BaseUrl}}/api/Products?pageIndex=1&pageSize=5&sort=name
            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path); // /api/Products

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");

                // /api/Products|pageIndex-1
                // /api/Products|pageIndex-1|pageSize-5
                // /api/Products|pageIndex-1|pageSize-5|sort-name
            }

            return keyBuilder.ToString();
        }
    }
}
