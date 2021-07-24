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
using Implement.Net.Tests.Utilities.Interfaces;
using Xunit;

namespace Implement.Net.Tests.GeneratedClassTests {
	public sealed class AttributeTests : AbstractGeneratedInstanceTest {
		[Fact]
		public void EventMethodsAreNotSealed() {
			Options.SealTypeAndMethods = false;
			Type type = CreateType<IEventInterface>(Options);

			EventInfo property = type.GetEvent(nameof(IEventInterface.SomeEvent))!;
			MethodInfo addMethod = property.GetAddMethod(true)!;
			MethodInfo removeMethod = property.GetRemoveMethod(true)!;

			Assert.False(addMethod.IsFinal);
			Assert.False(removeMethod.IsFinal);
		}

		[Fact]
		public void EventMethodsAreSealed() {
			Options.SealTypeAndMethods = true;
			Type type = CreateType<IEventInterface>(Options);

			EventInfo property = type.GetEvent(nameof(IEventInterface.SomeEvent))!;
			MethodInfo addMethod = property.GetAddMethod(true)!;
			MethodInfo removeMethod = property.GetRemoveMethod(true)!;

			Assert.True(addMethod.IsFinal);
			Assert.True(removeMethod.IsFinal);
		}

		[Fact]
		public void IsNotAbstract() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsAbstract);
		}

		[Fact]
		public void IsNotArray() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsArray);
		}

		// ReSharper disable once InconsistentNaming
		[Fact]
		public void IsNotCOMObject() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsCOMObject);
			Assert.False(type.IsImport);
		}

		[Fact]
		public void IsNotEnum() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsEnum);
		}

		[Fact]
		public void IsNotGeneric0() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsGenericType);
		}

		[Fact]
		public void IsNotGeneric1() {
			Type type = CreateType<IGenericInterface<object>>();
			Assert.False(type.IsGenericType);
		}

		[Fact]
		public void IsNotGeneric2() {
			Type type = CreateType<IDeriveGenericInterface>();
			Assert.False(type.IsGenericType);
		}

		[Fact]
		public void IsNotInterface() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsInterface);
		}

		[Fact]
		public void IsNotPointer() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsPointer);
		}

		[Fact]
		public void IsNotPrimitive() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsPrimitive);
		}

		[Fact]
		public void IsNotSealed() {
			Options.SealTypeAndMethods = false;
			Type type = CreateType<IEmptyInterface>(Options);
			Assert.False(type.IsSealed);
		}

		[Fact]
		public void IsNotSpecial() {
			Type type = CreateType<IEmptyInterface>();
			Assert.False(type.IsSpecialName);
		}

		[Fact]
		public void IsSealed() {
			Options.SealTypeAndMethods = true;
			Type type = CreateType<IEmptyInterface>(Options);
			Assert.True(type.IsSealed);
		}

		[Fact]
		public void IsTypeDefinition() {
			Type type = CreateType<IEmptyInterface>();
			Assert.True(type.IsTypeDefinition);
		}

		[Fact]
		public void IsVisible() {
			Type type = CreateType<IEmptyInterface>();
			Assert.True(type.IsVisible);
			Assert.True(type.IsPublic);
			Assert.False(type.IsNotPublic);
		}

		[Fact]
		public void MethodsAreNotSealed() {
			Options.SealTypeAndMethods = false;
			Type type = CreateType<IMethodInterface>(Options);

			MethodInfo method = type.GetMethod(nameof(IMethodInterface.Void))!;

			Assert.False(method.IsFinal);
		}

		[Fact]
		public void MethodsAreSealed() {
			Options.SealTypeAndMethods = true;
			Type type = CreateType<IMethodInterface>(Options);

			MethodInfo method = type.GetMethod(nameof(IMethodInterface.Void))!;

			Assert.True(method.IsFinal);
		}

		[Fact]
		public void PropertyMethodsAreNotSealed() {
			Options.SealTypeAndMethods = false;
			Type type = CreateType<IPropertyInterface>(Options);

			PropertyInfo property = type.GetProperty(nameof(IPropertyInterface.Object))!;
			MethodInfo getMethod = property.GetGetMethod(true)!;
			MethodInfo setMethod = property.GetSetMethod(true)!;

			Assert.False(getMethod.IsFinal);
			Assert.False(setMethod.IsFinal);
		}

		[Fact]
		public void PropertyMethodsAreSealed() {
			Options.SealTypeAndMethods = true;
			Type type = CreateType<IPropertyInterface>(Options);

			PropertyInfo property = type.GetProperty(nameof(IPropertyInterface.Object))!;
			MethodInfo getMethod = property.GetGetMethod(true)!;
			MethodInfo setMethod = property.GetSetMethod(true)!;

			Assert.True(getMethod.IsFinal);
			Assert.True(setMethod.IsFinal);
		}
	}
}
