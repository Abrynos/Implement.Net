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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Implement.Net.Extensions {
	internal static class ReflectionExtensions {
		internal static bool CanAdd(this EventInfo @event) {
			MethodInfo? addMethod = @event.GetAddMethod(true);

			return addMethod != null;
		}

		internal static bool CanRemove(this EventInfo @event) {
			MethodInfo? removeMethod = @event.GetRemoveMethod(true);

			return removeMethod != null;
		}

		internal static IEnumerable<EventInfo> GetEventsRecursive(this Type type, BindingFlags bindingAttr) => GetMembersRecursive(
			type,
			bindingAttr,
			(t, a) => t.GetEvents(a)
		);

		internal static MethodInfo GetMethodInfo(this Expression expression) => expression switch {
			null => throw new ArgumentNullException(nameof(expression)),
			MethodCallExpression mce => mce.Method,
			LambdaExpression le => le.Body.GetMethodInfo(),
			UnaryExpression ue => ue.Operand.GetMethodInfo(),
			_ => throw new ArgumentOutOfRangeException(nameof(expression))
		};

		internal static IEnumerable<MethodInfo> GetMethodsRecursive(this Type type, BindingFlags bindingAttr) => GetMembersRecursive(
			type,
			bindingAttr,
			(t, a) => t.GetMethods(a)
		);

		internal static IEnumerable<PropertyInfo> GetPropertiesRecursive(this Type type, BindingFlags bindingAttr) => GetMembersRecursive(
			type,
			bindingAttr,
			(t, a) => t.GetProperties(a)
		);

		private static IEnumerable<TMember> GetMembersRecursive<TMember>(Type type, BindingFlags bindingAttr, Func<Type, BindingFlags, TMember[]> getMembers) where TMember : MemberInfo {
			List<TMember> result = new ();
			List<Type> interfaces = new ();
			Type? current = type;

			while (current != null) {
				result.AddRange(getMembers(current, bindingAttr).Where(currentMember => result.All(resultMember => resultMember != currentMember)));

				interfaces.AddRange(current.GetInterfaces());
				current = type.BaseType;
			}

			foreach (Type iface in interfaces) {
				result.AddRange(getMembers(iface, bindingAttr).Where(currentMember => result.All(resultMember => resultMember != currentMember)));
			}

			return result;
		}
	}
}
