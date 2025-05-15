using Application.Common.DTOs;
using Application.Features.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Candidates.Commands.UpdateCandidate
{
    /// <summary>
    /// Handler encargado de procesar la actualización de un candidato.
    /// </summary>
    public class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, CandidateDto>
    {
        private readonly ICandidateRepository _candidateRepository;

        /// <summary>
        /// Inicializa una nueva instancia del <see cref="UpdateCandidateCommandHandler"/>.
        /// </summary>
        /// <param name="candidateRepository">Repositorio de acceso a datos del candidato.</param>
        public UpdateCandidateCommandHandler(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        /// <summary>
        /// Maneja la lógica de actualización de un candidato y sus experiencias asociadas.
        /// </summary>
        /// <param name="request">Comando con los datos actualizados del candidato.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Devuelve el <see cref="CandidateDto"/> actualizado, o <c>null</c> si el candidato no existe.</returns>
        public async Task<CandidateDto> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _candidateRepository.GetByIdAsync(request.Candidate.IdCandidate, cancellationToken);

            if (candidate == null)
            {
                // No se encontró el candidato a actualizar.
                return null;
            }

            // Actualizar datos principales del candidato.
            candidate.Name = request.Candidate.Name;
            candidate.Surname = request.Candidate.Surname;
            candidate.Birthdate = request.Candidate.Birthdate;
            candidate.Email = request.Candidate.Email;
            candidate.ModifyDate = DateTime.UtcNow;
            // Reemplazar experiencias asociadas.
            candidate.Experiences = [.. request.Candidate.Experiences.Select(e => new CandidateExperience
                                        {
                                            Company = e.Company,
                                            Job = e.Job,
                                            Description = e.Description,
                                            Salary = e.Salary,
                                            BeginDate = e.BeginDate,
                                            EndDate = e.EndDate,
                                            InsertDate = e.InsertDate,
                                            ModifyDate = e.ModifyDate
                                        })
                                    ];

            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // Devolver DTO actualizado.
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
