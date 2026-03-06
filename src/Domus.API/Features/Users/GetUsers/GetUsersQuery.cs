using Domus.Api.Features.Shared;
using Domus.Api.Features.Users.Shared;
using MediatR;

namespace Domus.Api.Features.Users.GetUsers;

/// <summary>
/// Query para listar usuários de forma paginada, com filtro opcional por nome.
/// </summary>
/// <param name="pageNumber">Número da página solicitada.</param>
/// <param name="pageSize">Quantidade de registros por página.</param>
/// <param name="searchTerm">Termo de busca para filtrar usuários pelo nome (opcional).</param>
public record GetUsersQuery(int pageNumber, int pageSize, string? searchTerm)
    : IRequest<PaginatedResult<UserDto>>;
