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

        foreach (var scopedInstance in _scopedInstances.Values)
        {
            // Attempt to dispose of the service instance and catch any exceptions.
            if (scopedInstance is IDisposable disposable)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception ex)
                {
                    //TODO: Log the exception to the console or to your preferred logging provider.
                    Console.WriteLine($"Error disposing scoped service {scopedInstance.GetType().Name}: {ex.Message}");
                }
            }
        }

        _scopedInstances.Clear();
    }
}