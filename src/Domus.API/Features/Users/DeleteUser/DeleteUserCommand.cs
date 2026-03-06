using MediatR;

namespace Domus.Api.Features.Users.DeleteUser;

/// <summary>
/// Comando para excluir um usuário do sistema.
/// </summary>
/// <param name="UserId">Identificador do usuário a ser excluído.</param>
public record DeleteUserCommand(Guid UserId) : IRequest<DeleteUserResult>;
