namespace Domus.Api.Features.Shared;

/// <summary>
/// Resultado paginado genérico retornado em endpoints de listagem.
/// </summary>
/// <typeparam name="T">Tipo dos itens retornados na página.</typeparam>
/// <param name="items">Itens da página atual.</param>
/// <param name="totalPages">Total de páginas disponíveis.</param>
/// <param name="totalItems">Total de itens em todas as páginas.</param>
/// <param name="currentPage">Número da página atual.</param>
/// <param name="pageSize">Quantidade de itens por página.</param>
public record PaginatedResult<T>(
    IEnumerable<T> items,
    int totalPages,
    int totalItems,
    int currentPage,
    int pageSize
);
