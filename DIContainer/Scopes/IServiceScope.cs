using System;
using IServiceProvider = DIContainer.Providers.IServiceProvider;
using Providers_IServiceProvider = DIContainer.Providers.IServiceProvider;

namespace DIContainer.Scopes
{
    /// <summary>
    /// Defines a mechanism to manage the scope of resolved services.
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// Defines a mechanism to manage the scope of resolved services.
        /// </summary>
        Providers_IServiceProvider ServiceProvider { get; }
    }
}