using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Shared;
using Domus.Core.Domain.Transactions.Enums;
using MediatR;

namespace Domus.Api.Features.Categories.ListCategories;

public record ListCategoriesQuery(int PageNumber, int PageSize, TransactionCategoryType? Finality)
    : IRequest<PaginatedResult<CategoryDto>>;
