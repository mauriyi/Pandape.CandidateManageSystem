using System.Net;
using System.Text.Json;

namespace Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (IsHtmlRequest(context))
                {
                    // Para solicitudes HTML, redirige a la página de error 
                    context.Response.Redirect("/Home/Error");
                }
                else
                {
                    // Para API responde con JSON
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Se ha producido un error interno. Por favor, contacte al administrador.",
                        // Solo incluir detalles en entorno de desarrollo o testing
                        Detailed = (_env.IsDevelopment() || _env.IsEnvironment("Testing")) ? ex.Message : null
                    };

                    var options = new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
                    var json = JsonSerializer.Serialize(response, options);

                    await context.Response.WriteAsync(json);
                }
            }
        }

        private bool IsHtmlRequest(HttpContext context)
        {
            var acceptHeader = context.Request.Headers["Accept"].ToString();
            return acceptHeader.Contains("text/html");
        }
    }
}
