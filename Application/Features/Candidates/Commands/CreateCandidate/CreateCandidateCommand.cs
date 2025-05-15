using Application.Common.DTOs;
using MediatR;

namespace Application.Features.Candidates.Commands.CreateCandidate
{
    /// <summary>
    /// Comando para crear un nuevo candidato en el sistema.
    /// </summary>
    /// <param name="Candidate">DTO con la información del candidato a crear.</param>
    /// <returns>El identificador único del candidato creado, o null si falla la creación.</returns>
    public record CreateCandidateCommand(CandidateDto Candidate)
        : IRequest<int?>; // Devuelve el ID del candidato creado
}
