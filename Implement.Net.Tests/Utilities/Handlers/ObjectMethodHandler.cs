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

namespace Implement.Net.Tests.Utilities.Handlers {
	internal sealed class ObjectMethodHandler : DefaultHandler {
		internal bool EqualsCalled { get; private set; }
		internal bool EqualsResult { get; set; }

		internal bool GetHashCodeCalled { get; private set; }
		internal int GetHashCodeResult { get; set; } = 42;
		internal object? LastEqualsParameter { get; private set; }

		internal bool ToStringCalled { get; private set; }
		internal string? ToStringResult { get; set; } = string.Empty;

		private readonly Action FinalizeAction;

		internal ObjectMethodHandler() : this(() => { }) { }

		internal ObjectMethodHandler(Action finalizeAction) => FinalizeAction = finalizeAction;

		~ObjectMethodHandler() => FinalizeAction.Invoke();

		public override bool Equals(object? obj) {
			EqualsCalled = true;
			LastEqualsParameter = obj;

			return EqualsResult;
		}

		public override int GetHashCode() {
			// ReSharper disable once NonReadonlyMemberInGetHashCode
			GetHashCodeCalled = true;

			// ReSharper disable once NonReadonlyMemberInGetHashCode
			return GetHashCodeResult;
		}

		public override string? ToString() {
			ToStringCalled = true;

			return ToStringResult;
		}
	}
}
