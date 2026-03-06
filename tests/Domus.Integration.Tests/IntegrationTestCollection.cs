namespace Domus.Integration.Tests;

/// <summary>
/// Defines the "IntegrationTests" xUnit collection.
/// All test classes decorated with [Collection("IntegrationTests")] share a single
/// <see cref="DomusApiFactory"/> instance (one container for the entire test session).
/// </summary>
[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<DomusApiFactory>
{
}
