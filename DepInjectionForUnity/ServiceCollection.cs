using DepInjectionForUnity.Interfaces;
using IServiceProvider = DepInjectionForUnity.Interfaces.IServiceProvider;

namespace DepInjectionForUnity;

public class ServiceCollection : IServiceCollection
{
    private readonly List<ServiceDescriptor> _services = new List<ServiceDescriptor>();

    public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
    }

    public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
    }

    public void RegisterScoped<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
    }

    public IServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(_services);
    }
}