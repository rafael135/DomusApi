using MediatR;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Comando para criar um novo usuário.
/// </summary>
/// <param name="Name">Nome do usuário a ser criado.</param>
/// <param name="Age">Idade do usuário a ser criado.</param>
public record CreateUserCommand(string Name, int Age) : IRequest<CreateUserResult>;
