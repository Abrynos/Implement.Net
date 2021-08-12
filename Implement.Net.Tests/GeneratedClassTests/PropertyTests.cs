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
using Implement.Net.Tests.Utilities;
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	public sealed class PropertyTests : AbstractGeneratedInstanceTest {
		private readonly Fruit Fruit = new ();

		private readonly Person Person = new ();

		[Fact]
		public void GetException() {
			CallbackHandler.PropertyGetter = PropertyGetter(true, (_, _) => throw new TestException());
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<TestException>(() => _ = instance.Object);
			Assert.Throws<TestException>(() => _ = instance.ValueType);
		}

		[Fact]
		public void GetNameParameterMatches() {
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			CallbackHandler.PropertyGetter = PropertyGetter(true, (_, name) => Assert.Equal(nameof(instance.ValueType), name));
			_ = instance.ValueType;
			CallbackHandler.PropertyGetter = PropertyGetter(true, (_, name) => Assert.Equal(nameof(instance.Object), name));
			_ = instance.Object;
			CallbackHandler.PropertyGetter = PropertyGetter(true, (_, name) => Assert.Equal(nameof(instance.GetOnlyValueType), name));
			_ = instance.GetOnlyValueType;
			CallbackHandler.PropertyGetter = PropertyGetter(true, (_, name) => Assert.Equal(nameof(instance.GetOnlyObject), name));
			_ = instance.GetOnlyObject;
		}

		[Fact]
		public void GetObject() {
			CallbackHandler.PropertyGetter = PropertyGetter(true, Person);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Person? result = instance.Object;
			Assert.Same(Person, result);
		}

		[Fact]
		public void GetObjectSetsNonAssignable() {
			CallbackHandler.PropertyGetter = PropertyGetter(true, Fruit);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(() => _ = instance.Object);
		}

		[Fact]
		public void GetObjectSetsNull() {
			CallbackHandler.PropertyGetter = PropertyGetter(true);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Person? result = instance.Object;
			Assert.Null(result);
		}

		[Fact]
		public void GetObjectSetsValueType() {
			CallbackHandler.PropertyGetter = PropertyGetter(true, 42);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(() => _ = instance.Object);
		}

		[Fact]
		public void GetReturnsFalse() {
			CallbackHandler.PropertyGetter = PropertyGetter(false);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<NotImplementedException>(() => _ = instance.GetOnlyValueType);
			Assert.Throws<NotImplementedException>(() => _ = instance.GetOnlyObject);
		}

		[Fact]
		public void GetterActuallyCalled() {
			// this test passing is a precondition for all other tests because those have all their asserts inside the handler, which, in case the handler is never called, are never executed
			bool called;
			CallbackHandler.PropertyGetter = PropertyGetter(true, (_, _) => called = true);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			called = false;
			_ = instance.GetOnlyObject;
			Assert.True(called);

			called = false;
			_ = instance.GetOnlyValueType;
			Assert.True(called);

			called = false;
			_ = instance.Object;
			Assert.True(called);

			called = false;
			_ = instance.ValueType;
			Assert.True(called);
		}

		[Fact]
		public void GetTypeParameterMatches() {
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

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
		public void GetValueType() {
			const int value = 42;
			CallbackHandler.PropertyGetter = PropertyGetter(true, value);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			int result = instance.ValueType;
			Assert.Equal(value, result);
		}

		[Fact]
		public void GetValueTypeSetsNonAssignable() {
			CallbackHandler.PropertyGetter = PropertyGetter(true, true);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(() => _ = instance.ValueType);
		}

		[Fact]
		public void GetValueTypeSetsNull() {
			CallbackHandler.PropertyGetter = PropertyGetter(true); // null is already the default here
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			int result = instance.ValueType;
			Assert.Equal(default(int), result);
		}

		[Fact]
		public void GetValueTypeSetsObject() {
			CallbackHandler.PropertyGetter = PropertyGetter(true, Person);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<InvalidCastException>(() => _ = instance.ValueType);
		}

		[Fact]
		public void SetException() {
			CallbackHandler.PropertySetter = PropertySetter(true, () => throw new TestException());
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<TestException>(() => instance.Object = null);
		}

		[Fact]
		public void SetNameParameterMatches() {
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			CallbackHandler.PropertySetter = PropertySetter(true, (_, name, _) => Assert.Equal(nameof(instance.ValueType), name));
			instance.ValueType = 42;
			CallbackHandler.PropertySetter = PropertySetter(true, (_, name, _) => Assert.Equal(nameof(instance.Object), name));
			instance.Object = Person;
			CallbackHandler.PropertySetter = PropertySetter(true, (_, name, _) => Assert.Equal(nameof(instance.SetOnlyValueType), name));
			instance.SetOnlyValueType = 42;
			CallbackHandler.PropertySetter = PropertySetter(true, (_, name, _) => Assert.Equal(nameof(instance.SetOnlyObject), name));
			instance.SetOnlyObject = Person;
		}

		[Fact]
		public void SetObject() {
			CallbackHandler.PropertySetter = PropertySetter(true, (_, _, value) => Assert.Same(Person, value));
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			instance.Object = Person;
		}

		[Fact]
		public void SetObjectToNull() {
			CallbackHandler.PropertySetter = PropertySetter(true, (_, _, value) => Assert.Null(value));
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			instance.Object = null;
		}

		[Fact]
		public void SetReturnsFalse() {
			CallbackHandler.PropertySetter = PropertySetter(false);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			Assert.Throws<NotImplementedException>(() => instance.SetOnlyValueType = 42);
			Assert.Throws<NotImplementedException>(() => instance.SetOnlyObject = Person);
		}

		[Fact]
		public void SetterActuallyCalled() {
			// this test passing is a precondition for all other tests because those have all their asserts inside the handler, which, in case the handler is never called, are never executed
			bool called;
			CallbackHandler.PropertySetter = PropertySetter(true, () => called = true);
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			called = false;
			instance.SetOnlyObject = null;
			Assert.True(called);

			called = false;
			instance.SetOnlyValueType = 42;
			Assert.True(called);

			called = false;
			instance.Object = null;
			Assert.True(called);

			called = false;
			instance.ValueType = 42;
			Assert.True(called);
		}

		[Fact]
		public void SetTypeParameterMatches() {
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.ValueType = 42;
			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.Object = Person;
			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.SetOnlyValueType = 42;
			CallbackHandler.PropertySetter = PropertySetter(true, (type, _, _) => Assert.Equal(typeof(IPropertyInterface), type));
			instance.SetOnlyObject = Person;
		}

		[Fact]
		public void SetValueType() {
			const int inValue = 42;
			CallbackHandler.PropertySetter = PropertySetter(true, (_, _, value) => Assert.Equal(inValue, value));
			IPropertyInterface instance = CreateInstance<IPropertyInterface>(CallbackHandler);

			instance.ValueType = inValue;
		}
	}
}
