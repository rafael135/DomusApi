namespace Domus.Integration.Tests;

/// <summary>
/// Define a coleção xUnit &quot;IntegrationTests&quot;.
/// Todas as classes de teste decoradas com <c>[Collection("IntegrationTests")]</c> compartilham uma única
/// instância de <see cref="DomusApiFactory"/> (um único container por sessão de testes).
/// </summary>
[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<DomusApiFactory> { }
