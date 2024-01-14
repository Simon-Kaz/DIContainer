using DepInjectionForUnity.Interfaces;
using IServiceProvider = DepInjectionForUnity.Interfaces.IServiceProvider;

namespace DepInjectionForUnity;

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

    public TService GetService<TService>()
    {
        return (TService) GetService(typeof(TService));
    }

    public object GetService(Type serviceType)
    {
        var descriptor = _services.FirstOrDefault(s => s.ServiceType == serviceType);
        if (descriptor == null)
        {
            throw new InvalidOperationException($"Service of type {serviceType.Name} is not registered.");
        }

        if (descriptor.Lifetime == ServiceLifetime.Singleton)
        {
            if (!_singletons.TryGetValue(descriptor, out var instance))
            {
                instance = CreateInstance(descriptor.ImplementationType);
                _singletons[descriptor] = instance;
            }

            return instance;
        }

        if (descriptor.Lifetime == ServiceLifetime.Transient)
        {
            return CreateInstance(descriptor.ImplementationType);
        }

        if (descriptor.Lifetime == ServiceLifetime.Scoped)
        {
            throw new InvalidOperationException("Cannot directly resolve scoped services. Use CreateScope() method.");
        }

        throw new InvalidOperationException("Invalid service lifetime.");
    }

    private object CreateInstance(Type implementationType)
    {
        var constructorInfo = implementationType.GetConstructors().First();
        var parameters = constructorInfo.GetParameters().Select(p => GetService(p.ParameterType)).ToArray();
        var instance = Activator.CreateInstance(implementationType, parameters);
        return instance;
    }

    public IServiceScope CreateScope()
    {
        // Create a new scope with its own scoped instances
        return new ServiceScope(this, _services);
    }

    public void Dispose()
    {
        if (_disposed) return;

        // Dispose of all singleton instances that implement IDisposable
        foreach (var instance in _singletons.Values)
        {
            (instance as IDisposable)?.Dispose();
        }
        _singletons.Clear();

        _disposed = true;
    }
}