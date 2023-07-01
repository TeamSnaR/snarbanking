using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using static System.Net.Mime.MediaTypeNames;

namespace SnarBanking.Api.ExceptionHandling;

public static class ExceptionHandling
{
    public static IApplicationBuilder UseSnarBankingGlobalExceptionHandler(this IApplicationBuilder app) =>
        app
            .UseExceptionHandler(SnarBankingGlobalExceptionHandler);

    internal static Action<IApplicationBuilder> SnarBankingGlobalExceptionHandler => (appBuilder) => appBuilder
        .Run(async context =>
        {
            if (context.RequestServices.GetRequiredService<IProblemDetailsService>() is
                { } problemDetailsService)
            {
                var exceptionHandlerFeature =
                   context.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature is not null)
                {
                    await problemDetailsService.WriteAsync(exceptionHandlerFeature.Error.MapToStatusCode(context));
                }
            }
        });
    internal static ProblemDetailsContext MapToStatusCode(this Exception exception, HttpContext httpContext)
    {
        int statusCode = exception switch
        {
            NotImplementedException => StatusCodes.Status501NotImplemented,
            NullReferenceException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = Text.Plain;
        return new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Title = ReasonPhrases.GetReasonPhrase(statusCode),
                Detail = exception.StackTrace,
                Type = $"https://httpstatuses.io/{statusCode}",
                Status = statusCode
            }
        };
    }
}
