using Application.Common.DTOs;
using MediatR;

namespace Application.Features.Candidates.Commands.UpdateCandidate
{
    /// <summary>
    /// Comando para actualizar la información de un candidato existente.
    /// </summary>
    /// <param name="Candidate">Datos actualizados del candidato.</param>
    /// <returns>Devuelve el <see cref="CandidateDto"/> actualizado</returns>
    public record UpdateCandidateCommand(CandidateDto Candidate)
        : IRequest<CandidateDto>;
}
