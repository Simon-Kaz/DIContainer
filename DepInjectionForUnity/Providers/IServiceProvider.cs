using DepInjectionForUnity.Scopes;

namespace DepInjectionForUnity.Providers;

/// <summary>
/// Defines a mechanism for retrieving a service object by type from the container.
/// </summary>
public interface IServiceProvider : IDisposable
{
    TService GetService<TService>();
    
    /// <summary>
    /// Resolves a service object by the given type.
    /// </summary>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <returns>An instance of the requested service type.</returns>
    object GetService(Type serviceType); // TODO: Decide if we want to keep this or make it internal. We want consumers of the interface to use the generic one instead.
    
    IServiceScope CreateScope();
    IServiceProvider CreateScopedServiceProvider();
}