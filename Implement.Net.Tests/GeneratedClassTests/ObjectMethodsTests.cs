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
	public sealed class ObjectMethodsTests : AbstractGeneratedInstanceTest {
		private static readonly MethodInfo Finalize = typeof(object).GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;

		[Fact]
		public void EqualsBound() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Bind, CallbackHandler);

			_ = instance.Equals(null);
			Assert.True(called);
		}

		[Fact]
		public void EqualsForwarded() {
			ObjectMethodHandler handler = new ();
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Forward, handler);

			_ = instance.Equals(null);
			Assert.True(handler.EqualsCalled);
		}

		[Fact]
		public void EqualsIgnored() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Ignore, CallbackHandler);

			_ = instance.Equals(null);
			Assert.False(called);
		}

		[Fact]
		public void FinalizeBound() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Bind, CallbackHandler);

			Finalize.Invoke(instance, Array.Empty<object>());
			Assert.True(called);
		}

		[Fact]
		public void FinalizeForwarded() {
			bool called = false;
			ObjectMethodHandler handler = new (() => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Forward, handler);

			Finalize.Invoke(instance, Array.Empty<object>());
			Assert.True(called);
		}

		[Fact]
		public void FinalizeIgnored() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Ignore, CallbackHandler);

			Finalize.Invoke(instance, Array.Empty<object>());
			Assert.False(called);
		}

		[Fact]
		public void GetHashCodeBound() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Bind, CallbackHandler);

			_ = instance.GetHashCode();
			Assert.True(called);
		}

		[Fact]
		public void GetHashcodeForwarded() {
			ObjectMethodHandler handler = new ();
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Forward, handler);

			_ = instance.GetHashCode();
			Assert.True(handler.GetHashCodeCalled);
		}

		[Fact]
		public void GetHashCodeIgnored() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Ignore, CallbackHandler);

			_ = instance.GetHashCode();
			Assert.False(called);
		}

		[Fact]
		public void GetTypeIgnored() {
			// We use GetType() as an example of a non-virtual method we can't override
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Ignore, CallbackHandler);

			_ = instance.GetType();
			Assert.False(called);
		}

		[Fact]
		public void ToStringBound() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Bind, CallbackHandler);

			_ = instance.ToString();
			Assert.True(called);
		}

		[Fact]
		public void ToStringForwarded() {
			ObjectMethodHandler handler = new ();
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Forward, handler);

			_ = instance.ToString();
			Assert.True(handler.ToStringCalled);
		}

		[Fact]
		public void ToStringIgnored() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IEmptyInterface instance = CreateInstanceWithObjectMethods<IEmptyInterface>(GenerationOptions.EObjectMethodBehaviour.Ignore, CallbackHandler);

			_ = instance.ToString();
			Assert.False(called);
		}

		private TInterface CreateInstanceWithObjectMethods<TInterface>(GenerationOptions.EObjectMethodBehaviour behaviour, IDynamicHandler? handler = null) where TInterface : class => CreateTypeAndInstanceWithObjectMethods<TInterface>(behaviour, handler).Instance;

		private (Type Type, TInterface Instance) CreateTypeAndInstanceWithObjectMethods<TInterface>(GenerationOptions.EObjectMethodBehaviour behaviour, IDynamicHandler? handler = null) where TInterface : class {
			Options.ObjectMethodBehaviour = behaviour;
			Type type = CreateType<TInterface>(Options);

			return (type, CreateInstance<TInterface>(type, handler));
		}
	}
}
