using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Users;
using Domus.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Features.Users.DeleteUser;

/// <summary>
/// Handler MediatR responsável por excluir um usuário do banco de dados.
/// </summary>
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResult>
{
    private readonly DomusDbContext _dbContext;
    private readonly ILogger<DeleteUserHandler> _logger;

    /// <summary>
    /// Inicializa o handler com o contexto de banco de dados e o logger.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    /// <param name="logger">Logger para registro de eventos.</param>
    public DeleteUserHandler(DomusDbContext dbContext, ILogger<DeleteUserHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Processa o comando de exclusão de usuário.
    /// </summary>
    /// <param name="request">Comando contendo o ID do usuário a excluir.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado com o ID do usuário excluído.</returns>
    public async Task<DeleteUserResult> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken
    )
    {
        User? existentUser = _dbContext.Users.Find(request.UserId);

        if (existentUser is null)
        {
            _logger.LogWarning("User with ID {UserId} not found for deletion.", request.UserId);
            throw new NotFoundException(nameof(User), request.UserId);
        }

        _dbContext.Users.Remove(existentUser);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with ID {UserId} deleted successfully.", request.UserId);
        return new DeleteUserResult(request.UserId);
    }
}
