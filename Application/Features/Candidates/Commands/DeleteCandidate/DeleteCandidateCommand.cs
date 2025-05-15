using MediatR;

namespace Application.Features.Candidates.Commands.DeleteCandidate
{
    /// <summary>
    /// Comando para eliminar un candidato del sistema por su identificador.
    /// </summary>
    /// <param name="IdCandidate">Identificador único del candidato a eliminar.</param>
    /// <returns>Devuelve <c>true</c> si la eliminación fue exitosa, <c>false</c> en caso contrario.</returns>
    public record DeleteCandidateCommand(int IdCandidate)
        : IRequest<bool>; // Devuelve si el candidato fue eliminado con éxito
}
