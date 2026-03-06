using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Users;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Users.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<DeleteUserHandler> _logger;

    public DeleteUserHandler(DomusDbContext dbContext, ILogger<DeleteUserHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<DeleteUserResult> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? existentUser = _dbContext.Users.Find(request.UserId);

        if (existentUser is null)
        {
            _logger.LogWarning("User with ID {UserId} not found for deletion.", request.UserId);
            throw new NotFoundException(nameof(User), request.UserId);
        }

        _dbContext.Users.Remove(existentUser);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with ID {UserId} deleted successfully.", request.UserId);
        return new DeleteUserResult(request.UserId);
    }
}
