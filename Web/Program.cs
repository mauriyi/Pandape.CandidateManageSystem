using Application.Features.Candidates.Commands.CreateCandidate;
using Application.Features.Interfaces;
using CandidateManagement.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// CONFIGURACI�N DE SERVICIOS 
// -----------------------------------------------------------------------------

// Agrega soporte a controladores con vistas 
builder.Services.AddControllersWithViews();

// Registro de MediatR para manejo de comandos y queries
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCandidateCommandHandler).Assembly));

// Configuraci�n del DbContext usando una base de datos en memoria 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("CandidateDb"));

// Registro del repositorio de candidatos mediante la interfaz
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

var app = builder.Build();

// -----------------------------------------------------------------------------
// CONFIGURACI�N DEL PIPELINE HTTP
// -----------------------------------------------------------------------------

// Middleware personalizado para manejo global de errores
// Se registra despu�s de Routing para capturar errores en los controladores tambi�n
app.UseRouting();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Redirecci�n autom�tica HTTP -> HTTPS
app.UseHttpsRedirection();

// Habilita el uso de archivos est�ticos como CSS, JS, im�genes, etc.
app.UseStaticFiles();

// Autorizaci�n 
app.UseAuthorization();

// Definici�n de la ruta por defecto del MVC que muestra Listado y administraci�n de Candidatos
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Candidate}/{action=Index}/{id?}");

// Punto de entrada principal de la aplicaci�n
app.Run();