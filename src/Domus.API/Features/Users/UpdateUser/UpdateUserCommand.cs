using MediatR;

namespace Domus.Api.Features.Users.UpdateUser;

/// <summary>
/// Comando para atualizar os dados de um usuário existente.
/// </summary>
/// <param name="UserId">Identificador do usuário a ser atualizado.</param>
/// <param name="Name">Novo nome do usuário.</param>
/// <param name="Age">Nova idade do usuário.</param>
public record UpdateUserCommand(Guid UserId, string Name, int Age) : IRequest<UpdateUserResult>;
