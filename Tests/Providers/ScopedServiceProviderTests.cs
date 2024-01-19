using DIContainer.Services;
using Tests.Common;
namespace Tests.Providers;

[TestFixture]
public class ScopedServiceProviderTests
{
    private IServiceCollection _services;
    private IServiceProvider _rootProvider;
    private IServiceProvider _scopedProvider;

    [SetUp]
    public void SetUp()
    {
        _services = new ServiceCollection();
        _services.RegisterScoped<IBazService, BazService>();
        _rootProvider = _services.BuildServiceProvider();
        _scopedProvider = _rootProvider.CreateScope().ServiceProvider;
    }

    [Test]
    public void ScopedServiceProvider_ResolvesScopedServiceCorrectly()
    {
        var scopedService = _scopedProvider.GetService<IBazService>();
        Assert.IsNotNull(scopedService);
        Assert.IsInstanceOf<BazService>(scopedService);
    }

    [Test]
    public void ScopedServiceProvider_DoesNotSupportCreatingSubScopes()
    {
        Assert.Throws<NotSupportedException>(() =>
        {
            var subScopedProvider = _scopedProvider.CreateScope();
        });
    }
}