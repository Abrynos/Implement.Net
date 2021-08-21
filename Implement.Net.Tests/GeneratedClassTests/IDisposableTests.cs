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
using Implement.Net.Tests.Utilities;
using Implement.Net.Tests.Utilities.Handlers;
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	// ReSharper disable once InconsistentNaming
#pragma warning disable CA2000
	public sealed class IDisposableTests : AbstractGeneratedInstanceTest {
		private static readonly MethodInfo Finalize = typeof(object).GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;

		[Fact]
		public void BooleanOptionForcesImplementation() {
			Options.EnforceDisposable = true;
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			bool assignable = type.IsAssignableTo(typeof(IDisposable));
			Assert.True(assignable);
		}

		[Fact]
		public void DisposesHandlerIfNeededByBooleanOption() {
			Options.EnforceDisposable = true;
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			DisposableDefaultHandler handler = new ();
			IEmptyInterface instance = CreateInstance<IEmptyInterface>(type, handler);

			Dispose(instance);
			Assert.True(handler.Disposed);
		}

		[Fact]
		public void DisposesHandlerIfNeededByHandlerType() {
			Options.HandlerType = typeof(DisposableDefaultHandler);
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			DisposableDefaultHandler handler = new ();
			IEmptyInterface instance = CreateInstance<IEmptyInterface>(type, handler);

			Dispose(instance);
			Assert.True(handler.Disposed);
		}

		[Fact]
		public void DisposesHandlerIfNeededByInterface() {
			DisposableDefaultHandler handler = new ();
			IEmptyIDisposableInterface instance = CreateInstance<IEmptyIDisposableInterface>(handler);

			Dispose(instance);
			Assert.True(handler.Disposed);
		}

		[Fact]
		public void DisposesInFinalize() {
			DisposableDefaultHandler handler = new ();
			Options.HandlerType = handler.GetType();
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			IEmptyInterface instance = CreateInstance<IEmptyInterface>(type, handler);
			Finalize.Invoke(instance, Array.Empty<object>());

			Assert.True(handler.Disposed);
		}

		[Fact]
		public void DisposesNothingIfNotNecessary() {
			Options.EnforceDisposable = true;
			Type type = TypeFactory.CreateType<IEmptyIDisposableInterface>(Options);

			IEmptyIDisposableInterface instance = CreateInstance<IEmptyIDisposableInterface>(type, DefaultHandler);

			// We will assume no exception from not trying to call the non-existing Dispose()-method on the handler good enough here.
			Dispose(instance);
		}

		[Fact]
		public void DoesNotDisposeInFinalizeAfterDisposingManually() {
			uint timesCalled = 0;
			DisposableCallbackHandler handler = new ();
			handler.DisposeAction = () => ++timesCalled;

			CreateAndUse(() => CreateInstance<IEmptyIDisposableInterface>(handler), instance => instance.Dispose());

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

			// We will assume no exception from not trying to call the non-existing Dispose()-method on the handler good enough here.
			Finalize.Invoke(instance, Array.Empty<object>());
		}

		[Fact]
		public void Exception() {
			DisposableCallbackHandler handler = new ();
			handler.DisposeAction = () => throw new TestException();

			IEmptyIDisposableInterface instance = CreateInstance<IEmptyIDisposableInterface>(handler);

			Assert.Throws<TestException>(instance.Dispose);

			// Cleanup - we have to make sure at all costs that the garbage collector does not EVER call this throwing method since that could mess up the other tests
#pragma warning disable CA1816

			// ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
			GC.SuppressFinalize(handler);

			// ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
			GC.SuppressFinalize(instance);
#pragma warning restore CA1816
		}

		[Fact]
		public void FinalizeCalledByGC() {
			DisposableDefaultHandler handler = new ();
			Options.HandlerType = handler.GetType();
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			CreateAndUse(() => CreateInstance<IEmptyInterface>(type, handler));

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.True(handler.Disposed);
		}

		[Fact]
		public void HandlerTypeForcesImplementation() {
			Options.HandlerType = typeof(DisposableDefaultHandler);
			Type type = TypeFactory.CreateType<IEmptyInterface>(Options);

			bool assignable = type.IsAssignableTo(typeof(IDisposable));
			Assert.True(assignable);
		}

		[Fact]
		public void InterfaceForcesImplementation() {
			Type type = TypeFactory.CreateType<IEmptyIDisposableInterface>();

			bool assignable = type.IsAssignableTo(typeof(IDisposable));
			Assert.True(assignable);
		}

		[Fact]
		public void NoForwardingToHandler() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			Type type = TypeFactory.CreateType<IEmptyIDisposableInterface>();
			IEmptyIDisposableInterface instance = CreateInstance<IEmptyIDisposableInterface>(type, CallbackHandler);

			Dispose(instance);
			Assert.False(called);
		}

		[Fact]
		public void NoImplementation() {
			Type type = TypeFactory.CreateType<IEmptyInterface>();

			bool assignable = type.IsAssignableTo(typeof(IDisposable));
			Assert.False(assignable);
		}

		// We use this method to test the garbage collector
		// If the creator really creates a new instance  and the action does not save a reference to it, it will be in a collectible state after the function finishes execution
		private static void CreateAndUse<T>(Func<T> creator, Action<T>? action = null) {
			T instance = creator();
			action?.Invoke(instance);
		}

		private static void Dispose(object obj) {
			if (obj is not IDisposable disposable) {
				throw new InvalidOperationException();
			}

			disposable.Dispose();
		}
	}
#pragma warning restore CA2000
}
