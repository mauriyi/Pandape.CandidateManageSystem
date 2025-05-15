using Application.Common.DTOs;
using MediatR;

namespace Application.Features.Candidates.Queries.GetAllCandidates
{
    /// <summary>
    /// Query para obtener todos los candidatos registrados en el sistema.
    /// </summary>
    public record GetAllCandidatesCommand : IRequest<IEnumerable<CandidateDto>>;
}
