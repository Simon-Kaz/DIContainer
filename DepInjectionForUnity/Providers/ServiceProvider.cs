using DepInjectionForUnity.Scopes;
using DepInjectionForUnity.Services;

namespace DepInjectionForUnity.Providers;

/// <summary>
/// Default implementation of the IServiceProvider.
/// Responsible for creating and managing the lifetime of services.
/// </summary>
public class ServiceProvider : IServiceProvider
{
    private readonly List<ServiceDescriptor> _services;
    private readonly Dictionary<ServiceDescriptor, object> _singletons = new Dictionary<ServiceDescriptor, object>();
    private bool _disposed;

    public ServiceProvider(IEnumerable<ServiceDescriptor> services)
    {
        _services = services.ToList();
        _disposed = false;
    }

    /// <summary>
    /// Retrieves a singleton or transient service of the specified type.
    /// </summary>
    /// <typeparam name="TService">The type of service to resolve.</typeparam>
    /// <returns>An instance of the service.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the requested service has not been registered.
    /// </exception>
    public TService GetService<TService>()
    {
        return (TService) GetService(typeof(TService));
    }

    /// <summary>
    /// private method for getting the service by Type, intended only for internal usage by this ScopedServiceProvider.
    /// </summary>
    /// <param name="serviceType">The type of the service to resolve.</param>
    /// <returns>An instance of the requested service.</returns>
    public object GetService(Type serviceType)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ServiceProvider));

        var descriptor = _services.FirstOrDefault(s => s.ServiceType == serviceType);
        if (descriptor == null)
            throw new InvalidOperationException($"Service of type {serviceType.Name} is not registered.");

        switch (descriptor.Lifetime)
        {
            case ServiceLifetime.Singleton:
                return GetOrCreateSingleton(descriptor);
            case ServiceLifetime.Transient:
                return CreateInstance(descriptor.ImplementationType);
            case ServiceLifetime.Scoped:
                throw new InvalidOperationException("Cannot resolve scoped service from root provider.");
            default:
                throw new InvalidOperationException("Unknown service lifetime.");
        }
    }

    public IServiceProvider CreateScopedServiceProvider()
    {
        var scopedDescriptors = _services.Where(sd => sd.Lifetime == ServiceLifetime.Scoped).ToList();
        return new ScopedServiceProvider(this, scopedDescriptors);
    }

    private object GetOrCreateSingleton(ServiceDescriptor descriptor)
    {
        if (!_singletons.TryGetValue(descriptor, out var instance))
        {
            instance = CreateInstance(descriptor.ImplementationType);
            _singletons.Add(descriptor, instance);
        }

        return instance;
    }

    /// <summary>
    /// Instantiates an implementation type using its first available constructor.
    /// </summary>
    /// <param name="implementationType">The type to instantiate.</param>
    /// <returns>An instance of the implementation type.</returns>
    private object CreateInstance(Type implementationType)
    {
        // Logic here to create an instance of the service.
        var constructorInfo = implementationType.GetConstructors().First();
        var parameters = constructorInfo.GetParameters().Select(p => GetService(p.ParameterType)).ToArray();
        return Activator.CreateInstance(implementationType, parameters);
    }

    /// <summary>
    /// Creates a new scope for resolving scoped services.
    /// </summary>
    /// <returns>An IServiceScope that can be used to resolve scoped services.</returns>
    public IServiceScope CreateScope()
    {
        return new ServiceScope(this);
    }

    /// <summary>
    /// Disposes of the service provider, releasing all singleton services
    /// that implement IDisposable.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        foreach (var singleton in _singletons.Values)
        {
            // Attempt to dispose of the service instance and catch any exceptions.
            if (singleton is IDisposable disposable)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception ex)
                {
                    // Log the exception to the console or to your preferred logging provider.
                    Console.WriteLine($"Error disposing service {singleton.GetType().Name}: {ex.Message}");
                }
            }
        }

        _singletons.Clear();
        _disposed = true;
    }
}