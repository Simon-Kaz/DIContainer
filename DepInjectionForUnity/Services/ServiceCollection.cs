using DepInjectionForUnity.Providers;
using IServiceProvider = DepInjectionForUnity.Providers.IServiceProvider;

namespace DepInjectionForUnity.Services;

/// <summary>
/// Manages the collection of service descriptors and builds the service provider.
/// </summary>
public class ServiceCollection : IServiceCollection
{
    private readonly List<ServiceDescriptor> _services = new List<ServiceDescriptor>();

    /// <summary>
    /// Registers a singleton service with the service collection.
    /// </summary>
    /// <typeparam name="TService">The type of the service interface.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation of the service.</typeparam>
    public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
    }

    /// <summary>
    /// Registers a transient service with the service collection.
    /// </summary>
    /// <typeparam name="TService">The type of the service interface.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation of the service.</typeparam>
    public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
    }

    /// <summary>
    /// Registers a scoped service with the service collection.
    /// </summary>
    /// <typeparam name="TService">The type of the service interface.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation of the service.</typeparam>
    public void RegisterScoped<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
    }

    /// <summary>
    /// Builds the ServiceProvider containing services registered in the collection.
    /// </summary>
    /// <returns>An IServiceProvider instance ready to resolve services.</returns>
    public IServiceProvider BuildServiceProvider()
    {
        ValidateServiceCollection();
        return new ServiceProvider(_services);
    }

    /// <summary>
    /// Validates the current service collection to ensure all services
    /// can be properly constructed when required.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the service collection has invalid registrations.
    /// </exception>
    private void ValidateServiceCollection()
    {
        foreach (var service in _services)
        {
            // Ensure the implementation type is not abstract or an interface.
            if (service.ImplementationType.IsAbstract || service.ImplementationType.IsInterface)
            {
                throw new InvalidOperationException($"Service type '{service.ImplementationType.Name}' is not instantiable.");
            }

            // Ensure the service has at least one public constructor.
            if (!service.ImplementationType.GetConstructors().Any())
            {
                throw new InvalidOperationException($"Service type '{service.ImplementationType.Name}' does not have any public constructors.");
            }
        }

        // Additional validation logic could be placed here
    }
}