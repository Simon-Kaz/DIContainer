namespace DepInjectionForUnity.Interfaces;

public interface IServiceProvider : IDisposable
{
    TService GetService<TService>();
    object GetService(Type serviceType);
    IServiceScope CreateScope();
}