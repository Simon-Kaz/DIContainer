namespace Tests.Common;

public interface IFooService { }

public class FooService : IFooService { }

public interface IBarService { }

public class BarService : IBarService { }

public class DisposableService : IBarService, IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}

public interface IBazService { }

public class BazService : IBazService { }

public abstract class AbstractService
{
    // This class is abstract and should not be directly instantiated in tests.
}

public class ServiceWithPrivateConstructor
{
    // Using a private constructor to simulate a class that cannot be instantiated by the DI.
    private ServiceWithPrivateConstructor() { }
}

public class ServiceWithInternalConstructor
{
    // This class has an internal constructor and should also not be directly instantiated by the DI.
    internal ServiceWithInternalConstructor() { }
}

public class ServiceWithCyclicDependency
{
    // This class introduces a cyclic dependency, used to test the DI's capability to handle such scenarios.
    public ServiceWithCyclicDependency(ServiceWithCyclicDependency self) { }
}

// This service is designed to throw an exception during disposal, for testing the container's error handling.
public class FaultyDisposableService : IBarService, IDisposable
{
    public void Dispose()
    {
        throw new InvalidOperationException("Simulated exception during dispose");
    }
}