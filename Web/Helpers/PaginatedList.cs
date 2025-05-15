using Microsoft.EntityFrameworkCore;

namespace Web.Helpers
{
    /// <summary>
    /// Clase que encapsula una lista paginada de elementos
    /// </summary>
    /// <typeparam name="T">Tipo de los elementos en la lista.</typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// Página actual.
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Número total de páginas basado en la cantidad de elementos y el tamaño de página.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Constructor privado que inicializa la lista paginada.
        /// </summary>
        /// <param name="items">Elementos de la página actual.</param>
        /// <param name="count">Total de elementos disponibles.</param>
        /// <param name="pageIndex">Índice de la página actual.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items); // Añade los elementos a la lista base.
        }

        /// <summary>
        /// Indica si existe una página anterior.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Indica si existe una página siguiente.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Crea una lista paginada de manera asíncrona a partir de un IQueryable.
        /// Ideal para consultas a base de datos.
        /// </summary>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync(); // Total de elementos
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(); // Página actual
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        /// <summary>
        /// Crea una lista paginada en memoria a partir de un IList.
        /// Ideal para colecciones ya cargadas.
        /// </summary>
        public static PaginatedList<T> Create(IList<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
