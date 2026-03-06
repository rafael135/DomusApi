using Domus.Api.Features.Users.Shared;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Users;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Handler MediatR responsável por criar um novo usuário no banco de dados.
/// </summary>
public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<CreateUserHandler> _logger;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados e o logger.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    /// <param name="logger">Logger para registro de eventos.</param>
    public CreateUserHandler(DomusDbContext dbContext, ILogger<CreateUserHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Processa o comando de criação de usuário.
    /// </summary>
    /// <param name="request">Comando contendo os dados do novo usuário.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado com os dados do usuário criado.</returns>
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
