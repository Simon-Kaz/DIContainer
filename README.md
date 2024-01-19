# SimpleDI: A Dependency Injection Container for C#8 and .NET Standard 2.0

SimpleDI is a lightweight, easy-to-use Dependency Injection (DI) container designed to decouple the instantiation and configuration of dependencies from the business logic of your application. It provides a straightforward API for registering and resolving services with different lifetimes: singleton, transient, and scoped, aligning with the familiar patterns used in modern .NET applications. 

This project is tailored for systems using C#8 and targets .NET Standard 2.0, ensuring broad compatibility, including support for Unity 2021 projects.

## Features

- **Service Registration**: Simple API to register services as singleton, transient, or scoped.
- **Service Resolution**: Resolves services through constructors, supporting classes with multiple constructors.
- **Lifetime Management**: Manages instances according to their defined lifetimes, ensuring appropriate instantiation and disposal.
- **Scoping**: Supports service scoping, particularly useful for operations that require a lifespan beyond a single method call but shorter than the application's lifespan (e.g., per web request).

## Projects

The repository consists of three main projects:

### 1. DI Container Library (SimpleDI)

The core of the solution, where the DI container is implemented. This library can be used across different project types like console apps, web apps, and Unity projects.

### 2. Sample Console App

An example project that demonstrates how the DI container can be integrated into a .NET Core Console Application. It showcases the resolution and lifetime management of different service types.

### 3. Unit Tests

A comprehensive suite of unit tests verifying the behavior of the DI container, ensuring that the registration and resolution of services work as expected across various scenarios.

## Getting Started

To integrate SimpleDI into your project, reference the library in your project file, and then set up your services and their lifetimes during the application's start-up phase:

```csharp
IServiceCollection services = new ServiceCollection();
services.RegisterSingleton<IFooService, FooService>();
services.RegisterTransient<IBarService, BarService>();
services.RegisterScoped<IBazService, BazService>();

IServiceProvider serviceProvider = services.BuildServiceProvider();

// Use the serviceProvider to resolve and use services throughout your application...
```

For complete examples of service registration and usage, refer to the Sample Console App project included in this repository.

## Compatibility

SimpleDI is built targeting C#8 and .NET Standard 2.0, which allows it to be compatible with a broad range of .NET environments, including:

- .NET Core 2.0 and later
- .NET Framework 4.6.1 and later
- Mono 5.4 and later
- Xamarin.iOS 10.14 and later
- Xamarin.Mac 3.8 and later
- Xamarin.Android 8.0 and later
- Unity 2021 and later

This ensures that regardless of the platform, SimpleDI can be used consistently across different project types.

## Contributing

Contributions are welcome! Feel free to submit issues, pull requests, or enhancements to improve the project lifecycle and management. For significant changes, please open an issue first to discuss what you would like to change.

## Upcoming Features

### Integration with Unity

While SimpleDI is already compatible with Unity 2021 due to its target of .NET Standard 2.0, further integration with Unity's unique component-based architecture is in progress. This will allow for smoother integration with Unity's MonoBehaviour lifecycle and scriptable objects.

Future updates and documentation will provide guidance on:

- **Injecting Dependencies into MonoBehaviours**: Leverage constructor injection or attribute-based injection to automatically inject dependencies into your MonoBehaviours.
- **Service Registration via ScriptableObjects**: Utilize Unity's ScriptableObject architecture to register services in a way that's natural for Unity developers.
- **Lifecycle Management in Unity**: Manage lifetime scopes that make sense in the context of a game's scene management, such as per-scene singletons.

### Enhanced Constructor Selection Logic

To further refine the selection logic when multiple constructors are available, we will introduce a way to mark a preferred constructor, possibly via an attribute or configuration method. This will help avoid ambiguities and give users explicit control over which constructor should be used by the DI container during the service instantiation process.

### Property Injection

In addition to constructor injection, SimpleDI will be extended to support property injection. This will offer more flexibility in specific cases where constructor injection is not ideal or possible.

### Better Exception Handling

We aim to provide more detailed and user-friendly exception messages when the DI container encounters errors during service registration or resolution. This will make troubleshooting and debugging DI-related problems more straightforward for developers.

## Using in Unity

Guides are forthcoming that will detail using SimpleDI within your Unity projects. This will cover typical game development scenarios where DI can simplify your codebase, aid in testing, and create more manageable code.

Stay tuned for updates and examples on seamless integration with Unity's patterns and practices.

---

Please note that our DI container is in early stages of development and not intended for production.
