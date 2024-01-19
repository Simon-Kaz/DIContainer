using DIContainer.Services;
using Tests.Common;
namespace Tests.Scopes
{
    [TestFixture]
    public class ServiceScopeTests
    {
        private IServiceCollection _services;
        private IServiceProvider _rootProvider;

        [SetUp]
        public void SetUp()
        {
            _services = new ServiceCollection();
        }

        [Test]
        public void ServiceProvider_ResolvesScopedService_WithinScope_AsSameInstance()
        {
            _services.RegisterScoped<IBazService, BazService>();
            _rootProvider = _services.BuildServiceProvider();

            using (var scope1 = _rootProvider.CreateScope())
            {
                var instance1 = scope1.ServiceProvider.GetService<IBazService>();
                var instance2 = scope1.ServiceProvider.GetService<IBazService>();

                Assert.That(instance1, Is.SameAs(instance2));
            }
        }

        [Test]
        public void ServiceProvider_ResolvesScopedService_OutsideScope_AsDifferentInstances()
        {
            _services.RegisterScoped<IBazService, BazService>();
            _rootProvider = _services.BuildServiceProvider();

            IBazService instance1, instance2;
            using (var scope1 = _rootProvider.CreateScope())
            {
                instance1 = scope1.ServiceProvider.GetService<IBazService>();
            }

            using (var scope2 = _rootProvider.CreateScope())
            {
                instance2 = scope2.ServiceProvider.GetService<IBazService>();
            }

            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void ServiceProvider_CannotResolveScopedService_Directly()
        {
            _services.RegisterScoped<IBazService, BazService>();
            _rootProvider = _services.BuildServiceProvider();

            Assert.Throws<InvalidOperationException>(() => _rootProvider.GetService<IBazService>());
        }
        
        [Test]
        public void ScopedServiceProvider_DisposesScopedServices_OnScopeDisposal()
        {
            _services.RegisterScoped<IBarService, DisposableService>();
            _rootProvider = _services.BuildServiceProvider();

            var scope = _rootProvider.CreateScope();
            var serviceInstance = scope.ServiceProvider.GetService<IBarService>() as DisposableService;
            Assert.IsFalse(serviceInstance.IsDisposed, "Service instance should not be disposed immediately after resolution.");

            scope.Dispose();
            Assert.IsTrue(serviceInstance.IsDisposed, "Service instance should be disposed after the scope is disposed.");
        }
        
        [Test]
        public void ScopedServiceProvider_CanDisposeMultipleTimes_WithoutError()
        {
            _services.RegisterScoped<DisposableService, DisposableService>();
            _rootProvider = _services.BuildServiceProvider();

            var scope = _rootProvider.CreateScope();
            scope.Dispose();
            Assert.DoesNotThrow(() => scope.Dispose());
        }
        [Test]
        public void ServiceScope_DisposesScopedServices()
        {
            _services.RegisterScoped<IBarService, DisposableService>();
            _rootProvider = _services.BuildServiceProvider();
            
            IBarService scopedService;
            using (var scope = _rootProvider.CreateScope())
            {
                scopedService = scope.ServiceProvider.GetService<IBarService>();
                Assert.IsNotNull(scopedService);
            }
            
            Assert.IsTrue(((DisposableService)scopedService).IsDisposed);
        }
        

        [TearDown]
        public void Teardown()
        {
            (_rootProvider as IDisposable)?.Dispose();
        }
    }
}