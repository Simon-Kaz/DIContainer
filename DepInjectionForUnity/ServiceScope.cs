using DepInjectionForUnity.Interfaces;
using IServiceProvider = DepInjectionForUnity.Interfaces.IServiceProvider;

namespace DepInjectionForUnity;

public class ServiceScope : IServiceScope
{
    public IServiceProvider ServiceProvider { get; }
    private readonly Dictionary<ServiceDescriptor, object> _scopedInstances = new Dictionary<ServiceDescriptor, object>();
    private bool _disposed;

    public ServiceScope(IServiceProvider rootProvider, IEnumerable<ServiceDescriptor> services)
    {
        ServiceProvider = new ScopedServiceProvider(rootProvider, services, _scopedInstances);
        _disposed = false;
    }

    public void Dispose()
    {
        if (_disposed) return;

        // Dispose of all scoped instances that implement IDisposable
        foreach (var instance in _scopedInstances.Values)
        {
            (instance as IDisposable)?.Dispose();
        }
        _scopedInstances.Clear();

        _disposed = true;
    }
}