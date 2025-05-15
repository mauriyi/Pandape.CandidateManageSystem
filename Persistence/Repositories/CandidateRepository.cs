using Application.Features.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;

namespace Persistence.Repositories
{
    /// <summary>
    /// Implementación del repositorio para la entidad Candidate.
    /// Maneja las operaciones CRUD usando EF Core y el ApplicationDbContext.
    /// </summary>
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inyección del contexto para acceso a datos.
        /// </summary>
        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega un nuevo candidato de forma asíncrona.
        /// </summary>
        public async Task AddAsync(Candidate candidate, CancellationToken token)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync(token);
        }

        /// <summary>
        /// Actualiza un candidato existente de forma asíncrona.
        /// </summary>
        public async Task UpdateAsync(Candidate candidate, CancellationToken token)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync(token);
        }

        /// <summary>
        /// Elimina un candidato de forma asíncrona.
        /// </summary>
        public async Task DeleteAsync(Candidate candidate, CancellationToken token)
        {
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync(token);
        }

        /// <summary>
        /// Obtiene la lista completa de candidatos con sus experiencias.
        /// </summary>
        public async Task<List<Candidate>> GetAllAsync(CancellationToken token)
        {
            return await _context.Candidates
                .Include(c => c.Experiences)
                .ToListAsync(token);
        }

        /// <summary>
        /// Obtiene un candidato por su ID, incluyendo sus experiencias relacionadas.
        /// </summary>
        public async Task<Candidate?> GetByIdAsync(int idCandidate, CancellationToken token)
        {
            return await _context.Candidates
                .Include(c => c.Experiences)
                .FirstOrDefaultAsync(c => c.IdCandidate == idCandidate, token);
        }
    }
}
