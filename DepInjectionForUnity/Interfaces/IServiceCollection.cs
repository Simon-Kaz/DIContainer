namespace DepInjectionForUnity.Interfaces;

public interface IServiceCollection
{
    void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;
    void RegisterTransient<TService, TImplementation>() where TImplementation : TService;
    void RegisterScoped<TService, TImplementation>() where TImplementation : TService;
    IServiceProvider BuildServiceProvider();
}