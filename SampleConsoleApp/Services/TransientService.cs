using SampleConsoleApp.Interfaces;

namespace SampleConsoleApp.Services;

public class TransientService : ITransientService
{
    private readonly Guid _identifier = Guid.NewGuid();

    public void PrintTime()
    {
        Console.WriteLine($"Transient has identifier: {_identifier}");
    }
}
