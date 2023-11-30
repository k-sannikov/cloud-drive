using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CloudDrive.Auth;

public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Api Key missing");
            return;
        }

        IConfiguration configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        List<string> apiKeys = configuration.GetSection("Auth:ApiKeys").Get<List<string>>();

        if (!apiKeys.Contains(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid Api Key ");
            return;
        }
    }
}
