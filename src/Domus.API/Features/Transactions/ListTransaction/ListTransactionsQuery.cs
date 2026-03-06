using Domus.Api.Features.Shared;
using Domus.Api.Features.Transactions.Shared;
using MediatR;

namespace Domus.Api.Features.Transactions.ListTransaction;

/// <summary>
/// Query para listar transações de forma paginada, com filtros opcionais por usuário e categoria.
/// </summary>
/// <param name="PageNumber">Número da página solicitada.</param>
/// <param name="PageSize">Quantidade de registros por página.</param>
/// <param name="UserId">Filtro pelo identificador do usuário (opcional).</param>
/// <param name="CategoryId">Filtro pelo identificador da categoria (opcional).</param>
public record ListTransactionsQuery(int PageNumber, int PageSize, Guid? UserId, Guid? CategoryId)
    : IRequest<PaginatedResult<TransactionDto>>;
