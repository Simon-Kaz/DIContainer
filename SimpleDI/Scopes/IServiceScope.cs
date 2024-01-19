using System;
using IServiceProvider = SimpleDI.Providers.IServiceProvider;
using Providers_IServiceProvider = SimpleDI.Providers.IServiceProvider;
using SimpleDI_Providers_IServiceProvider = SimpleDI.Providers.IServiceProvider;

namespace SimpleDI.Scopes
{
    /// <summary>
    /// Defines a mechanism to manage the scope of resolved services.
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// Defines a mechanism to manage the scope of resolved services.
        /// </summary>
        SimpleDI_Providers_IServiceProvider ServiceProvider { get; }
    }
}