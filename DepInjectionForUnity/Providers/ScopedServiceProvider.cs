using System;
using System.Collections.Generic;
using System.Linq;
using DepInjectionForUnity.Scopes;
using DepInjectionForUnity.Services;

namespace DepInjectionForUnity.Providers
{
    public class ScopedServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _rootProvider;
        private readonly Dictionary<Type, object> _scopedInstances = new Dictionary<Type, object>();
        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();
        private bool _disposed;

        public ScopedServiceProvider(IServiceProvider rootProvider, IEnumerable<ServiceDescriptor> scopedServiceDescriptors)
        {
            _rootProvider = rootProvider;
            _scopedInstances = new Dictionary<Type, object>();
            foreach (var descriptor in scopedServiceDescriptors)
            {
                if (descriptor.Lifetime == ServiceLifetime.Scoped)
                {
                    _serviceDescriptors[descriptor.ServiceType] = descriptor;
                }
            }
        }

        /// <summary>
        /// Resolves scoped services within the current scope or defers to the root provider for other services.
        /// </summary>
        /// <typeparam name="TService">The type of the service to resolve.</typeparam>
        /// <returns>An instance of the requested service.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if trying to resolve a scoped service directly from the root provider or if the service is not registered.
        /// </exception>
        public TService GetService<TService>()
        {
            return (TService) GetService(typeof(TService));
        }

        /// <summary>
        /// private method for getting the service by Type, intended only for internal usage by this ScopedServiceProvider.
        /// </summary>
        /// <param name="serviceType">The type of the service to resolve.</param>
        /// <returns>An instance of the requested service.</returns>
        public object GetService(Type serviceType)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ScopedServiceProvider));

            if (_scopedInstances.TryGetValue(serviceType, out var instance))
                return instance;

            if (_serviceDescriptors.TryGetValue(serviceType, out var descriptor))
            {
                instance = CreateInstance(descriptor.ImplementationType);
                _scopedInstances.Add(serviceType, instance);
                return instance;
            }

            // Delegate the creation of non-scoped services to the root provider.
            return _rootProvider.GetService(serviceType);
        }


        private object CreateInstance(Type implementationType)
        {
            var constructorInfo = implementationType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters().Select(p => GetService(p.ParameterType)).ToArray();
            return Activator.CreateInstance(implementationType, parameters);
        }

        public IServiceScope CreateScope()
        {
            throw new NotSupportedException("Nested scopes are not supported.");
        }

        /// <summary>
        /// Throws NotSupportedException as scoped providers cannot create further scoped providers.
        /// </summary>
        public IServiceProvider CreateScopedServiceProvider()
        {
            throw new NotSupportedException("Scoped providers cannot create further scoped providers.");
        }
    
        /// <summary>
        /// Disposes the scoped services created within this service provider.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            foreach (var instance in _scopedInstances.Values)
            {
                // Attempt to dispose of the service instance and catch any exceptions.
                if (instance is IDisposable disposable)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception to the console or to your preferred logging provider.
                        Console.WriteLine($"Error disposing service {instance.GetType().Name}: {ex.Message}");
                    }
                }
            }

            _scopedInstances.Clear();
            _disposed = true;
        }
    }
}