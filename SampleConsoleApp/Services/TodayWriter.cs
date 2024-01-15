using SampleConsoleApp.Interfaces;

namespace SampleConsoleApp.Services;

public class TodayWriter : IDateWriter
{
    private readonly IGreeter _greeter;

    public TodayWriter(IGreeter greeter)
    {
        _greeter = greeter;
    }

    public void WriteDate()
    {
        _greeter.Greet();
        Console.WriteLine(DateTime.Now.ToShortDateString());
    }
}