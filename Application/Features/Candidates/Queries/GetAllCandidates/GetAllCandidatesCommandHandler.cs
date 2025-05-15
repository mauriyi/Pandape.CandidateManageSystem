using Application.Common.DTOs;
using Application.Features.Interfaces;
using MediatR;

namespace Application.Features.Candidates.Queries.GetAllCandidates
{
    /// <summary>
    /// Handler encargado de manejar la consulta para obtener todos los candidatos.
    /// </summary>
    public class GetAllCandidatesCommandHandler : IRequestHandler<GetAllCandidatesCommand, IEnumerable<CandidateDto>>
    {
        private readonly ICandidateRepository _candidateRepository;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="GetAllCandidatesCommandHandler"/>.
        /// </summary>
        /// <param name="candidateRepository">Repositorio de acceso a candidatos.</param>
        public GetAllCandidatesCommandHandler(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        /// <summary>
        /// Maneja la consulta para obtener todos los candidatos con sus experiencias laborales.
        /// </summary>
        /// <param name="request">Comando que representa la solicitud.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Una lista de <see cref="CandidateDto"/> con los datos de todos los candidatos.</returns>
        public async Task<IEnumerable<CandidateDto>> Handle(GetAllCandidatesCommand request, CancellationToken cancellationToken)
        {
            var candidates = await _candidateRepository.GetAllAsync(cancellationToken);
            return candidates.Select(c => new CandidateDto
            {
                IdCandidate = c.IdCandidate,
                Name = c.Name,
                Surname = c.Surname,
                Birthdate = c.Birthdate,
                Email = c.Email,
                InsertDate = c.InsertDate,
                ModifyDate = c.ModifyDate,
                Experiences = c.Experiences.Select(e => new CandidateExperienceDto
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
            });
        }
    }
}
