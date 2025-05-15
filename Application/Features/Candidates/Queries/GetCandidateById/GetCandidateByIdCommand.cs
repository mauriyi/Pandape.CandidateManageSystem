using Application.Common.DTOs;
using MediatR;

namespace Application.Features.Candidates.Queries.GetCandidateById
{
    /// <summary>
    /// Query para obtener un candidato específico por su identificador único.
    /// </summary>
    /// <param name="IdCandidate">ID del candidato a recuperar.</param>
    public record GetCandidateByIdCommand(int IdCandidate) : IRequest<CandidateDto>;
}
