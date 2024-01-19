using SampleConsoleApp.Interfaces;

namespace SampleConsoleApp.Services;

public class SingletonService : ISingletonService
{
    private readonly Guid _identifier = Guid.NewGuid();
    public void PrintIdentifier()
    {
        Console.WriteLine($"Singleton has identifier: {_identifier}");
    }
}