using System;

namespace SimpleDI.Providers
{
    public static class LazyResolutionExtensions
    {
        /// <summary>
        /// Resolves a Lazy service allowing the service creation to be deferred until the Lazy Value is accessed.
        /// </summary>
        /// <typeparam name="TService">The type of service to be resolved.</typeparam>
        /// <param name="provider">The IServiceProvider to use for resolution.</param>
        /// <returns>A Lazy container for the resolved service.</returns>
        public static Lazy<TService> GetLazyService<TService>(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            Lazy<TService> CreateLazy()
            {
                return new Lazy<TService>(() => provider.GetService<TService>());
            }

            return CreateLazy();
        }
    }
}