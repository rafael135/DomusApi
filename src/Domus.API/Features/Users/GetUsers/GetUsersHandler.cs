using Domus.Api.Features.Shared;
using Domus.Api.Features.Users.Shared;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Users.GetUsers;

/// <summary>
/// Handler MediatR responsável por listar usuários de forma paginada.
/// </summary>
public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginatedResult<UserDto>>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<GetUsersHandler> _logger;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados e o logger.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    /// <param name="logger">Logger para registro de eventos.</param>
    public GetUsersHandler(DomusDbContext dbContext, ILogger<GetUsersHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Processa a query de listagem de usuários aplicando filtro e paginação.
    /// </summary>
    /// <param name="request">Query contendo os parâmetros de página e filtro.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado paginado com os usuários encontrados.</returns>
    public async Task<PaginatedResult<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(request.searchTerm))
        {
            query = query.Where(u => u.Name.Contains(request.searchTerm));
        }

        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)request.pageSize);

        var result = query
            .Skip((request.pageNumber - 1) * request.pageSize)
            .Take(request.pageSize)
            .Select(u => new UserDto(u.Id, u.Name, u.Age))
            .ToList();

        return new PaginatedResult<UserDto>(
            result,
            totalPages,
            totalItems,
            request.pageNumber,
            request.pageSize
        );
    }
}
