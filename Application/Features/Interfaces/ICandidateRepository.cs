using Domain.Entities;

namespace Application.Features.Interfaces
{
    /// <summary>
    /// Define el contrato para el acceso a datos relacionados con candidatos.
    /// Encapsula operaciones CRUD sobre la entidad <see cref="Candidate"/>.
    /// </summary>
    public interface ICandidateRepository
    {
        /// <summary>
        /// Agrega un nuevo candidato a la fuente de datos.
        /// </summary>
        /// <param name="candidate">Entidad <see cref="Candidate"/> que se desea agregar.</param>
        /// <param name="token">Token de cancelación para abortar la operación si es necesario.</param>
        Task AddAsync(Candidate candidate, CancellationToken token);

        /// <summary>
        /// Recupera todos los candidatos disponibles en la fuente de datos.
        /// </summary>
        /// <param name="token">Token de cancelación para abortar la operación si es necesario.</param>
        /// <returns>Lista de entidades <see cref="Candidate"/>.</returns>
        Task<List<Candidate>> GetAllAsync(CancellationToken token);

        /// <summary>
        /// Recupera un candidato específico por su identificador único.
        /// </summary>
        /// <param name="idCandidate">ID del candidato a recuperar.</param>
        /// <param name="token">Token de cancelación para abortar la operación si es necesario.</param>
        /// <returns>Una instancia de <see cref="Candidate"/> si existe; de lo contrario, <c>null</c>.</returns>
        Task<Candidate?> GetByIdAsync(int idCandidate, CancellationToken token);

        /// <summary>
        /// Actualiza los datos de un candidato existente.
        /// </summary>
        /// <param name="candidate">Entidad <see cref="Candidate"/> con los datos actualizados.</param>
        /// <param name="token">Token de cancelación para abortar la operación si es necesario.</param>
        Task UpdateAsync(Candidate candidate, CancellationToken token);

        /// <summary>
        /// Elimina un candidato de la fuente de datos.
        /// </summary>
        /// <param name="candidate">Entidad <see cref="Candidate"/> que se desea eliminar.</param>
        /// <param name="token">Token de cancelación para abortar la operación si es necesario.</param>
        Task DeleteAsync(Candidate candidate, CancellationToken token);
    }
}
