using Application.Common.DTOs;
using Application.Features.Candidates.Commands.CreateCandidate;
using Application.Features.Candidates.Commands.DeleteCandidate;
using Application.Features.Candidates.Commands.UpdateCandidate;
using Application.Features.Candidates.Queries.GetAllCandidates;
using Application.Features.Candidates.Queries.GetCandidateById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;
using Web.ViewModels.Candidate;

namespace CandidateManagement.Web.Controllers
{
    /// <summary>
    /// Controlador responsable de gestionar las operaciones relacionadas con los candidatos.
    /// Incluye funcionalidades para listar, crear, editar y eliminar candidatos.
    /// </summary>
    public class CandidateController : Controller
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor del controlador que recibe una instancia del mediador para la gestión de comandos y consultas.
        /// </summary>
        /// <param name="mediator">Instancia de IMediator para manejar la lógica de negocio desacoplada.</param>
        public CandidateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Muestra la lista de candidatos con paginación.
        /// </summary>
        /// <param name="pageNumber">Número de página actual (por defecto es 1).</param>
        /// <param name="pageSize">Tamaño de página (por defecto es 5).</param>
        /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
        /// <returns>Vista con la lista paginada de candidatos.</returns>
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var candidateDtos = await _mediator.Send(new GetAllCandidatesCommand(), cancellationToken);

            var viewModels = candidateDtos.Select(dto => new CandidateViewModel
            {
                IdCandidate = dto.IdCandidate,
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Birthdate = dto.Birthdate,
                InsertDate = dto.InsertDate,
                Experiences = (dto.Experiences ?? Enumerable.Empty<CandidateExperienceDto>())
                                .Select(e => new CandidateExperienceViewModel
                                {
                                    Company = e.Company,
                                    Job = e.Job,
                                    Description = e.Description,
                                    Salary = e.Salary,
                                    BeginDate = e.BeginDate,
                                    EndDate = e.EndDate,
                                    InsertDate = e.InsertDate,
                                    ModifyDate = e.ModifyDate
                                }).ToList()
            }).ToList();

            var paginatedCandidates = PaginatedList<CandidateViewModel>.Create(viewModels, pageNumber, pageSize);

            return View(paginatedCandidates);
        }

        /// <summary>
        /// Muestra la vista para crear un nuevo candidato.
        /// </summary>
        /// <returns>Vista del formulario de creación de candidato.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new CandidateViewModel
            {
                Name = string.Empty,
                Surname = string.Empty,
                Birthdate = DateTime.UtcNow,
                Email = string.Empty,
                InsertDate = DateTime.UtcNow,
                Experiences = new List<CandidateExperienceViewModel>()
            };
            return View(vm);
        }

        /// <summary>
        /// Procesa el formulario de creación de un nuevo candidato.
        /// </summary>
        /// <param name="vm">Modelo de vista con la información del candidato a crear.</param>
        /// <returns>Redirecciona a la vista principal si se crea correctamente, de lo contrario, muestra el formulario con errores.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new CandidateDto
            {
                Name = vm.Name,
                Surname = vm.Surname,
                Birthdate = vm.Birthdate,
                Email = vm.Email,
                InsertDate = DateTime.UtcNow,
                Experiences = (vm.Experiences ?? Enumerable.Empty<CandidateExperienceViewModel>())
                                .Select(e => new CandidateExperienceDto
                                {
                                    Company = e.Company,
                                    Job = e.Job,
                                    Description = e.Description,
                                    Salary = e.Salary,
                                    BeginDate = e.BeginDate,
                                    EndDate = e.EndDate,
                                    InsertDate = DateTime.UtcNow
                                }).ToList()
            };

            var command = new CreateCandidateCommand(dto);
            var createdId = await _mediator.Send(command);

            if (createdId > 0)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al crear el candidato.");
            return View(vm);
        }


        /// <summary>
        /// Muestra la vista para editar un candidato existente.
        /// </summary>
        /// <param name="idCandidate">Identificador del candidato a editar.</param>
        /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
        /// <returns>Vista del formulario de edición o un error 404 si no se encuentra.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int idCandidate, CancellationToken cancellationToken = default)
        {
            var dto = await _mediator.Send(new GetCandidateByIdCommand(idCandidate), cancellationToken);

            if (dto == null)
                return NotFound();

            var vm = new CandidateViewModel
            {
                IdCandidate = dto.IdCandidate,
                Name = dto.Name,
                Surname = dto.Surname,
                Birthdate = dto.Birthdate,
                Email = dto.Email,
                InsertDate = dto.InsertDate,
                Experiences = (dto.Experiences ?? Enumerable.Empty<CandidateExperienceDto>())
                                .Select(e => new CandidateExperienceViewModel
                                {
                                    Company = e.Company,
                                    Job = e.Job,
                                    Description = e.Description,
                                    Salary = e.Salary,
                                    BeginDate = e.BeginDate,
                                    EndDate = e.EndDate,
                                    InsertDate = e.InsertDate,
                                    ModifyDate = e.ModifyDate
                                }).ToList()
            };

            return View(vm);
        }

        /// <summary>
        /// Procesa la edición de un candidato.
        /// </summary>
        /// <param name="vm">Modelo de vista con los datos actualizados del candidato.</param>
        /// <returns>Redirecciona si la actualización fue exitosa; de lo contrario, muestra los errores.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CandidateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new CandidateDto
            {
                IdCandidate = vm.IdCandidate,
                Name = vm.Name,
                Surname = vm.Surname,
                Birthdate = vm.Birthdate,
                Email = vm.Email,
                InsertDate = vm.InsertDate,
                ModifyDate = DateTime.UtcNow,
                Experiences = (vm.Experiences ?? Enumerable.Empty<CandidateExperienceViewModel>())
                    .Select(e => new CandidateExperienceDto
                    {
                        Company = e.Company,
                        Job = e.Job,
                        Description = e.Description,
                        Salary = e.Salary,
                        BeginDate = e.BeginDate,
                        EndDate = e.EndDate,
                        InsertDate = e.InsertDate,
                        ModifyDate = DateTime.UtcNow
                    }).ToList()
            };

            var command = new UpdateCandidateCommand(dto);
            var updated = await _mediator.Send(command);

            if (updated != null)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al actualizar el candidato.");
            return View(vm);
        }

        /// <summary>
        /// Elimina un candidato existente por su identificador.
        /// </summary>
        /// <param name="idCandidate">Identificador del candidato a eliminar.</param>
        /// <returns>Redirecciona a la vista principal tras intentar eliminar el candidato.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int idCandidate)
        {
            var result = await _mediator.Send(new DeleteCandidateCommand(idCandidate));

            if (result)
                return RedirectToAction(nameof(Index));

            TempData["ErrorMessage"] = "No se pudo eliminar el candidato.";
            return RedirectToAction(nameof(Index));
        }
    }
}
