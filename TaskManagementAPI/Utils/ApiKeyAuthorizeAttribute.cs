using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagementAPI.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "API Key was not provided."
                };
                return;
            }

            var apiKey = configuration?["Security:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 500,
                    Content = "API Key is not configured."
                };
                return;
            }

            if (!string.Equals(apiKey, extractedApiKey.ToString(), StringComparison.Ordinal))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Unauthorized API Key."
                };
                return;
            }
        }
    }
}