using DepInjectionForUnity.Services;
using SampleConsoleApp.Interfaces;
using SampleConsoleApp.Services;
using IServiceProvider = DepInjectionForUnity.Providers.IServiceProvider;

// Set up the DI system
IServiceCollection services = new ServiceCollection();

// Register our services with their corresponding implementations
services.RegisterSingleton<IGreeter, HelloGreeter>();
services.RegisterScoped<IDateWriter, TodayWriter>();

// Build the provider from our service collection
IServiceProvider serviceProvider = services.BuildServiceProvider();

// Create a scope to get a scoped service
using (var scope = serviceProvider.CreateScope())
{
    // Resolve a scoped service within the scope
    IDateWriter dateWriter = scope.ServiceProvider.GetService<IDateWriter>();
    dateWriter.WriteDate();
}

// Prevent the console window from closing immediately
Console.WriteLine("Press any key to exit...");
Console.ReadKey();