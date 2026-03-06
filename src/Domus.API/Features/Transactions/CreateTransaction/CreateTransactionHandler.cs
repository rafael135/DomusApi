using Domus.Api.Features.Transactions.Shared;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Users;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Transactions.CreateTransaction;

public class CreateTransactionHandler
    : IRequestHandler<CreateTransactionCommand, CreateTransactionResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<CreateTransactionHandler> _logger;

    public CreateTransactionHandler(
        DomusDbContext dbContext,
        ILogger<CreateTransactionHandler> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

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
