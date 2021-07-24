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
using Implement.Net.Tests.Utilities.Handlers;
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests {
	public class TypeFactoryTests : AbstractTest {
		[Fact]
		public void ConstructedGenericInterface0() => TypeFactory.CreateType(typeof(IGenericInterface<object>));

		[Fact]
		public void ConstructedGenericInterface1() => TypeFactory.CreateType(typeof(IDeriveGenericInterface));

		// We cannot trust everyone to have nullable types enabled - Make sure we don't run into any problems when receiving null values
#pragma warning disable 8625
		[Fact]
		public void CreateNullType() => Assert.Throws<ArgumentNullException>(() => TypeFactory.CreateType(null));
#pragma warning restore 8625

		[Fact]
		public void DefinesGenericParameters() {
			IDeriveGenericInterface instance = (Activator.CreateInstance(CreateType<IDeriveGenericInterface>(), new DefaultHandler()) as IDeriveGenericInterface)!;
			Assert.NotNull(instance);
		}

		[Fact]
		public void GenericInterface() => Assert.Throws<ArgumentException>(() => TypeFactory.CreateType(typeof(IGenericInterface<>)));

		[Fact]
		public void GenericMethodInInterface() => Assert.Throws<ArgumentException>(() => CreateType<IGenericMethod>());

		[Fact]
		public void IgnoreCache() {
			Options.IgnoreCache = true;

			Type typeA = TypeFactory.CreateType<IEmptyInterface>(Options);
			Type typeB = TypeFactory.CreateType<IEmptyInterface>(Options);
			Assert.NotSame(typeA, typeB);
		}

		[Fact]
		public void ImplementIDisposeDirectly() => Assert.Throws<ArgumentException>(() => CreateType<IDisposable>());

		[Fact]
		public void ImplementNonInterfaceType() => Assert.Throws<ArgumentException>(() => TypeFactory.CreateType<AbstractTest>());

		[Fact]
		public void UseCache() {
			Options.IgnoreCache = false;

			Type typeA = TypeFactory.CreateType<IEmptyInterface>(Options);
			Type typeB = TypeFactory.CreateType<IEmptyInterface>(Options);
			Assert.Same(typeA, typeB);
		}
	}
}
