using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Domus.Api.Extensions;

/// <summary>
/// Métodos de extensão para descoberta e registro automático de endpoints que implementam <see cref="IEndpoint"/>.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Varre o <paramref name="assembly"/> em busca de tipos que implementam <see cref="IEndpoint"/>
    /// e os registra como serviços no contêiner de dependências.
    /// </summary>
    /// <param name="services">Coleção de serviços onde os endpoints serão registrados.</param>
    /// <param name="assembly">Assembly a ser varrido em busca de implementações de <see cref="IEndpoint"/>.</param>
    /// <returns>A própria <see cref="IServiceCollection"/> para encadeamento de chamadas.</returns>
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var endpointTypes = assembly
            .GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            services.AddScoped(typeof(IEndpoint), type);
        }

        return services;
    }

    /// <summary>
    /// Resolve todos os <see cref="IEndpoint"/> registrados e chama <see cref="IEndpoint.MapEndpoint"/>
    /// adicionando as rotas ao <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O route builder onde os endpoints serão mapeados.</param>
    /// <returns>O próprio <see cref="IEndpointRouteBuilder"/> para encadeamento de chamadas.</returns>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var scope = app.ServiceProvider.CreateScope();
        var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
