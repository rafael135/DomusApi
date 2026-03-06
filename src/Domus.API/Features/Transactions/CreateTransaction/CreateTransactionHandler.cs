using Domus.Api.Features.Transactions.Shared;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Users;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Transactions.CreateTransaction;

/// <summary>
/// Handler MediatR responsável por validar e registrar uma nova transação financeira.
/// </summary>
public class CreateTransactionHandler
    : IRequestHandler<CreateTransactionCommand, CreateTransactionResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<CreateTransactionHandler> _logger;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados e o logger.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    /// <param name="logger">Logger para registro de eventos.</param>
    public CreateTransactionHandler(
        DomusDbContext dbContext,
        ILogger<CreateTransactionHandler> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Processa o comando de criação de transação, validando usuário, categoria e regras de negócio.
    /// </summary>
    /// <param name="request">Comando com os dados da transação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado com os dados da transação criada.</returns>
    public async Task<CreateTransactionResult> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _dbContext.Users.FindAsync(
            new object[] { request.UserId },
            cancellationToken
        );
        if (user is null)
            throw new NotFoundException(nameof(User), request.UserId);

        TransactionCategory? category = await _dbContext.TransactionCategories.FindAsync(
            new object[] { request.CategoryId },
            cancellationToken
        );
        if (category is null)
            throw new NotFoundException(nameof(TransactionCategory), request.CategoryId);

        Transaction transaction = Transaction.Create(
            request.Description,
            request.Value,
            request.Type,
            category,
            user
        );

        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Transaction created with ID: {TransactionId}", transaction.Id);

        return new CreateTransactionResult(
            new TransactionDto(
                transaction.Id,
                transaction.Description,
                transaction.Value,
                transaction.Type,
                transaction.TransactionCategoryId,
                category.Description,
                transaction.UserId,
                user.Name
            )
        );
    }
}
