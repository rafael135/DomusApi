using Domus.Api.Features.Categories.Shared;
using Domus.Core.Domain.Transactions;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Categories.CreateCategory;

/// <summary>
/// Handler MediatR responsável por criar uma nova categoria de transação.
/// </summary>
public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<CreateCategoryHandler> _logger;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados e o logger.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    /// <param name="logger">Logger para registro de eventos.</param>
    public CreateCategoryHandler(DomusDbContext dbContext, ILogger<CreateCategoryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Processa o comando de criação de categoria.
    /// </summary>
    /// <param name="request">Comando contendo os dados da nova categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado com os dados da categoria criada.</returns>
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
