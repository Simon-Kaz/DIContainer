// Common/TestServices.cs

namespace Tests.Common;

public class ServiceWithMultipleConstructors
{
    public IFooService FooService { get; }
    public IBarService BarService { get; }
    
    // Default constructor
    public ServiceWithMultipleConstructors()
    {
    }

    // Constructor with one parameter
    public ServiceWithMultipleConstructors(IFooService fooService)
    {
        FooService = fooService;
    }

    // Constructor with two parameters
    public ServiceWithMultipleConstructors(IBarService barService, IFooService fooService)
    {
        BarService = barService;
        FooService = fooService;
    }
}

public class ServiceWithUnresolvableConstructor
{
    public INonRegisteredService NonRegisteredService { get; }

    public ServiceWithUnresolvableConstructor(INonRegisteredService nonRegisteredService)
    {
        NonRegisteredService = nonRegisteredService;
    }
}

public class ServiceWithMixedConstructors
{
    public IFooService FooService { get; }
    public IOptionalService OptionalService { get; }

    public ServiceWithMixedConstructors()
    {
    }

    public ServiceWithMixedConstructors(IFooService fooService)
    {
        FooService = fooService;
    }

    public ServiceWithMixedConstructors(IFooService fooService, IOptionalService optionalService = null)
    {
        FooService = fooService;
        OptionalService = optionalService;
    }
}

public class ServiceWithCompetingConstructors
{
    public IFooService FooService { get; }
    public IBarService BarService { get; }

    // First constructor
    public ServiceWithCompetingConstructors(IFooService fooService, IOptionalService optionalService)
    {
        FooService = fooService;
    }

    // Second constructor
    public ServiceWithCompetingConstructors(IBarService barService, IOptionalService optionalService)
    {
        BarService = barService;
    }
}

public class OptionalService : IOptionalService
{
}

public interface INonRegisteredService { }
public interface IOptionalService { }