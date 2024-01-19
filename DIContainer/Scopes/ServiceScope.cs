using System;
using DIContainer.Providers;
using IServiceProvider = DIContainer.Providers.IServiceProvider;
using Providers_IServiceProvider = DIContainer.Providers.IServiceProvider;

namespace DIContainer.Scopes
{
    /// <summary>
    /// Represents a scope for service resolution and disposal. Scoped services resolved
    /// in this scope will be disposed when the scope itself is disposed.
    /// </summary>
    public class ServiceScope : IServiceScope
    {
        public Providers_IServiceProvider ServiceProvider { get; }
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the ServiceScope class.
        /// </summary>
        /// <param name="rootProvider">The root service provider used to create the scope.</param>
        public ServiceScope(Providers_IServiceProvider rootProvider)
        {
            ServiceProvider = rootProvider.CreateScopedServiceProvider();
            _disposed = false;
        }

        /// <summary>
        /// Disposes the scoped services that were created within the scope.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            try
            {
                (ServiceProvider as ScopedServiceProvider)?.Dispose();
            }
            catch (Exception ex)
            {
                //TODO: Log the exception to the console or to e.g. logging provider, will need to inject the logger
                Console.WriteLine($"Error disposing Scoped Service Provider {ServiceProvider.GetType().Name}: {ex.Message}");
            }
        
            _disposed = true;
        }
    }
}