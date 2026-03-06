using MediatR;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Command to create a new user.
/// </summary>
/// <param name="Name">The name of the user to create.</param>
/// <param name="Age">The age of the user to create.</param>
public record CreateUserCommand(string Name, int Age) : IRequest<CreateUserResult>;
