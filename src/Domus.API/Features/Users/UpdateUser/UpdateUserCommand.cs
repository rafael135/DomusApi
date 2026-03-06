using MediatR;

namespace Domus.Api.Features.Users.UpdateUser;

public record UpdateUserCommand(Guid UserId, string Name, int Age) : IRequest<UpdateUserResult>;
