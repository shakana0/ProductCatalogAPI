namespace EcommerceAPI.WebAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (FluentValidation.ValidationException ex)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    var errors = ex.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    });

                    var response = new
                    {
                        title = "Validation Failed",
                        status = 400,
                        errors
                    };

                    await context.Response.WriteAsJsonAsync(response);
                }
            });
        }
    }
}
