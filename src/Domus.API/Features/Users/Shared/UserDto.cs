namespace Domus.Api.Features.Users.Shared;

/// <summary>
/// DTO de usuário utilizado nas respostas da API.
/// </summary>
/// <param name="Id">Identificador único do usuário.</param>
/// <param name="Name">Nome do usuário.</param>
/// <param name="Age">Idade do usuário.</param>
public record UserDto(Guid Id, string Name, int Age);
