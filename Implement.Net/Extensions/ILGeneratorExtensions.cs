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
using System.Reflection.Emit;
using Implement.Net.Reflection;

namespace Implement.Net.Extensions {
	// ReSharper disable once InconsistentNaming
	internal static class ILGeneratorExtensions {
		internal static void EmitLoadType(this ILGenerator generator, Type type) {
			generator.Emit(OpCodes.Ldtoken, type);
			generator.EmitCall(OpCodes.Call, Methods.Type.GetTypeFromHandle, null);
		}

		internal static void EmitThrowIfLocalIsFalse(this ILGenerator generator, LocalBuilder variable, Type exceptionType) {
			if (variable.LocalType != typeof(bool)) {
				throw new ArgumentException($"{nameof(variable)}{nameof(variable.LocalType)} != typeof({nameof(Boolean)})");
			}

			if (!exceptionType.IsAssignableTo(typeof(Exception))) {
				throw new ArgumentException($"!typeof({nameof(Exception)}).IsAssignableFrom({nameof(exceptionType)}");
			}

			Label successLabel = generator.DefineLabel();

			// if([variable] == 0) {
			generator.Emit(OpCodes.Ldloc, variable);
			generator.Emit(OpCodes.Ldc_I4_0);
			generator.Emit(OpCodes.Ceq);
			generator.Emit(OpCodes.Brfalse, successLabel);

			// throw new [exceptionType]
			generator.ThrowException(exceptionType);

			// }
			generator.MarkLabel(successLabel);
		}
	}
}
