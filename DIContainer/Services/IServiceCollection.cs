using IServiceProvider = DIContainer.Providers.IServiceProvider;
using Providers_IServiceProvider = DIContainer.Providers.IServiceProvider;

namespace DIContainer.Services
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
        Providers_IServiceProvider BuildServiceProvider();
    }
}