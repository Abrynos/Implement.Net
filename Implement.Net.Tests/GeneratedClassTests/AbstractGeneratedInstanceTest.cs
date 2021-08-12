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
using Implement.Net.Tests.Utilities.Handlers;

namespace Implement.Net.Tests.GeneratedClassTests {
	public abstract class AbstractGeneratedInstanceTest : AbstractTest {
		protected CallbackHandler CallbackHandler { get; } = new ();

		protected DefaultHandler DefaultHandler { get; } = new ();

		protected static TInterface CreateInstance<TInterface>(Type type, IDynamicHandler? handler = null) where TInterface : class => (Activator.CreateInstance(type, handler ?? new DefaultHandler()) as TInterface)!;

		protected TInterface CreateInstance<TInterface>(IDynamicHandler? handler = null) where TInterface : class => (Activator.CreateInstance(CreateType<TInterface>(), handler ?? new DefaultHandler()) as TInterface)!;

		protected static CallbackHandler.EventDelegate EventModifier(bool success, Action additionalAction) => EventModifier(success, (_, _, _) => additionalAction.Invoke());

		protected static CallbackHandler.EventDelegate EventModifier(bool success, Action<Type, string, object?>? additionalAction = null) => (t, n, v) => {
			additionalAction?.Invoke(t, n, v);

			return success;
		};

		protected static CallbackHandler.MethodInvokeDelegate MethodInvoker(bool success, Action additionalAction, object? outValue = null) => MethodInvoker(success, (_, _) => additionalAction.Invoke(), outValue);

		protected static CallbackHandler.MethodInvokeDelegate MethodInvoker(bool success, object? outValue) => MethodInvoker(success, () => { }, outValue);

		protected static CallbackHandler.MethodInvokeDelegate MethodInvoker(bool success, Action<MethodInfo, object?[]>? additionalAction = null, object? outValue = null) => (MethodInfo m, object?[] p, out object? r) => {
			additionalAction?.Invoke(m, p);
			r = outValue;

			return success;
		};

		protected static CallbackHandler.PropertyGetDelegate PropertyGetter(bool success, object? outValue) => PropertyGetter(success, () => { }, outValue);

		protected static CallbackHandler.PropertyGetDelegate PropertyGetter(bool success, Action additionalAction, object? outValue = null) => PropertyGetter(success, (_, _) => additionalAction.Invoke(), outValue);

		protected static CallbackHandler.PropertyGetDelegate PropertyGetter(bool success, Action<Type, string>? additionalAction = null, object? outValue = null) => (Type t, string n, out object? r) => {
			additionalAction?.Invoke(t, n);
			r = outValue;

			return success;
		};

		protected static CallbackHandler.PropertySetDelegate PropertySetter(bool success, Action additionalAction) => PropertySetter(success, (_, _, _) => additionalAction.Invoke());

		protected static CallbackHandler.PropertySetDelegate PropertySetter(bool success, Action<Type, string, object?>? additionalAction = null) => (t, n, v) => {
			additionalAction?.Invoke(t, n, v);

			return success;
		};
	}
}
