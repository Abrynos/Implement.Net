# Implement.Net

## Description

Implement.Net is a C# library with the primary purpose of implementing interfaces at runtime by forwarding calls to their members to a statically typed handler.

## Usage

### Step 0: Verification

Make sure this is what you need. There is a very limited amount of actual use cases for this and you might be better off using something else.

### Step 1: Dependency

From the command line add the dependency to your project:

```bash
dotnet add package Implement.Net
```

Alternatively you can edit your `.csproj` file directly. Just make sure you replace the `*` from the example below with the version you want to use:

```xml
	<ItemGroup>
		<PackageReference Include="Implement.Net" Version="*" />
	</ItemGroup>
```

In addition to the official NuGet registry we also push to [GitHub packages](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry).

### Step 2: Handler implementation

This is the single most important piece of code you are going to write if you use this library. Here you are going to handle any access to the methods/properties/events of the interface you generated an implementation for. Most likely you will either go by name or some implementations of `Attribute`.

```c#
using Implement.Net;

internal sealed class MyHandler : IDynamicHandler {
	public bool TryAddEventHandler(Type declaringType, string eventName, object? value) {
		// TODO
		return true;
	}
	public bool TryGetProperty(Type declaringType, string propertyName, out object? result) {
		// TODO
		result = null;
		return true;
	}
	public bool TryInvokeMethod(MethodInfo method, object?[] arguments, out object? result) {
		// TODO
		result = null;
		return true;
	}
	public bool TryRemoveEventHandler(Type declaringType, string eventName, object? value) {
		// TODO
		return true;
	}
	public bool TrySetProperty(Type declaringType, string propertyName, object? value) {
		// TODO
		return true;
	}
}
```

Your IDE should be able to show you our documentation if you hover over the method names. This should explain to you what they are expected to return and set to their `out` parameters.

### Step 3: Interface creation

You can most likely skip this step, as the primary use-case for this is to provide the users of YOUR library a declarative `Attribute`-API to work with.

```c#
public interface MyInterface {
	public int SomeValue { get; set; }
}
```

### Step 4: Type generation

As mentioned earlier: The type `MyInterface` will probably be something your library receives itself via generic parameter or `Type`-Object. This is not a problem. The process works for these cases as well.

```c#
TypeFactory typeFactory = new ();
GenerationOptions options = new ();
Type createdType = typeFactory.CreateType<MyInterface>(options);
MyInterface instance = (Activator.CreateInstance(createdType, new DefaultHandler()) as IEmptyInterface)!;
instance.SomeValue = 42;
```

To find out about all the configuration options and method overloads you have, you can once again visit our documentation.

## Options and Restrictions

We have extensive documentation in form of XML-comments in our source code. These are also included in our NuGet package and should be available in your IDE. We invite you to open a discussion in our GitHub repository if anything is unclear. Please be aware that discussions are different from issues. Issues are purely for development purposes, such as feature requests and bug reports. Incorrect use will get them closed with a canned response in no time at all.

There are several pre-conditions that must be met for type-generation to succeed. Most notably are the need for the interface to be `public` and the prohibition of unbound generic methods within it.

Here are some examples of what will or will not work:

```c#
public interface MySecondInterface<TInterfaceParameter> {
	// This will work
	TInterfaceParameter SomeProperty { get; }
	
	// This will work as well
	TInterfaceParameter SomeMethod(TInterfaceParameter m);
	
	// Even this will work
	delegate TInterfaceParameter SomeDelegate(TInterfaceParameter m);
	event SomeDelegate SomeEvent;
	
	// This will fail
	void SomeMethod<TMethodParameter>(TMethodParameter p);
}
```

```c#
// This will not work - The interface has to be public
internal interface MyThirdInterface { }
```

```c#
TypeFactory typeFactory = new ();
// This will work
typeFactory.CreateType(typeof(SomeGenericInterface<int>));
// This will fail - There are unbound generic parameters
typeFactory.CreateType(typeof(SomeGenericInterface<>));
// This will fail - You can derive from it tho
typeFactory.CreateType(typeof(IDisposable));
// This will fail - TypeFactory is not an interface
typeFactory.CreateType(typeof(TypeFactory));
```

## FAQ

### What's the difference between this and the `dynamic` keyword?

This allows for static typing. The "end user" can use a predefined interface instead of `dynamic`.
