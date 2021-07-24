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

using System.Reflection;
using Implement.Net.Tests.Utilities;
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	/**
	 * If an interface derives from another one we want to make sure that the meta-information passed into the handler is referencing the type that originally declared the member and not the derived one.
	 */
	public class DerivationTests : AbstractGeneratedInstanceTest {
		private readonly Person Person = new ();

		[Fact]
		public void AddTypeParameterMatches() {
			CallbackHandler.EventAdder = EventModifier(true, (type, _, _) => Assert.Equal(typeof(IEventInterface), type));
			IEventInterface instance = CreateInstance<IDeriveOthers>(CallbackHandler);

			instance.SomeEvent += null;
		}

		[Fact]
		public void GetTypeParameterMatches() {
			IDeriveOthers instance = CreateInstance<IDeriveOthers>(CallbackHandler);

			CallbackHandler.PropertyGetter = PropertyGetter(true, (type, _) => Assert.Equal(typeof(IPropertyInterface), type));
			_ = instance.ValueType;

			CallbackHandler.PropertyGetter = PropertyGetter(true, (type, _) => Assert.Equal(typeof(IPropertyInterface), type));
			_ = instance.Object;

			CallbackHandler.PropertyGetter = PropertyGetter(true, (type, _) => Assert.Equal(typeof(IPropertyInterface), type));
			_ = instance.GetOnlyValueType;

			CallbackHandler.PropertyGetter = PropertyGetter(true, (type, _) => Assert.Equal(typeof(IPropertyInterface), type));
			_ = instance.GetOnlyObject;
		}

		[Fact]
		public void MethodInfoParameterMatches() {
			IDeriveOthers instance = CreateInstance<IDeriveOthers>(CallbackHandler);

			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => AssertMethodAndDeclaringType<IMethodInterface>(method, nameof(instance.Void)));
			instance.Void();

			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => AssertMethodAndDeclaringType<IMethodInterface>(method, nameof(instance.ObjectParameter)));
			instance.ObjectParameter(null);

			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => AssertMethodAndDeclaringType<IMethodInterface>(method, nameof(instance.ValueTypeParameter)));
			instance.ValueTypeParameter(42);

			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => AssertMethodAndDeclaringType<IMethodInterface>(method, nameof(instance.MixedTypeParameters)));
			instance.MixedTypeParameters(42, null);
		}

		[Fact]
		public void RemoveTypeParameterMatches() {
			CallbackHandler.EventRemover = EventModifier(true, (type, _, _) => Assert.Equal(typeof(IEventInterface), type));
			IEventInterface instance = CreateInstance<IDeriveOthers>(CallbackHandler);

			instance.SomeEvent -= null;
		}

		[Fact]
		public void SetTypeParameterMatches() {
			IDeriveOthers instance = CreateInstance<IDeriveOthers>(CallbackHandler);

			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.ValueType = 42;

			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.Object = Person;

			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.SetOnlyValueType = 42;

			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.SetOnlyObject = Person;
		}

		private static void AssertMethodAndDeclaringType<TExpectedDeclaringType>(MethodInfo method, string expectedUniqueMethodName) {
			MethodInfo expectedMethod = typeof(TExpectedDeclaringType).GetMethod(expectedUniqueMethodName)!;
			Assert.Equal(expectedMethod, method);
			Assert.Equal(typeof(IMethodInterface), method.DeclaringType);
		}
	}
}
