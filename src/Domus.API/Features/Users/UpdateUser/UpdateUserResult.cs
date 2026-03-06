using Domus.Api.Features.Users.Shared;

namespace Domus.Api.Features.Users.UpdateUser;

/// <summary>
/// Resultado da operação de atualização de usuário.
/// </summary>
/// <param name="User">DTO com os dados atualizados do usuário.</param>
public record UpdateUserResult(UserDto User);
