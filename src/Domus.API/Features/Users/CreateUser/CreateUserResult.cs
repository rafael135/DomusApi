using Domus.Api.Features.Users.Shared;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Result containing detailed user information.
/// </summary>
/// <param name="User">The detailed user information.</param>
public record CreateUserResult(UserDto User);
