using SimpleDI.Services;
using SampleConsoleApp.Interfaces;
using SampleConsoleApp.Services;
using SimpleDI.Providers;
using IServiceProvider = SimpleDI.Providers.IServiceProvider;

// Set up DI system
IServiceCollection services = new ServiceCollection();
services.RegisterSingleton<ISingletonService, SingletonService>();
services.RegisterTransient<ITransientService, TransientService>();
services.RegisterScoped<IScopedService, ScopedService>();

// Build the provider from the service collection
IServiceProvider serviceProvider = services.BuildServiceProvider();

// Use the Singleton service
var singletonService = serviceProvider.GetService<ISingletonService>();
singletonService.PrintIdentifier(); // This time should always be the same

// Use the Transient service
var transientService1 = serviceProvider.GetService<ITransientService>();
transientService1.PrintTime(); // This time should be unique every time

var transientService2 = serviceProvider.GetService<ITransientService>();
transientService2.PrintTime(); // Different from the previous time

// Use the Scoped service within a scope
using (var scope1 = serviceProvider.CreateScope())
{
    var scopedService1 = scope1.ServiceProvider.GetService<IScopedService>();
    scopedService1.PrintIdentifier(); // This time should stay the same within this scope

    var scopedService2 = scope1.ServiceProvider.GetService<IScopedService>();
    scopedService2.PrintIdentifier(); // Same as above, because it's in the same scope
}

// Use the Scoped service within another scope
using (var scope2 = serviceProvider.CreateScope())
{
    var scopedService3 = scope2.ServiceProvider.GetService<IScopedService>();
    scopedService3.PrintIdentifier(); // This time should be different from the previous scope's time
}

// Request singleton again
var singletonService2 = serviceProvider.GetService<ISingletonService>();
singletonService2.PrintIdentifier(); // This time should always be the same

// Using the extension method to resolve a Lazy<ISomeService> instance.
var lazyService = serviceProvider.GetLazyService<ITransientService>();
// The service within the Lazy<T> container is only created when the `.Value` property is accessed.
Console.WriteLine($"Was service initialized? {lazyService.IsValueCreated}");
lazyService.Value.PrintTime();
Console.WriteLine($"Was service initialized? {lazyService.IsValueCreated}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();