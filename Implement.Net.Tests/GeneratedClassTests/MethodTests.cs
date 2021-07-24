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
using Implement.Net.Tests.Utilities;
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	public sealed class MethodTests : AbstractGeneratedInstanceTest {
		private readonly Fruit Fruit = new ();

		private readonly Person Person = new ();

		[Fact]
		public void ActuallyCalled() {
			// this test passing is the precondition for all other tests because those have all their asserts inside the handler, which, in case the handler is never called, are never executed
			bool called;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			called = false;
			instance.Void();
			Assert.True(called);

			called = false;
			instance.GetValueType();
			Assert.True(called);

			called = false;
			instance.GetObject();
			Assert.True(called);

			called = false;
			instance.ValueTypeParameter(42);
			Assert.True(called);

			called = false;
			instance.ObjectParameter(null);
			Assert.True(called);

			called = false;
			instance.MixedTypeParameters(42, null);
			Assert.True(called);
		}

		[Fact]
		public void Exception() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => throw new TestException());
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Assert.Throws<TestException>(instance.Void);
		}

		[Fact]
		public void GetObject() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, Person);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Person result = instance.GetObject();
			Assert.NotNull(result);
			Assert.Same(Person, result);
		}

		[Fact]
		public void GetObjectSetsNonAssignable() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, Fruit);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(instance.GetObject);
		}

		[Fact]
		public void GetObjectSetsNull() {
			CallbackHandler.MethodInvoker = MethodInvoker(true); // default outValue is null already
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Person result = instance.GetObject();
			Assert.Null(result);
		}

		[Fact]
		public void GetObjectSetsValueType() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, 42);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(instance.GetObject);
		}

		[Fact]
		public void GetValueType() {
			const int inValue = 42;
			CallbackHandler.MethodInvoker = MethodInvoker(true, inValue);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			int result = instance.GetValueType();
			Assert.Equal(inValue, result);
		}

		[Fact]
		public void GetValueTypeSetsNonAssignable() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, true);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(() => instance.GetValueType());
		}

		[Fact]
		public void GetValueTypeSetsNull() {
			CallbackHandler.MethodInvoker = MethodInvoker(true);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			int result = instance.GetValueType();
			Assert.Equal(default(int), result);
		}

		[Fact]
		public void GetValueTypeSetsObject() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, new object());
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(() => instance.GetValueType());
		}

		[Fact]
		public void MethodInfoParameterMatches() {
			Type basicType = typeof(IMethodInterface);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => Assert.Equal(basicType.GetMethod(nameof(instance.Void)), method));
			instance.Void();
			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => Assert.Equal(basicType.GetMethod(nameof(instance.ObjectParameter)), method));
			instance.ObjectParameter(null);
			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => Assert.Equal(basicType.GetMethod(nameof(instance.ValueTypeParameter)), method));
			instance.ValueTypeParameter(42);
			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, _) => Assert.Equal(basicType.GetMethod(nameof(instance.MixedTypeParameters)), method));
			instance.MixedTypeParameters(42, null);
		}

		[Fact]
		public void MixedParameterValuesMatch() {
			const int value = 42;

			CallbackHandler.MethodInvoker = MethodInvoker(
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				true,
				(_, parameters) => {
					Assert.Equal(value, parameters.First());
					Assert.Same(Person, parameters.Skip(1).First());
				}
			);

			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.MixedTypeParameters(value, Person);
		}

		[Fact]
		public void ObjectParameterNullValueMatches() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, (_, parameters) => Assert.Null(parameters.First()));
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.ObjectParameter(null);
		}

		[Fact]
		public void ObjectParameterValueMatches() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, (_, parameters) => Assert.Same(Person, parameters.First()));
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.ObjectParameter(Person);
		}

		[Fact]
		public void ParameterCountMatches() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, (method, parameters) => Assert.Equal(method.GetParameters().Length, parameters.Length));
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.Void();
			instance.ObjectParameter(null);
			instance.ValueTypeParameter(42);
			instance.MixedTypeParameters(42, null);
		}

		[Fact]
		public void ParametersParameterNotNull() {
			CallbackHandler.MethodInvoker = MethodInvoker(true, (_, parameters) => Assert.NotNull(parameters));

			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.Void();
			instance.ObjectParameter(null);
			instance.ValueTypeParameter(42);
			instance.MixedTypeParameters(42, null);
		}

		[Fact]
		public void ParameterTypesMatch() {
			CallbackHandler.MethodInvoker = MethodInvoker(
				true,
				(method, parameters) => {
					Type[] parameterTypes = method.GetParameters().Select(parameter => parameter.ParameterType).ToArray();

					for (byte i = 0; i < parameterTypes.Length; ++i) {
						Type type = parameterTypes[i];
						object? value = parameters[i];

						if (value == null) {
							continue;
						}

						Type actualType = value.GetType();
						bool assignable = actualType.IsAssignableTo(type);
						Assert.True(assignable);
					}
				}
			);

			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.Void();
			instance.ObjectParameter(null);
			instance.ObjectParameter(Person);
			instance.ValueTypeParameter(42);
			instance.MixedTypeParameters(42, null);
			instance.MixedTypeParameters(42, Person);
		}

		[Fact]
		public void ReturnsFalse() {
			CallbackHandler.MethodInvoker = MethodInvoker(false);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			Assert.Throws<NotImplementedException>(instance.Void);
		}

		[Fact]
		public void ReturnsTrue() {
			CallbackHandler.MethodInvoker = MethodInvoker(true);
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.Void();
		}

		[Fact]
		public void ValueTypeParameterValueMatches() {
			const int value = 42;
			CallbackHandler.MethodInvoker = MethodInvoker(true, (_, parameters) => Assert.Equal(value, parameters.First()));
			IMethodInterface instance = CreateInstance<IMethodInterface>(CallbackHandler);

			instance.ValueTypeParameter(value);
		}
	}
}
