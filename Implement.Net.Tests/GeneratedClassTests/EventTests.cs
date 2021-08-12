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
	public class EventTests : AbstractGeneratedInstanceTest {
		[Fact]
		public void AddActuallyCalled() {
			// this test passing is a precondition for all other tests because those have all their asserts inside the handler, which, in case the handler is never called, are never executed
			bool called = false;
			CallbackHandler.EventAdder = EventModifier(true, () => called = true);
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent += null;
			Assert.True(called);
		}

		[Fact]
		public void AddException() {
			CallbackHandler.EventAdder = EventModifier(true, () => throw new TestException());
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			Assert.Throws<TestException>(() => instance.SomeEvent += null);
		}

		[Fact]
		public void AddNameParameterMatches() {
			CallbackHandler.EventAdder = EventModifier(true, (_, name, _) => Assert.Equal(nameof(IEventInterface.SomeEvent), name));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent += null;
		}

		[Fact]
		public void AddNull() {
			CallbackHandler.EventAdder = EventModifier(true, (_, _, value) => Assert.Null(value));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent += null;
		}

		[Fact]
		public void AddReturnsFalse() {
			CallbackHandler.EventAdder = EventModifier(false);
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			Assert.Throws<NotImplementedException>(() => instance.SomeEvent += null);
		}

		[Fact]
		public void AddTypeParameterMatches() {
			CallbackHandler.EventAdder = EventModifier(true, (type, _, _) => Assert.Equal(typeof(IEventInterface), type));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent += null;
		}

		[Fact]
		public void AddValue() {
			Action inValue = IEventInterface.ExampleHandler;
			CallbackHandler.EventAdder = EventModifier(true, (_, _, value) => Assert.Equal(inValue, value));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent += inValue;
		}

		[Fact]
		public void RemoveActuallyCalled() {
			// this test passing is a precondition for all other tests because those have all their asserts inside the handler, which, in case the handler is never called, are never executed
			bool called = false;
			CallbackHandler.EventRemover = EventModifier(true, () => called = true);
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent -= null;
			Assert.True(called);
		}

		[Fact]
		public void RemoveException() {
			CallbackHandler.EventRemover = EventModifier(true, () => throw new TestException());
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			Assert.Throws<TestException>(() => instance.SomeEvent -= null);
		}

		[Fact]
		public void RemoveNameParameterMatches() {
			CallbackHandler.EventRemover = EventModifier(true, (_, name, _) => Assert.Equal(nameof(IEventInterface.SomeEvent), name));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent -= null;
		}

		[Fact]
		public void RemoveNull() {
			CallbackHandler.EventRemover = EventModifier(true, (_, _, value) => Assert.Null(value));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent -= null;
		}

		[Fact]
		public void RemoveReturnsFalse() {
			CallbackHandler.EventRemover = EventModifier(false);
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			Assert.Throws<NotImplementedException>(() => instance.SomeEvent -= null);
		}

		[Fact]
		public void RemoveTypeParameterMatches() {
			CallbackHandler.EventRemover = EventModifier(true, (type, _, _) => Assert.Equal(typeof(IEventInterface), type));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent -= null;
		}

		[Fact]
		public void RemoveValue() {
			Action inValue = IEventInterface.ExampleHandler;
			CallbackHandler.EventRemover = EventModifier(true, (_, _, value) => Assert.Equal(inValue, value));
			IEventInterface instance = CreateInstance<IEventInterface>(CallbackHandler);

			instance.SomeEvent -= inValue;
		}
	}
}
