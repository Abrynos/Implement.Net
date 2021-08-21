# Implement.Net

[![CI status](https://img.shields.io/github/workflow/status/Abrynos/Implement.Net/CI?label=Build&logo=GitHub)](https://github.com/Abrynos/Implement.Net/actions/workflows/ci.yml)
[![Test status](https://img.shields.io/github/workflow/status/Abrynos/Implement.Net/CI?label=Tests&logo=GitHub)](https://github.com/Abrynos/Implement.Net/actions/workflows/ci.yml)
[![GitHub last commit](https://img.shields.io/github/last-commit/Abrynos/Implement.Net?label=Updated&logo=GitHub)](https://github.com/Abrynos/Implement.Net/commits)
[![open issues](https://img.shields.io/github/issues/Abrynos/Implement.Net?label=Issues&logo=GitHub)](https://github.com/Abrynos/Implement.Net/issues)
[![open merge requests](https://img.shields.io/github/issues-pr/Abrynos/Implement.Net?label=Merge%20requests&logo=GitHub)](https://github.com/Abrynos/Implement.Net/pulls)

[![dotnet version](https://img.shields.io/badge/Version-net5.0-brightgreen?logo=csharp)](https://dotnet.microsoft.com/download/dotnet/5.0)
[![license](https://img.shields.io/github/license/Abrynos/Implement.Net?label=License)](https://github.com/Abrynos/Implement.Net/blob/master/LICENSE.txt)

[![GitHub release date](https://img.shields.io/github/release-date/Abrynos/Implement.Net?label=Release&logo=GitHub)](https://github.com/Abrynos/Implement.Net/releases)
[![GitHub release](https://img.shields.io/github/v/release/Abrynos/Implement.Net?label=GitHub&logo=GitHub)](https://github.com/Abrynos/Implement.Net/releases)
[![Nuget](https://img.shields.io/nuget/v/Implement.Net?label=NuGet&logo=NuGet)](https://www.nuget.org/packages/Implement.Net)

## Description

Implement.Net is a C# library with the primary purpose of implementing interfaces at runtime by forwarding calls to their members to a statically typed handler.

## Usage

### Step 0: Verification

Make sure this is what you need. There is a very limited amount of actual use cases for this and you might be better off using something else.

#### When should I (not) use this library?

- When you control what will be implemented (e.g. you want a method called `MultiplyWithTwo` that works for all numeric types) you have following possibilities:
	- Use `dynamic` - This is a bad idea with a lot of runtime overhead, but it will work.
	- Manually implement the method for all numeric types, since a generic version using the multiplication operator won't compile - This works but you'll have to copy the code a bunch of times.
	- Implement the generic method `T MutliplyWithTwo<T>(T t);` using the `System.Linq.Expressions` namespace and compiled expressions - You should definitely go with this!
- When you **can't** control what will be implemented (You create a library with a few attributes the user can put on their own interface and you are expected to return an instance of said interface; e.g. you want to create the dotnet equivalent of [OpenFeign](https://github.com/OpenFeign/feign)):
	- You can **not** use [Moq](https://github.com/moq/moq4), since that would require you to know the methods and properties in advance for calls like `mock.Setup(foo => foo.TheMethodYouDontKnowAbout()).Throws<Exception>();`.
	- You force the users of your library to use `dynamic` and you go by name and arguments of whatever they call - Apart from the runtime overhead of `dynamic` they have no possibility of fine-tuning the behaviour of your implementation via e.g. attributes.
	- You use this library - This statically binds everything, gives you access to reflection (including attributes) and even handles `IDisposable` for you.

### Step 1: Installation

From the command line add the [NuGet package](https://www.nuget.org/packages/Implement.Net) to your project:

```bash
dotnet add package Implement.Net
```

Alternatively you can edit your `.csproj` file directly. Just make sure you replace the `*` from the example below with the version you want to use:

```xml
<ItemGroup>
	<PackageReference Include="Implement.Net" Version="*"/>
</ItemGroup>
```

In addition to the official package registry on [nuget.org](https://nuget.org/) we also push to [GitHub packages](https://github.com/Abrynos/Implement.Net/packages/932196).

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

## Documentation

We have extensive documentation in form of XML-comments in our source code. These are also included in our NuGet package and should be available in your IDE.

We invite you to open a [discussion](https://github.com/Abrynos/Implement.Net/discussions) in our GitHub repository if anything is unclear. Please be aware that discussions are different from issues. Issues are purely for development purposes, such as feature requests and bug reports. Incorrect use will get them closed with a canned response in no time at all.
