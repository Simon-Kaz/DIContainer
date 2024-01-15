namespace DepInjectionForUnity.Services;

/// <summary>
/// Defines the lifetime options for services in the dependency injection container.
/// </summary>
public enum ServiceLifetime
{
    Singleton,
    Transient,
    Scoped
}

/// <summary>
/// Describes a service with its service type, implementation, and lifetime.
/// </summary>
public class ServiceDescriptor
{
    /// <summary>
    /// Gets the type of the service to register.
    /// </summary>
    public Type ServiceType { get; }

    /// <summary>
    /// Gets the type that implements the service.
    /// </summary>
    public Type ImplementationType { get; }

    /// <summary>
    /// Gets the lifetime of the service.
    /// </summary>
    public ServiceLifetime Lifetime { get; }

    /// <summary>
    /// Holds an instance of the service for Singleton lifetimes.
    /// </summary>
    public object Instance { get; set; }

    /// <summary>
    /// Initializes a new instance of the ServiceDescriptor class.
    /// </summary>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The type that implements the service.</param>
    /// <param name="lifetime">The lifetime of the service.</param>
    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
        Instance = null;
    }
}