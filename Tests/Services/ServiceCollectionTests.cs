using DepInjectionForUnity.Services;
using Tests.Common;

namespace Tests.Services;

public class ServiceCollectionTests
{
    private IServiceCollection _services;

    [SetUp]
    public void SetUp()
    {
        _services = new ServiceCollection();
    }

    [Test]
    public void BuildingProvider_ThrowsForAbstractClass()
    {
        _services.RegisterSingleton<AbstractService, AbstractService>();
        Assert.Throws<InvalidOperationException>(() => _services.BuildServiceProvider(),
            "Cannot instantiate an abstract class.");
    }

    [Test]
    public void BuildingProvider_ThrowsForInterface()
    {
        _services.RegisterSingleton<IFooService, IFooService>();
        Assert.Throws<InvalidOperationException>(() => _services.BuildServiceProvider(),
            "Cannot instantiate an interface.");
    }

    [Test]
    public void BuildingProvider_ThrowsForClassWithoutPublicConstructor()
    {
        _services.RegisterSingleton<ServiceWithPrivateConstructor, ServiceWithPrivateConstructor>();
        Assert.Throws<InvalidOperationException>(() => _services.BuildServiceProvider(),
            "Cannot instantiate a class without a public constructor.");
    }
    

    // TODO: Decide what we want to do when multiple instances of the same type and scope have been registered.
    // TODO: We can either ignore multiple registrations, resolve to the last registration, or resolve all registered implementations. Or just throw an exception.
    // [Test]
    // public void ServiceCollection_ThrowsException_WhenDuplicateSingletonRegistered()
    // {
    //     _services.RegisterSingleton<IFooService, FooService>();
    //     Assert.Throws<InvalidOperationException>(() => _services.RegisterSingleton<IFooService, FooService>());
    // }
}