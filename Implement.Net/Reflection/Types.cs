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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Implement.Net.Reflection {
	internal static class Types {
		internal static readonly Type Boolean = typeof(bool);

		// ReSharper disable once InconsistentNaming
		internal static readonly Type IAsyncDisposable = typeof(IAsyncDisposable);

		// ReSharper disable once InconsistentNaming
		internal static readonly Type IDisposable = typeof(IDisposable);

		// ReSharper disable once InconsistentNaming
		internal static readonly Type IDynamicHandler = typeof(IDynamicHandler);

		internal static readonly Type InvalidCastException = typeof(InvalidCastException);

		internal static readonly Type NotImplementedException = typeof(NotImplementedException);

		internal static readonly Type Object = typeof(object);

		internal static readonly Type ObjectList = typeof(List<object?>);

		internal static readonly Type TaskAwaiter = typeof(TaskAwaiter);

		internal static readonly Type ValueTask = typeof(ValueTask);

		internal static readonly Type Void = typeof(void);
	}
}
