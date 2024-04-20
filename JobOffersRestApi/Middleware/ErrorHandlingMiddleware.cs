using JobOffersRestApi.Exceptions;

namespace JobOffersRestApi.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public ErrorHandlingMiddleware(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFoundException.Message);
        }
        catch (BadHttpRequestException badHttpRequestException)
        {
            context.Response.StatusCode = badHttpRequestException.StatusCode;
            await context.Response.WriteAsync(badHttpRequestException.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            string message = _hostEnvironment.IsDevelopment() ? e.Message : "Something went wrong";
            await context.Response.WriteAsync(message);
        }
    }
}