using Application.Common.DTOs;
using Application.Features.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Candidates.Commands.CreateCandidate
{
    /// <summary>
    /// Handler encargado de procesar el comando <see cref="CreateCandidateCommand"/>.
    /// </summary>
    public class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, int?>
    {
        private readonly ICandidateRepository _candidateRepository;

        /// <summary>
        /// Inicializa una nueva instancia del handler con el repositorio de candidatos.
        /// </summary>
        /// <param name="candidateRepository">Repositorio para manejar la persistencia de candidatos.</param>
        public CreateCandidateCommandHandler(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        /// <summary>
        /// Procesa la creación de un nuevo candidato a partir del DTO recibido.
        /// </summary>
        /// <param name="request">Comando que contiene los datos del candidato a crear.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>El Id generado para el candidato, o null si la creación no fue exitosa.</returns>
        public async Task<int?> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
        {
            if (request.Candidate == null)
            {
                // No se proporciona información para crear el candidato.
                return null;
            }

            var candidate = new Candidate
            {
                Name = request.Candidate.Name,
                Surname = request.Candidate.Surname,
                Birthdate = request.Candidate.Birthdate,
                Email = request.Candidate.Email,
                InsertDate = DateTime.UtcNow,
                ModifyDate = null,
                Experiences = (request.Candidate.Experiences ?? Enumerable.Empty<CandidateExperienceDto>())
                                .Select(e => new CandidateExperience
                                {
                                    Company = e.Company,
                                    Job = e.Job,
                                    Description = e.Description,
                                    Salary = e.Salary,
                                    BeginDate = e.BeginDate,
                                    EndDate = e.EndDate,
                                    InsertDate = DateTime.UtcNow,
                                    ModifyDate = null
                                }).ToList()
            };

            await _candidateRepository.AddAsync(candidate, cancellationToken);
            return candidate.IdCandidate;
        }
    }
}
