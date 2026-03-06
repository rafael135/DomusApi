using Domus.Api.Features.Users.Shared;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Users;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Handler for creating a new user.
/// </summary>
public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<CreateUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserHandler"/> class.
    /// </summary>
    /// <param name="dbContext">The database context for accessing user data.</param>
    /// <param name="logger">The logger instance.</param>
    public CreateUserHandler(DomusDbContext dbContext, ILogger<CreateUserHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Handles the create user command.
    /// </summary>
    /// <param name="request">The command containing the user information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the create user operation.</returns>
    /// <exception cref="FormException">Thrown when the user data is invalid.</exception>
    public async Task<CreateUserResult> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User newUser = User.Create(request.Name, request.Age);

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User created with ID: {UserId}", newUser.Id);

        return new CreateUserResult(new UserDto(newUser.Id, newUser.Name, newUser.Age));
    }
}
