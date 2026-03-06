namespace Domus.Api.Features.Users.DeleteUser;

/// <summary>
/// Resultado da operação de exclusão de usuário.
/// </summary>
/// <param name="UserId">Identificador do usuário que foi excluído.</param>
public record DeleteUserResult(Guid UserId);
