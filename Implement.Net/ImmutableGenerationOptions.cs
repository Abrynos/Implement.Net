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

namespace Implement.Net {
	internal sealed class ImmutableGenerationOptions {
		internal readonly bool EnforceDisposable;

		internal readonly Type HandlerType;

		internal readonly bool IgnoreCache;

		internal readonly GenerationOptions.EObjectMethodBehaviour ObjectMethodBehaviour;

		internal readonly bool SealTypeAndMethods;

		private ImmutableGenerationOptions(GenerationOptions options) {
			EnforceDisposable = options.EnforceDisposable;
			HandlerType = options.HandlerType;
			IgnoreCache = options.IgnoreCache;
			ObjectMethodBehaviour = options.ObjectMethodBehaviour;
			SealTypeAndMethods = options.SealTypeAndMethods;
		}

		public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ImmutableGenerationOptions other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(EnforceDisposable, HandlerType, ObjectMethodBehaviour);

		internal static ImmutableGenerationOptions FromOptions(GenerationOptions options) => new (options);

		private bool Equals(ImmutableGenerationOptions other) => EnforceDisposable == other.EnforceDisposable
																 && HandlerType == other.HandlerType
																 && ObjectMethodBehaviour == other.ObjectMethodBehaviour
																 && SealTypeAndMethods == other.SealTypeAndMethods;
	}
}
