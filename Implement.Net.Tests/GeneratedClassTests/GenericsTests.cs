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

using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	public sealed class GenericsTests : AbstractGeneratedInstanceTest {
		[Fact]
		public void DerivedMethodCall() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IDeriveGenericInterface>(CallbackHandler);

			_ = instance.Method(new object());
			Assert.True(called);
		}

		[Fact]
		public void DerivedPropertyGet() {
			bool called = false;
			CallbackHandler.PropertyGetter = PropertyGetter(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IDeriveGenericInterface>(CallbackHandler);

			_ = instance.Property;
			Assert.True(called);
		}

		[Fact]
		public void DerivedPropertySet() {
			bool called = false;
			CallbackHandler.PropertySetter = PropertySetter(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IDeriveGenericInterface>(CallbackHandler);

			instance.Property = new object();
			Assert.True(called);
		}

		[Fact]
		public void EventAdd() {
			bool called = false;
			CallbackHandler.EventAdder = EventModifier(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IGenericInterface<object>>(CallbackHandler);

			instance.Event += null;
			Assert.True(called);
		}

		[Fact]
		public void EventRemove() {
			bool called = false;
			CallbackHandler.EventRemover = EventModifier(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IGenericInterface<object>>(CallbackHandler);

			instance.Event -= null;
			Assert.True(called);
		}

		[Fact]
		public void MethodCall() {
			bool called = false;
			CallbackHandler.MethodInvoker = MethodInvoker(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IGenericInterface<object>>(CallbackHandler);

			_ = instance.Method(new object());
			Assert.True(called);
		}

		[Fact]
		public void PropertyGet() {
			bool called = false;
			CallbackHandler.PropertyGetter = PropertyGetter(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IGenericInterface<object>>(CallbackHandler);

			_ = instance.Property;
			Assert.True(called);
		}

		[Fact]
		public void PropertySet() {
			bool called = false;
			CallbackHandler.PropertySetter = PropertySetter(true, () => called = true);
			IGenericInterface<object> instance = CreateInstance<IGenericInterface<object>>(CallbackHandler);

			instance.Property = new object();
			Assert.True(called);
		}
	}
}
