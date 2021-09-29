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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Implement.Net.Extensions;

namespace Implement.Net.Reflection {
	internal static class Methods {
		internal static class Activator {
			internal static readonly MethodInfo CreateInstance = ((Expression<Action<object>>) (obj => System.Activator.CreateInstance(typeof(object)))).GetMethodInfo();
		}

		internal static class GC {
			internal static readonly MethodInfo SuppressFinalize = ((Expression<Action>) (() => System.GC.SuppressFinalize(new object()))).GetMethodInfo();
		}

		// ReSharper disable once InconsistentNaming
		internal static class IAsyncDisposable {
			internal static readonly MethodInfo DisposeAsync = ((Expression<Func<System.IAsyncDisposable, System.Threading.Tasks.ValueTask>>) (asyncDisposable => asyncDisposable.DisposeAsync())).GetMethodInfo();
		}

		// ReSharper disable once InconsistentNaming
		internal static class IDisposable {
			internal static readonly MethodInfo Dispose = ((Expression<Action<System.IDisposable>>) (disposable => disposable.Dispose())).GetMethodInfo();
		}

		// ReSharper disable once InconsistentNaming
		internal static class IDynamicHandler {
			internal static readonly MethodInfo TryAddEventHandler = ((Expression<Func<Net.IDynamicHandler, bool>>) (handler => handler.TryAddEventHandler(typeof(object), "", new object()))).GetMethodInfo();

			internal static readonly MethodInfo TryGetProperty = ((Expression<Func<Net.IDynamicHandler, bool>>) (handler => handler.TryGetProperty(typeof(object), "", out DummyResult))).GetMethodInfo();

			internal static readonly MethodInfo TryInvokeMethod = ((Expression<Func<Net.IDynamicHandler, bool>>) (handler => handler.TryInvokeMethod(TryGetProperty, Array.Empty<object?>(), out DummyResult))).GetMethodInfo();

			internal static readonly MethodInfo TryRemoveEventHandler = ((Expression<Func<Net.IDynamicHandler, bool>>) (handler => handler.TryRemoveEventHandler(typeof(object), "", new object()))).GetMethodInfo();

			internal static readonly MethodInfo TrySetProperty = ((Expression<Func<Net.IDynamicHandler, bool>>) (handler => handler.TrySetProperty(typeof(object), "", new object()))).GetMethodInfo();

			// We need a dummy result because an expression tree may not contain discards
#pragma warning disable IDE0052
			private static object? DummyResult;
#pragma warning restore IDE0052
		}

		internal static class MethodBase {
			internal static readonly MethodInfo GetMethodFromHandle = ((Expression<Func<RuntimeMethodHandle, System.Reflection.MethodBase?>>) (rth => System.Reflection.MethodBase.GetMethodFromHandle(rth))).GetMethodInfo();

			internal static readonly MethodInfo GetMethodFromHandleWithDeclaringType = ((Expression<Func<RuntimeMethodHandle, RuntimeTypeHandle, System.Reflection.MethodBase?>>) ((rmh, rth) => System.Reflection.MethodBase.GetMethodFromHandle(rmh, rth))).GetMethodInfo();
		}

		internal static class Object {
			internal static new readonly MethodInfo GetType = ((Expression<Func<object, System.Type>>) (obj => obj.GetType())).GetMethodInfo();
		}

		internal static class ObjectList {
			internal static readonly MethodInfo Add = ((Expression<Action<List<object?>>>) (ol => ol.Add(null))).GetMethodInfo();

			internal static readonly MethodInfo ToArray = ((Expression<Action<List<object?>>>) (l => l.ToArray())).GetMethodInfo();
		}

		internal static class Task {
			internal static readonly MethodInfo GetAwaiter = ((Expression<Func<System.Threading.Tasks.Task, System.Runtime.CompilerServices.TaskAwaiter>>) (valueTask => valueTask.GetAwaiter())).GetMethodInfo();
		}

		internal static class TaskAwaiter {
			internal static readonly MethodInfo GetResult = ((Expression<Action<System.Runtime.CompilerServices.TaskAwaiter>>) (taskAwaiter => taskAwaiter.GetResult())).GetMethodInfo();
		}

		internal static class Type {
			internal static readonly MethodInfo GetTypeFromHandle = ((Expression<Func<RuntimeTypeHandle, System.Type>>) (rth => System.Type.GetTypeFromHandle(rth))).GetMethodInfo();

			internal static readonly MethodInfo IsAssignableTo = ((Expression<Action<System.Type>>) (t => t.IsAssignableTo(typeof(object)))).GetMethodInfo();
		}

		internal static class ValueTask {
			internal static readonly MethodInfo AsTask = ((Expression<Func<System.Threading.Tasks.ValueTask, System.Threading.Tasks.Task>>) (valueTask => valueTask.AsTask())).GetMethodInfo();

			internal static readonly MethodInfo GetCompletedTask = Properties.ValueTask.CompletedTask.GetGetMethod(true) ?? throw new InvalidOperationException();
		}
	}
}
