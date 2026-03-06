using Domus.Api.Features.Users.Shared;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Users;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Users.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(DomusDbContext dbContext, ILogger<UpdateUserHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found for update.", request.UserId);
            throw new NotFoundException(nameof(User), request.UserId);
        }

        user.Update(request.Name, request.Age);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with ID {UserId} updated successfully.", request.UserId);
        return new UpdateUserResult(new UserDto(user.Id, user.Name, user.Age));
    }
}
