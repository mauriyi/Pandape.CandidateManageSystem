using Application.Features.Candidates.Commands.CreateCandidate;
using Application.Features.Interfaces;
using CandidateManagement.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// CONFIGURACIÓN DE SERVICIOS 
// -----------------------------------------------------------------------------

// Agrega soporte a controladores con vistas 
builder.Services.AddControllersWithViews();

// Registro de MediatR para manejo de comandos y queries
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCandidateCommandHandler).Assembly));

// Configuración del DbContext usando una base de datos en memoria 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("CandidateDb"));

// Registro del repositorio de candidatos mediante la interfaz
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

var app = builder.Build();

// -----------------------------------------------------------------------------
// CONFIGURACIÓN DEL PIPELINE HTTP
// -----------------------------------------------------------------------------

// Middleware personalizado para manejo global de errores
// Se registra después de Routing para capturar errores en los controladores también
app.UseRouting();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Redirección automática HTTP -> HTTPS
app.UseHttpsRedirection();

// Habilita el uso de archivos estáticos como CSS, JS, imágenes, etc.
app.UseStaticFiles();

// Autorización 
app.UseAuthorization();

// Definición de la ruta por defecto del MVC que muestra Listado y administración de Candidatos
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Candidate}/{action=Index}/{id?}");

// Punto de entrada principal de la aplicación
app.Run();