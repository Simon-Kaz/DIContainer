using DepInjectionForUnity.Scopes;
using DepInjectionForUnity.Services;
using Tests.Common;

namespace Tests.Providers
{
    [TestFixture]
    public class ServiceProviderTests
    {
        private IServiceCollection _services;
        private IServiceProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _services = new ServiceCollection();
        }

        [TearDown]
        public void Teardown()
        {
            (_provider as IDisposable)?.Dispose();
        }

        [Test]
        public void ServiceProvider_ResolvesSingletons_AsSameInstance()
        {
            _services.RegisterSingleton<IFooService, FooService>();
            _provider = _services.BuildServiceProvider();

            var instance1 = _provider.GetService<IFooService>();
            var instance2 = _provider.GetService<IFooService>();

            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void ServiceProvider_ResolvesTransients_AsDifferentInstances()
        {
            _services.RegisterTransient<IBarService, BarService>();
            _provider = _services.BuildServiceProvider();

            var instance1 = _provider.GetService<IBarService>();
            var instance2 = _provider.GetService<IBarService>();
            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void ServiceProvider_DisposesSingletonServices_OnContainerDisposal()
        {
            _services.RegisterSingleton<IBarService, DisposableService>();
            _provider = _services.BuildServiceProvider();

            var disposableService = _provider.GetService<IBarService>() as DisposableService;
            Assert.IsFalse(disposableService.IsDisposed);

            _provider.Dispose();
            Assert.IsTrue(disposableService.IsDisposed);
        }

        [Test]
        public void ServiceProvider_ThrowsException_WhenUnregisteredServiceRequested()
        {
            _provider = _services.BuildServiceProvider();
            Assert.Throws<InvalidOperationException>(() =>
                    _provider.GetService<IFooService>(),
                "Requesting an unregistered service should throw an exception.");
        }

        [Test]
        public void ServiceProvider_CatchesExceptions_WhenDisposingServices()
        {
            _services.RegisterSingleton<IBarService, FaultyDisposableService>();
            _provider = _services.BuildServiceProvider();

            // FaultyDisposableService throws an exception when disposed.
            Assert.DoesNotThrow(() => _provider.Dispose());
        }

        [Test]
        public void ServiceProvider_DisposesOnlyDisposableSingletonServices()
        {
            _services.RegisterSingleton<IFooService, FooService>();  // Non-Disposable implementation
            _services.RegisterSingleton<IBarService, DisposableService>(); // Disposable implementation
            _provider = _services.BuildServiceProvider();

            var nonDisposableService = _provider.GetService<IFooService>();
            var disposableService = _provider.GetService<IBarService>() as DisposableService;
            Assert.IsFalse(disposableService.IsDisposed, "Disposable service should start off as not disposed.");

            _provider.Dispose();
            Assert.IsTrue(disposableService.IsDisposed, "Disposable service should be disposed after the service provider is disposed.");

            // Check if nonDisposableService does not have a Dispose method to call
            var isNonDisposableServiceDisposable = nonDisposableService is IDisposable;
            Assert.IsFalse(isNonDisposableServiceDisposable, "Non-Disposable service should not implement IDisposable.");
        }

        [Test]
        public void ServiceProvider_ShouldAllowDisposing_WhenNoServicesRegistered()
        {
            _provider = _services.BuildServiceProvider();
            Assert.DoesNotThrow(() => (_provider as IDisposable).Dispose());
        }
        
        [Test]
        public void ServiceProvider_CanCreateScopeSuccessfully()
        {
            _services.RegisterScoped<IBazService, BazService>();
            _provider = _services.BuildServiceProvider();
            
            using (var scope = _provider.CreateScope())
            {
                Assert.IsNotNull(scope);
                Assert.IsInstanceOf<IServiceScope>(scope);

                // Verify we can resolve a scoped service within the scope
                var scopedService = scope.ServiceProvider.GetService<IBazService>();
                Assert.IsNotNull(scopedService);
                Assert.IsInstanceOf<BazService>(scopedService);
            }
        }
    }
}