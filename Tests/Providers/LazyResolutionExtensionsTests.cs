using SimpleDI.Providers;
using SimpleDI.Services;
using Tests.Common;

namespace Tests.Providers;

[TestFixture]
public class LazyResolutionExtensionsTests
{
    [Test]
    public void ServiceProvider_ResolvesLazyServiceCorrectly()
    {
        IServiceCollection services = new ServiceCollection();
        services.RegisterTransient<IFooService, FooService>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        Lazy<IFooService> lazyService = serviceProvider.GetLazyService<IFooService>();
        Assert.IsFalse(lazyService.IsValueCreated, "The service should not be created before accessing the Value property.");

        // Accessing the Value property to create the service.
        IFooService serviceInstance = lazyService.Value;

        Assert.IsTrue(lazyService.IsValueCreated, "The service should be created after accessing the Value property.");
        Assert.IsNotNull(serviceInstance, "The service instance should not be null after creation.");
    }
}