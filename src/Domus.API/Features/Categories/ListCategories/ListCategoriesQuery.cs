using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Shared;
using Domus.Core.Domain.Transactions.Enums;
using MediatR;

namespace Domus.Api.Features.Categories.ListCategories;

/// <summary>
/// Query para listar categorias de transação de forma paginada, com filtro opcional por finalidade.
/// </summary>
/// <param name="PageNumber">Número da página solicitada.</param>
/// <param name="PageSize">Quantidade de registros por página.</param>
/// <param name="Finality">Finalidade para filtrar as categorias (opcional).</param>
public record ListCategoriesQuery(int PageNumber, int PageSize, TransactionCategoryType? Finality)
    : IRequest<PaginatedResult<CategoryDto>>;
