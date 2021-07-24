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

namespace Implement.Net.Tests.Utilities.Handlers {
	public class CallbackHandler : IDynamicHandler {
		internal EventDelegate EventAdder = (_, _, _) => true;

		internal EventDelegate EventRemover = (_, _, _) => true;

		internal MethodInvokeDelegate MethodInvoker = DefaultMethodInvoker;

		internal PropertyGetDelegate PropertyGetter = DefaultPropertyGetter;

		internal PropertySetDelegate PropertySetter = (_, _, _) => true;

		public bool TryAddEventHandler(Type declaringType, string eventName, object? value) => EventAdder(declaringType, eventName, value);

		public bool TryGetProperty(Type declaringType, string propertyName, out object? result) => PropertyGetter(declaringType, propertyName, out result);

		public bool TryInvokeMethod(MethodInfo method, object?[] arguments, out object? result) => MethodInvoker(method, arguments, out result);

		public bool TryRemoveEventHandler(Type declaringType, string eventName, object? value) => EventRemover(declaringType, eventName, value);

		public bool TrySetProperty(Type declaringType, string propertyName, object? value) => PropertySetter(declaringType, propertyName, value);

		private static bool DefaultMethodInvoker(MethodInfo m, object?[] a, out object? r) {
			r = null;

			return true;
		}

		private static bool DefaultPropertyGetter(Type t, string n, out object? r) {
			r = null;

			return true;
		}
#pragma warning disable CA1711
		public delegate bool EventDelegate(Type t, string n, object? v);

		public delegate bool MethodInvokeDelegate(MethodInfo m, object?[] a, out object? r);

		public delegate bool PropertyGetDelegate(Type t, string n, out object? r);

		public delegate bool PropertySetDelegate(Type t, string n, object? v);
#pragma warning restore CA1711
	}
}
