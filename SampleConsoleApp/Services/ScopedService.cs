using SampleConsoleApp.Interfaces;

namespace SampleConsoleApp.Services;

public class ScopedService : IScopedService
{
    private readonly Guid _identifier = Guid.NewGuid();
    public void PrintIdentifier()
    {
        Console.WriteLine($"Scoped has identifier: {_identifier}");
    }
}