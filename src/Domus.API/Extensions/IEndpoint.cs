using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Extensions;

/// <summary>
/// Abstração mínima para endpoints descobríveis que sabem como se registrar em um <see cref="IEndpointRouteBuilder"/>.
/// Implemente esta interface para registrar endpoints por feature usando o mecanismo de descoberta automática do projeto.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Mapeia as rotas e handlers no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde as rotas serão registradas.</param>
    void MapEndpoint(IEndpointRouteBuilder app);
}
