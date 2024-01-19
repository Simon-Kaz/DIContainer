using System;
using IServiceProvider = DepInjectionForUnity.Providers.IServiceProvider;

namespace DepInjectionForUnity.Scopes
{
    /// <summary>
    /// Defines a mechanism to manage the scope of resolved services.
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// Defines a mechanism to manage the scope of resolved services.
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}