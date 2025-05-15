using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Web.ViewModels.Error;

namespace CandidateManagement.Web.Middleware
{
    /// <summary>
    /// Middleware encargado de capturar excepciones no controladas en la aplicación,
    /// registrar el error y devolver una respuesta HTML personalizada que muestra
    /// información detallada del error en ambiente de desarrollo.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        /// <summary>
        /// Constructor que inyecta las dependencias necesarias para el manejo de errores,
        /// renderizado de vistas y acceso al ambiente de ejecución.
        /// </summary>
        /// <param name="next">Delegate del siguiente middleware en la cadena.</param>
        /// <param name="logger">Logger para registrar excepciones y eventos.</param>
        /// <param name="env">Proporciona información sobre el entorno actual (desarrollo, producción, etc.).</param>
        /// <param name="viewEngine">Motor Razor para renderizar vistas a string.</param>
        /// <param name="tempDataDictionaryFactory">Fábrica para crear diccionarios TempData si se requieren.</param>
        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IHostEnvironment env,
            IRazorViewEngine viewEngine,
            ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _viewEngine = viewEngine;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        /// <summary>
        /// Método principal que intercepta la ejecución de la pipeline de middleware.
        /// Intenta continuar la ejecución normal y captura cualquier excepción no manejada.
        /// </summary>
        /// <param name="context">Contexto HTTP actual.</param>
        /// <returns>Una tarea asíncrona.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Ejecuta el siguiente middleware en la cadena
                await _next(context);
            }
            catch (Exception ex)
            {
                // Loguea la excepción para diagnóstico
                _logger.LogError(ex, "Excepción no controlada");

                // Configura la respuesta HTTP con código 500 y contenido HTML
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/html";

                // Crea el modelo que se pasará a la vista, incluyendo detalles
                // solo si estamos en ambiente de desarrollo para evitar filtraciones en producción
                var model = new ErrorViewModel
                {
                    Message = ex.Message,
                    StackTrace = _env.IsDevelopment() ? ex.StackTrace : null,
                    IsDevelopment = _env.IsDevelopment()
                };

                // Renderiza la vista de error como string HTML usando Razor
                var html = await RenderViewToStringAsync(context, "/Views/Home/Error.cshtml", model);

                // Escribe el HTML renderizado en la respuesta HTTP
                await context.Response.WriteAsync(html);
            }
        }

        /// <summary>
        /// Método auxiliar para renderizar una vista Razor a una cadena HTML.
        /// Utiliza el motor Razor y construye un ActionContext adecuado.
        /// </summary>
        /// <param name="context">Contexto HTTP actual.</param>
        /// <param name="viewPath">Ruta completa a la vista Razor que se desea renderizar.</param>
        /// <param name="model">Modelo que será pasado a la vista.</param>
        /// <returns>String con el contenido HTML renderizado de la vista.</returns>
        private async Task<string> RenderViewToStringAsync(HttpContext context, string viewPath, object model)
        {
            // Construye un ActionContext necesario para renderizar la vista fuera del flujo MVC estándar
            var actionContext = new ActionContext(
                context,
                context.GetRouteData() ?? new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            using var sw = new StringWriter();

            // Obtiene la vista Razor desde la ruta especificada
            var viewResult = _viewEngine.GetView(executingFilePath: null, viewPath, isMainPage: true);

            if (!viewResult.Success)
                throw new InvalidOperationException($"No se encontró la vista '{viewPath}'.");

            // Prepara el ViewData con el modelo proporcionado
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            // Obtiene TempData para el contexto actual (aunque no se usa en este caso)
            var tempData = _tempDataDictionaryFactory.GetTempData(context);

            // Crea el ViewContext que encapsula todos los datos necesarios para renderizar la vista
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                tempData,
                sw,
                new HtmlHelperOptions()
            );

            // Renderiza la vista de forma asíncrona dentro del ViewContext
            await viewResult.View.RenderAsync(viewContext);

            // Devuelve el contenido HTML generado como string
            return sw.ToString();
        }
    }
}
