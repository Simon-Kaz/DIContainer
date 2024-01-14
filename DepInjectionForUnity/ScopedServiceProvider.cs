namespace DepInjectionForUnity;
using Interfaces;
using System.Collections.Generic;
using System.Linq;

public class ScopedServiceProvider : IServiceProvider
{
    private readonly IServiceProvider _rootProvider;
    private readonly IEnumerable<ServiceDescriptor> _services;
    private readonly Dictionary<ServiceDescriptor, object> _scopedInstances;
    private bool _disposed;

    public ScopedServiceProvider(IServiceProvider rootProvider, IEnumerable<ServiceDescriptor> services,
        Dictionary<ServiceDescriptor, object> scopedInstances)
    {
        _rootProvider = rootProvider;
        _services = services;
        _scopedInstances = scopedInstances;
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
            return _rootProvider.GetService(serviceType);
        }

        if (descriptor.Lifetime == ServiceLifetime.Transient)
        {
            return CreateInstance(descriptor.ImplementationType);
        }

        if (descriptor.Lifetime == ServiceLifetime.Scoped)
        {
            if (!_scopedInstances.TryGetValue(descriptor, out var instance))
            {
                instance = CreateInstance(descriptor.ImplementationType);
                _scopedInstances[descriptor] = instance;
            }

            return instance;
        }

        throw new InvalidOperationException("Invalid service lifetime.");
    }

    private object CreateInstance(Type implementationType)
    {
        var constructorInfo = implementationType.GetConstructors().First();
        var parameters = constructorInfo.GetParameters().Select(p => GetService(p.ParameterType)).ToArray();
        return Activator.CreateInstance(implementationType, parameters);
    }

    public IServiceScope CreateScope()
    {
        // Create a new scope with its own scoped instances
        return new ServiceScope(this, _services);
    }

    public void Dispose()
    {
        if (_disposed) return;

        // Since the ServiceScope is responsible for disposing scoped instances,
        // there is no need to duplicate the logic here. The flag is set to prevent
        // future use of this disposed service provider.

        _disposed = true;
    }
}