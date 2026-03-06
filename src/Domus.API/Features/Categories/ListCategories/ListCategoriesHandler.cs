using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Shared;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Categories.ListCategories;

/// <summary>
/// Handler MediatR responsável por listar categorias de forma paginada.
/// </summary>
public class ListCategoriesHandler
    : IRequestHandler<ListCategoriesQuery, PaginatedResult<CategoryDto>>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<ListCategoriesHandler> _logger;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados e o logger.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    /// <param name="logger">Logger para registro de eventos.</param>
    public ListCategoriesHandler(DomusDbContext dbContext, ILogger<ListCategoriesHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Processa a query de listagem de categorias aplicando filtro e paginação.
    /// </summary>
    /// <param name="request">Query contendo os parâmetros de página e filtro de finalidade.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado paginado com as categorias encontradas.</returns>
    public async Task<PaginatedResult<CategoryDto>> Handle(
        ListCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = _dbContext.TransactionCategories.AsQueryable();

        if (request.Finality.HasValue)
            query = query.Where(c => c.Finality == request.Finality.Value);

        int totalItems = await query.CountAsync(cancellationToken);
        int totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CategoryDto(c.Id, c.Description, c.Finality))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CategoryDto>(
            items,
            totalPages,
            totalItems,
            request.PageNumber,
            request.PageSize
        );
    }
}
