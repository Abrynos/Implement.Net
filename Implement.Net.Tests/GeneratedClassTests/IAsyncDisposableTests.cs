//   _____                _                           _     _   _      _
//  |_   _|              | |                         | |   | \ | |    | |
//    | | _ __ ___  _ __ | | ___ _ __ ___   ___ _ __ | |_  |  \| | ___| |_
//    | || '_ ` _ \| '_ \| |/ _ \ '_ ` _ \ / _ \ '_ \| __| | . ` |/ _ \ __|
//   _| || | | | | | |_) | |  __/ | | | | |  __/ | | | |_ _| |\  |  __/ |_
//   \___/_| |_| |_| .__/|_|\___|_| |_| |_|\___|_| |_|\__(_)_| \_/\___|\__|
//                 | |
//                 |_|
// 
// Copyright (C) 2021-2021 Sebastian Göls
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation;
// version 2.1 of the License only.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301
// USA

using System;
using System.Reflection;
using System.Threading.Tasks;
using Implement.Net.Tests.Utilities;
using Implement.Net.Tests.Utilities.Handlers;
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	// ReSharper disable once InconsistentNaming
#pragma warning disable CA2000
#pragma warning disable CA2007
	public sealed class IAsyncDisposableTests : AbstractGeneratedInstanceTest {
		private static readonly MethodInfo Finalize = typeof(object).GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;

		[Fact]
		public void BooleanOptionForcesImplementation() {
			Options.EnforceDisposable = true;
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			Assert.True(type.IsAssignableTo(typeof(IAsyncDisposable)));
		}

		[Fact]
		public async Task DisposesHandlerIfNeededByBooleanOption() {
			Options.EnforceDisposable = true;
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			AsyncDisposableDefaultHandler handler = new ();
			IEmptyInterface instance = CreateInstance<IEmptyInterface>(type, handler);

			await DisposeAsync(instance);
			Assert.True(handler.Disposed);
		}

		[Fact]
		public async Task DisposesHandlerIfNeededByHandlerType() {
			Options.HandlerType = typeof(AsyncDisposableDefaultHandler);
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			AsyncDisposableDefaultHandler handler = new ();
			IEmptyInterface instance = CreateInstance<IEmptyInterface>(type, handler);

			await DisposeAsync(instance);
			Assert.True(handler.Disposed);
		}

		[Fact]
		public async Task DisposesHandlerIfNeededByInterface() {
			AsyncDisposableDefaultHandler handler = new ();
			IEmptyIAsyncDisposableInterface instance = CreateInstance<IEmptyIAsyncDisposableInterface>(handler);

			await DisposeAsync(instance);
			Assert.True(handler.Disposed);
		}

		[Fact]
		public void DisposesInFinalize() {
			AsyncDisposableDefaultHandler handler = new ();
			Options.HandlerType = handler.GetType();
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			IEmptyInterface instance = CreateInstance<IEmptyInterface>(type, handler);
			Finalize.Invoke(instance, Array.Empty<object>());

			Assert.True(handler.Disposed);
		}

		[Fact]
		public async Task DisposesNothingIfNotNecessary() {
			Options.EnforceDisposable = true;
			Type type = TypeFactory.CreateType<IEmptyIDisposableInterface>(Options);

			IEmptyIDisposableInterface instance = CreateInstance<IEmptyIDisposableInterface>(type, DefaultHandler);

			// We will assume no exception from not trying to call the non-existing DisposeAsync()-method on the handler good enough here.
			await DisposeAsync(instance);
		}

		[Fact]
		public async Task DoesNotDisposeDuringGCAfterDisposingManually() {
			uint timesCalled = 0;
			AsyncDisposableCallbackHandler handler = new ();
			handler.DisposeAction = () => ++timesCalled;

			await CreateAndUse(() => CreateInstance<IEmptyIAsyncDisposableInterface>(handler), instance => instance.DisposeAsync().AsTask());

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.Equal((uint) 1, timesCalled);
		}

		[Fact]
		public void DoesNotDisposeInFinalizeIfNotNecessary() {
			Options.EnforceDisposable = true;
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			DefaultHandler handler = new ();
			IEmptyInterface instance = CreateInstance<IEmptyInterface>(type, handler);

			// We will assume no exception from not trying to call the non-existing DisposeAsync()-method on the handler good enough here.
			Finalize.Invoke(instance, Array.Empty<object>());
		}

		[Fact]
		public async Task Exception() {
			AsyncDisposableCallbackHandler handler = new ();
			handler.DisposeAction = () => throw new TestException();

			IEmptyIAsyncDisposableInterface instance = CreateInstance<IEmptyIAsyncDisposableInterface>(handler);

			await Assert.ThrowsAsync<TestException>(() => instance.DisposeAsync().AsTask());

			// Cleanup - garbage collector will try to dispose this later and an exception could mess up other tests running at the same time
			handler.DisposeAction = () => { };
		}

		[Fact]
		public async Task FinalizeCalledByGC() {
			AsyncDisposableDefaultHandler handler = new ();
			Options.HandlerType = handler.GetType();
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			await CreateAndUse(() => CreateInstance<IEmptyInterface>(type, handler));

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.True(handler.Disposed);
		}

		[Fact]
		public void HandlerTypeForcesImplementation() {
			Options.HandlerType = typeof(AsyncDisposableDefaultHandler);
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			bool assignable = type.IsAssignableTo(typeof(IAsyncDisposable));
			Assert.True(assignable);
		}

		[Fact]
		public void InterfaceForcesImplementation() {
			Type type = TypeFactory.CreateType<IEmptyIAsyncDisposableInterface>();

			bool assignable = type.IsAssignableTo(typeof(IAsyncDisposable));
			Assert.True(assignable);
		}

		[Fact]
		public async Task NoForwardingToHandler() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			Type type = TypeFactory.CreateType<IEmptyIAsyncDisposableInterface>();
			IEmptyIAsyncDisposableInterface instance = CreateInstance<IEmptyIAsyncDisposableInterface>(type, CallbackHandler);

			await DisposeAsync(instance);
			Assert.False(called);
		}

		[Fact]
		public void NoImplementation() {
			Type type = TypeFactory.CreateType<IEmptyInterface>();

			bool assignable = type.IsAssignableTo(typeof(IAsyncDisposable));
			Assert.False(assignable);
		}

		// We use this method to test the garbage collector
		// If the creator really creates a new instance and the action does not save a reference to it, it will be in a collectible state after the function finishes execution
		private static async Task CreateAndUse<T>(Func<T> creator, Func<T, Task>? action = null) {
			T instance = creator();
			await (action?.Invoke(instance) ?? Task.CompletedTask);
		}

		private static ValueTask DisposeAsync(object obj) {
			if (obj is not IAsyncDisposable asyncDisposable) {
				throw new InvalidOperationException();
			}

			return asyncDisposable.DisposeAsync();
		}
	}
#pragma warning restore CA2007
#pragma warning restore CA2000
}
