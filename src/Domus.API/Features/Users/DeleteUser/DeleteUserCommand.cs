using MediatR;

namespace Domus.Api.Features.Users.DeleteUser;

public record DeleteUserCommand(Guid UserId) : IRequest<DeleteUserResult>;
