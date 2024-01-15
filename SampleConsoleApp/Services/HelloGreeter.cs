using SampleConsoleApp.Interfaces;

namespace SampleConsoleApp.Services;

public class HelloGreeter : IGreeter
{
    public void Greet()
    {
        Console.WriteLine("Hello, world!");
    }
}