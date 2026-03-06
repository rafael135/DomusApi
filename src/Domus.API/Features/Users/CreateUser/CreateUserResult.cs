using Domus.Api.Features.Users.Shared;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Resultado da operação de criação de usuário, contendo os dados do usuário criado.
/// </summary>
/// <param name="User">DTO com os dados do usuário criado.</param>
public record CreateUserResult(UserDto User);
