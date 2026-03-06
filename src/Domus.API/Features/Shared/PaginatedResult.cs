namespace Domus.Api.Features.Shared;

public record PaginatedResult<T>(
    IEnumerable<T> items,
    int totalPages,
    int totalItems,
    int currentPage,
    int pageSize
);
