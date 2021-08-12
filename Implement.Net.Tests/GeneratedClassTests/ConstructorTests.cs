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
using System.Linq;
using System.Reflection;
using Implement.Net.Tests.Utilities.Handlers;
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	public sealed class ConstructorTests : AbstractGeneratedInstanceTest {
		[Fact]
		public void CorrectHandlerForConfiguredHandlerType() {
			Options.HandlerType = typeof(DefaultHandler);
			Type type = CreateType<IEmptyInterface>(Options);

			CreateInstanceViaConstructor<IEmptyInterface>(type, DefaultHandler);
		}

		[Fact]
		public void CorrectHandlerTypeForDefault() {
			IEmptyInterface? instance = CreateInstanceViaConstructor<IEmptyInterface>(handler: DefaultHandler);
			Assert.NotNull(instance);
		}

		[Fact]
		public void DerivedHandlerForConfiguredHandlerType() {
			Options.HandlerType = typeof(DefaultHandler);
			Type type = CreateType<IEmptyInterface>(Options);

			CreateInstanceViaConstructor<IEmptyInterface>(type, new DerivedDefaultHandler());
		}

		[Fact]
		public void IncorrectHandlerTypeForDefault() {
			Options.HandlerType = typeof(DefaultHandler);
			Type type = CreateType<IEmptyInterface>(Options);

			// This exception will be thrown by the runtime if our constructor is correctly typed to the configured HandlerType instead of IDynamicHandler
			Assert.Throws<ArgumentException>(() => CreateInstanceViaConstructor<IEmptyInterface>(type, new CallbackHandler()));
		}

		// This exception will be thrown by the runtime if our constructor is correctly typed to IDynamicHandler
		[Fact]
		public void NonHandlerObject() => Assert.Throws<ArgumentException>(() => CreateInstanceViaConstructor<IEmptyInterface>(handler: new object()));

		[Fact]
		public void NullHandler() {
			Type type = TypeFactory.CreateType<IEmptyInterface>();
			IDynamicHandler? parameter = null;

			// Since we invoke the constructor via reflection we expect anything thrown from inside to be wrapped inside a TargetInvocationException
			TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => CreateInstanceViaConstructor<IEmptyInterface>(type, parameter));

			// We do not only want to make sure that we throw, but also what we throw
			Assert.NotNull(exception.InnerException);
			ArgumentNullException argNullException = Assert.IsType<ArgumentNullException>(exception.InnerException);

			// And where we throw (mainly the exception coming from a constructor and the throwing class being in our generated namespace)
			MethodBase? throwingMethod = argNullException.TargetSite;
			Assert.NotNull(throwingMethod);
			Assert.True(throwingMethod!.IsConstructor);
			Assert.StartsWith(TypeFactory.GeneratedTypeNamespace, throwingMethod.DeclaringType!.FullName!, StringComparison.InvariantCulture);
		}

		private TInterface? CreateInstanceViaConstructor<TInterface>(Type? type = null, object? handler = null) where TInterface : class {
			type ??= CreateType<TInterface>();

			ConstructorInfo constructor = type.GetConstructors().First();

			return constructor.Invoke(new[] { handler }) as TInterface;
		}

		private sealed class DerivedDefaultHandler : DefaultHandler { }
	}
}
