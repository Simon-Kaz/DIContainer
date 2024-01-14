using DepInjectionForUnity;
using DepInjectionForUnity.Interfaces;
using IServiceProvider = DepInjectionForUnity.Interfaces.IServiceProvider;

namespace Tests;

public class DependencyInjectionTests
{
    private IServiceCollection _services;
    private IServiceProvider _provider;

    [SetUp]
    public void Setup()
    {
        _services = new ServiceCollection();
        // Configure services as necessary for each test
    }

    [TearDown]
    public void Teardown()
    {
        (_provider as IDisposable)?.Dispose();
        // Clean up any other resources
    }

    [Test]
    public void ServiceProvider_ResolvesSingletonService_Correctly()
    {
        _services.RegisterSingleton<IFooService, FooService>();
        _provider = _services.BuildServiceProvider();

        var service1 = _provider.GetService<IFooService>();
        var service2 = _provider.GetService<IFooService>();

        Assert.That(service1, Is.Not.Null);
        Assert.That(service1, Is.InstanceOf<FooService>());
        Assert.That(service2, Is.EqualTo(service1)); // Singleton should return the same instance
    }

    [Test]
    public void ServiceProvider_ResolvesTransientService_Correctly()
    {
        _services.RegisterTransient<IBarService, BarService>();
        _provider = _services.BuildServiceProvider();

        var service1 = _provider.GetService<IBarService>();
        var service2 = _provider.GetService<IBarService>();

        Assert.That(service1, Is.Not.Null);
        Assert.That(service1, Is.InstanceOf<BarService>());
        Assert.That(service2, Is.Not.EqualTo(service1)); // Transient should return a new instance
    }

    [Test]
    public void ServiceProvider_ResolvesScopedService_WithinScope_Correctly()
    {
        _services.RegisterScoped<IBazService, BazService>();
        _provider = _services.BuildServiceProvider();

        using (var scope = _provider.CreateScope())
        {
            var service1 = scope.ServiceProvider.GetService<IBazService>();
            var service2 = scope.ServiceProvider.GetService<IBazService>();

            Assert.That(service1, Is.Not.Null);
            Assert.That(service1, Is.InstanceOf<BazService>());
            Assert.That(service2, Is.EqualTo(service1)); // Scoped should return the same instance within the scope
        }
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
    public void ServiceProvider_ResolvesScopedService_WithinScope_AsSameInstance()
    {
        _services.RegisterScoped<IBazService, BazService>();
        _provider = _services.BuildServiceProvider();

        using (var scope1 = _provider.CreateScope())
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
        _provider = _services.BuildServiceProvider();

        IBazService instance1, instance2;
        using (var scope1 = _provider.CreateScope())
        {
            instance1 = scope1.ServiceProvider.GetService<IBazService>();
        }

        using (var scope2 = _provider.CreateScope())
        {
            instance2 = scope2.ServiceProvider.GetService<IBazService>();
        }

        Assert.That(instance1, Is.Not.SameAs(instance2));
    }

    [Test]
    public void ServiceProvider_CannotResolveScopedService_Directly()
    {
        _services.RegisterScoped<IBazService, BazService>();
        _provider = _services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() => _provider.GetService<IBazService>());
    }

    [Test]
    public void ScopedServiceProvider_DisposesScopedServices_OnScopeDisposal()
    {
        _services.RegisterScoped<DisposableService, DisposableService>();
        _provider = _services.BuildServiceProvider();

        var scope = _provider.CreateScope();
        var serviceInstance = scope.ServiceProvider.GetService<DisposableService>();
        Assert.IsFalse(serviceInstance.Disposed);

        scope.Dispose();
        Assert.IsTrue(serviceInstance.Disposed);
    }

    [Test]
    public void ScopedServiceProvider_CanDisposeMultipleTimes_WithoutError()
    {
        _services.RegisterScoped<DisposableService, DisposableService>();
        _provider = _services.BuildServiceProvider();

        var scope = _provider.CreateScope();
        scope.Dispose();
        Assert.DoesNotThrow(() => scope.Dispose());
    }

    [Test]
    public void ServiceProvider_DisposesSingletonServices_OnContainerDisposal()
    {
        _services.RegisterSingleton<DisposableService, DisposableService>();
        _provider = _services.BuildServiceProvider();

        var serviceInstance = _provider.GetService<DisposableService>();
        Assert.IsFalse(serviceInstance.Disposed);

        (_provider as IDisposable).Dispose();
        Assert.IsTrue(serviceInstance.Disposed);
    }

    [Test]
    public void ServiceProvider_ThrowsException_WhenUnregisteredServiceRequested()
    {
        _provider = _services.BuildServiceProvider();
        Assert.Throws<InvalidOperationException>(() => _provider.GetService<IBazService>());
    }

    // TODO: Decide what we want to do when multiple instances of the same type and scope have been registered. We can either ignore multiple registrations, resolve to the last registration, or resolve all registered implementations depending on the expected behavior. Or just throw an exception.
    // [Test]
    // public void ServiceCollection_ThrowsException_WhenDuplicateSingletonRegistered()
    // {
    //     _services.RegisterSingleton<IFooService, FooService>();
    //     Assert.Throws<InvalidOperationException>(() => _services.RegisterSingleton<IFooService, FooService>());
    // }

    [Test]
    public void ServiceProvider_DisposesOnlyDisposableServices()
    {
        _services.RegisterSingleton<IFooService, FooService>();
        _services.RegisterSingleton<IBarService, DisposableBarService>();
        _provider = _services.BuildServiceProvider();

        var disposableBarService = (DisposableBarService) _provider.GetService<IBarService>();
        var fooService = _provider.GetService<IFooService>();

        Assert.IsFalse(disposableBarService.Disposed);

        (_provider as IDisposable).Dispose();

        Assert.IsTrue(disposableBarService.Disposed);
        // FooService does not implement IDisposable, should not have Dispose method.
        Assert.That(fooService, Is.Not.AssignableFrom<IDisposable>());
    }

    [Test]
    public void ServiceProvider_ShouldAllowDisposing_WhenNoServicesRegistered()
    {
        _provider = _services.BuildServiceProvider();
        Assert.DoesNotThrow(() => (_provider as IDisposable).Dispose());
    }

    [Test]
    public void ServiceProvider_CatchesExceptions_WhenDisposingServices()
    {
        _services.RegisterSingleton<IDisposableService, FaultyDisposableService>();
        _provider = _services.BuildServiceProvider();
        var faultyService = _provider.GetService<IDisposableService>();

        Assert.DoesNotThrow(() => (_provider as IDisposable).Dispose());
        // Here you may also want to verify a log entry or error handling mechanism.
    }

    // Additional helper classes for testing purposes
    public class DisposableBarService : IBarService, IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }
    
    public interface IDisposableService : IDisposable
    {
        // Additional members, if any
    }
    
    // An alternative implementation of DisposableService that throws an exception in Dispose.
    public class FaultyDisposableService : IDisposableService
    {
        public void Dispose()
        {
            throw new InvalidOperationException("Error during disposal");
        }
    }

    // Define dummy services for testing purposes
    public interface IFooService
    {
    }

    public class FooService : IFooService
    {
    }

    public interface IBarService
    {
    }

    public class BarService : IBarService
    {
    }

    public interface IBazService
    {
    }

    public class BazService : IBazService
    {
    }

    public class DisposableService : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}