using Application.Common.DTOs;
using Application.Features.Interfaces;
using MediatR;

namespace Application.Features.Candidates.Queries.GetCandidateById
{
    /// <summary>
    /// Handler que procesa la solicitud para obtener un candidato por su ID.
    /// </summary>
    public class GetCandidateByIdCommandHandler : IRequestHandler<GetCandidateByIdCommand, CandidateDto>
    {
        private readonly ICandidateRepository _candidateRepository;

        /// <summary>
        /// Constructor que inyecta el repositorio de candidatos.
        /// </summary>
        /// <param name="candidateRepository">Repositorio de acceso a datos de candidatos.</param>
        public GetCandidateByIdCommandHandler(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        /// <summary>
        /// Maneja la consulta para obtener un candidato por su ID.
        /// </summary>
        /// <param name="request">Instancia del comando con el ID del candidato.</param>
        /// <param name="cancellationToken">Token para cancelar la operación de forma anticipada.</param>
        /// <returns>
        /// Una instancia de <see cref="CandidateDto"/> si el candidato existe; de lo contrario, <c>null</c>.
        /// </returns>
        public async Task<CandidateDto> Handle(GetCandidateByIdCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _candidateRepository.GetByIdAsync(request.IdCandidate, cancellationToken);

            if (candidate == null)
            {
                return null;
            }

            return new CandidateDto
            {
                IdCandidate = candidate.IdCandidate,
                Name = candidate.Name,
                Surname = candidate.Surname,
                Birthdate = candidate.Birthdate,
                Email = candidate.Email,
                InsertDate = candidate.InsertDate,
                ModifyDate = candidate.ModifyDate,
                Experiences = candidate.Experiences.Select(e => new CandidateExperienceDto
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
        }
    }
}
