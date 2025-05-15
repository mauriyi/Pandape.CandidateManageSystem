using Application.Features.Interfaces;
using MediatR;

namespace Application.Features.Candidates.Commands.DeleteCandidate
{
    /// <summary>
    /// Handler que gestiona la eliminación de un candidato usando el comando <see cref="DeleteCandidateCommand"/>.
    /// </summary>
    public class DeleteCandidateCommandHandler : IRequestHandler<DeleteCandidateCommand, bool>
    {
        private readonly ICandidateRepository _candidateRepository;

        /// <summary>
        /// Inicializa una nueva instancia del handler con el repositorio de candidatos.
        /// </summary>
        /// <param name="candidateRepository">Repositorio para operaciones sobre candidatos.</param>
        public DeleteCandidateCommandHandler(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        /// <summary>
        /// Ejecuta la eliminación del candidato si existe.
        /// </summary>
        /// <param name="request">Comando que contiene el ID del candidato a eliminar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Devuelve <c>true</c> si el candidato fue eliminado, <c>false</c> si no se encontró.</returns>
        public async Task<bool> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _candidateRepository.GetByIdAsync(request.IdCandidate, cancellationToken);

            if (candidate == null)
            {
                // No se encontró el candidato con el IdCandidate especificado.
                return false;
            }

            // Se eliminan las experiencias antes de la eliminación.
            candidate.Experiences.Clear();

            await _candidateRepository.DeleteAsync(candidate, cancellationToken);
            return true;
        }
    }
}
