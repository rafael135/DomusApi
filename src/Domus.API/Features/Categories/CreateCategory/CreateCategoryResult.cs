using Domus.Api.Features.Categories.Shared;

namespace Domus.Api.Features.Categories.CreateCategory;

/// <summary>
/// Resultado da operação de criação de categoria.
/// </summary>
/// <param name="Category">DTO com os dados da categoria criada.</param>
public record CreateCategoryResult(CategoryDto Category);
