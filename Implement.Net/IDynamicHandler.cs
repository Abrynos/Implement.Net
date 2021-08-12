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

namespace Implement.Net {
	/// <summary>
	///     Provides a mechanism for handling  calls to generated classes.
	/// </summary>
	/// <seealso cref="TypeFactory"/>
	public interface IDynamicHandler {
		/// <summary>
		///     Invoked when adding an event handler to any event in a generated class.
		/// </summary>
		/// <param name="declaringType">The declaring type of the event which is being accessed</param>
		/// <param name="eventName">The name of the event which is being accessed</param>
		/// <param name="value">The event handler the caller tries to add</param>
		/// <returns><c>true</c> if the <paramref name="value"/> was received and handled successfully. <c>false</c> if a <see cref="NotImplementedException"/> should be thrown instead.</returns>
		bool TryAddEventHandler(Type declaringType, string eventName, object? value);

		/// <summary>
		///     Invoked when getting the value of any property in a generated class.
		/// </summary>
		/// <param name="declaringType">The declaring type of the property which is being accessed</param>
		/// <param name="propertyName">The name of the property which is being accessed</param>
		/// <param name="result">The value the caller is expected to receive</param>
		/// <returns><c>true</c> if the <paramref name="result"/> can be returned to the caller. <c>false</c> if a <see cref="NotImplementedException"/> should be thrown instead.</returns>
		bool TryGetProperty(Type declaringType, string propertyName, out object? result);

		/// <summary>
		///     Invoked when calling any method in a generating class.
		/// </summary>
		/// <param name="method"><see cref="MethodInfo"/> providing metadata access to the method that was invoked</param>
		/// <param name="arguments">The parameters of the method call</param>
		/// <param name="result">The value the caller is expected to receive; ignored for <c>void</c> -methods</param>
		/// <returns><c>true</c> if the <paramref name="result"/> can be returned to the caller. <c>false</c> if a <see cref="NotImplementedException"/> should be thrown instead.</returns>
		bool TryInvokeMethod(MethodInfo method, object?[] arguments, out object? result);

		/// <summary>
		///     Invoked when removing an event handler from any event in a generated class.
		/// </summary>
		/// <param name="declaringType">The declaring type of the event which is being accessed</param>
		/// <param name="eventName">The name of the event which is being accessed</param>
		/// <param name="value">The event handler the caller tries to remove</param>
		/// <returns><c>true</c> if the <paramref name="value"/> was received and handled successfully. <c>false</c> if a <see cref="NotImplementedException"/> should be thrown instead.</returns>
		bool TryRemoveEventHandler(Type declaringType, string eventName, object? value);

		/// <summary>
		///     Invoked when setting the value of any property in a generated class.
		/// </summary>
		/// <param name="declaringType">The declaring type of the property which is being accessed</param>
		/// <param name="propertyName">The name of the property which is being accessed</param>
		/// <param name="value">The value the caller tries to set</param>
		/// <returns><c>true</c> if the <paramref name="value"/> was received and handled successfully. <c>false</c> if a <see cref="NotImplementedException"/> should be thrown instead.</returns>
		bool TrySetProperty(Type declaringType, string propertyName, object? value);
	}
}
