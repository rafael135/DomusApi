using Domus.Api.Features.Categories.Shared;
using Domus.Core.Domain.Transactions;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Categories.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(DomusDbContext dbContext, ILogger<CreateCategoryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CreateCategoryResult> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        TransactionCategory category = TransactionCategory.Create(
            request.Description,
            request.Finality
        );

        _dbContext.TransactionCategories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category created with ID: {CategoryId}", category.Id);

        return new CreateCategoryResult(
            new CategoryDto(category.Id, category.Description, category.Finality)
        );
    }
}
