using IServiceProvider = DepInjectionForUnity.Providers.IServiceProvider;

namespace DepInjectionForUnity.Services
{
    /// <summary>
    /// Defines methods for registering services in the dependency injection container
    /// and building a service provider.
    /// </summary>
    public interface IServiceCollection
    {
        void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;
        void RegisterTransient<TService, TImplementation>() where TImplementation : TService;
        void RegisterScoped<TService, TImplementation>() where TImplementation : TService;
        IServiceProvider BuildServiceProvider();
    }
}