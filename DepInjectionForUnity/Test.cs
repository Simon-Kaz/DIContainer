namespace DepInjectionForUnity;

public class Test
{
    // Defining some services and their implementations for test purposes
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
        public BarService(IFooService fooService)
        {
        }
    }

    public void TestDependencyInjection()
    {
        // Set up the service collection and register services
        var services = new ServiceCollection();
        services.RegisterSingleton<IFooService, FooService>();
        services.RegisterTransient<IBarService, BarService>();

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();

        // Resolve a singleton service
        var fooService = serviceProvider.GetService<IFooService>();

        // Create a scope and resolve a transient service within that scope
        using (var scope = serviceProvider.CreateScope())
        {
            var barService = scope.ServiceProvider.GetService<IBarService>();
        }
    }
}