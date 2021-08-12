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
using System.Linq;
using Xunit;

namespace Implement.Net.Tests {
	public class GenerationOptionsTests {
		[Fact]
		public void SetNonExistingObjectMethodBehaviour() {
			GenerationOptions options = new ();
			GenerationOptions.EObjectMethodBehaviour invalidOption = (GenerationOptions.EObjectMethodBehaviour) (Enum.GetValues<GenerationOptions.EObjectMethodBehaviour>().Select(option => (int) option).Max() + 1);

			Assert.Throws<ArgumentException>(() => options.ObjectMethodBehaviour = invalidOption);
		}

		[Fact]
		public void SetNonHandlerHandlerType() {
			GenerationOptions options = new ();

			Assert.Throws<ArgumentException>(() => options.HandlerType = typeof(object));
		}

		// We cannot trust everyone to have nullable types enabled - Make sure we don't run into any problems when receiving null values
#pragma warning disable 8625
		[Fact]
		public void SetNullHandlerType() {
			GenerationOptions options = new ();

			Assert.Throws<ArgumentNullException>(() => options.HandlerType = null);
		}
#pragma warning restore 8625
	}
}
