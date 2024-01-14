namespace DepInjectionForUnity.Interfaces;

public interface IServiceScope : IDisposable
{
    IServiceProvider ServiceProvider { get; }
}