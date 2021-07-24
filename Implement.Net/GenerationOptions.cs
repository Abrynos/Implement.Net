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
using Implement.Net.Reflection;
using JetBrains.Annotations;

namespace Implement.Net {
	/// <summary>
	///     Used to configure the generation of a new class.
	/// </summary>
	/// <seealso cref="TypeFactory"/>
	public sealed class GenerationOptions {
		/// <summary>
		///     The default value used for the <see cref="EnforceDisposable"/> property.
		/// </summary>
		/// <value><c>false</c></value>
		[PublicAPI]
		public const bool DefaultEnforceDisposable = false;

		/// <summary>
		///     The default value used for the <see cref="IgnoreCache"/> property.
		/// </summary>
		/// <value><c>false</c></value>
		[PublicAPI]
		public const bool DefaultIgnoreCache = false;

		/// <summary>
		///     The default value used for the <see cref="ObjectMethodBehaviour"/> property.
		/// </summary>
		/// <value><see cref="EObjectMethodBehaviour.Ignore"/></value>
		/// <seealso cref="EObjectMethodBehaviour"/>
		[PublicAPI]
		public const EObjectMethodBehaviour DefaultObjectMethodBehaviour = EObjectMethodBehaviour.Ignore;

		/// <summary>
		///     The default value used for the <see cref="SealTypeAndMethods"/> property.
		/// </summary>
		/// <value><c>true</c></value>
		[PublicAPI]
		public const bool DefaultSealTypeAndMethods = true;

		/// <summary>
		///     The default value used for the<see cref="HandlerType"/>property.
		/// </summary>
		/// <value><see cref="IDynamicHandler"/></value>
		[PublicAPI]
		public static readonly Type DefaultHandlerType = Types.IDynamicHandler;

		/// <summary>
		///     This property decides whether the generated class will always implement <see cref="IDisposable"/>.
		/// </summary>
		/// <value><see cref="DefaultEnforceDisposable"/></value>
		/// <seealso cref="HandlerType"/>
		[PublicAPI]
		public bool EnforceDisposable { get; set; } = DefaultEnforceDisposable;

		/// <summary>
		///     This property decides which type is used as the <see cref="System.Reflection.ParameterInfo.ParameterType"/> for generated constructors.
		/// </summary>
		/// <remarks>
		///     The configured type inheriting from <see cref="IDisposable"/> has a similar effect to setting <see cref="EnforceDisposable"/> to <c>true</c>.
		/// </remarks>
		/// <value><see cref="DefaultHandlerType"/></value>
		/// <exception cref="ArgumentNullException">value is null</exception>
		/// <exception cref="ArgumentException">set value is not assignable to <see cref="IDynamicHandler"/></exception>
		/// <seealso cref="EnforceDisposable"/>
		[PublicAPI]
		public Type HandlerType {
			get => _HandlerType;
			set {
				if (value == null) {
					throw new ArgumentNullException(nameof(value));
				}

				if (!value.IsAssignableTo(DefaultHandlerType)) {
					throw new ArgumentException($"!{nameof(value)}.{nameof(value.IsAssignableTo)}(typeof({DefaultHandlerType.Name}))");
				}

				_HandlerType = value;
			}
		}

		/// <summary>
		///     This property decides whether the internal cache of the <see cref="TypeFactory"/> is to be ignored during generation.
		/// </summary>
		/// <value><see cref="DefaultIgnoreCache"/></value>
		[PublicAPI]
		public bool IgnoreCache { get; set; } = DefaultIgnoreCache;

		/// <summary>
		///     <para>
		///         This property decides what behaviour the generated class will have regarding methods defined by<see cref="Object"/>.
		///     </para>
		///     <para>
		///         This includes, but in the future may not be limited to:
		///         <list type="bullet">
		///             <item><see cref="Object.ToString()"/></item>
		///             <item><see cref="Object.GetHashCode()"/></item>
		///             <item><see cref="Object.Equals(Object)"/></item>
		///             <item><see cref="Object.Finalize()"/></item>
		///         </list>
		///     </para>
		/// </summary>
		/// <value><see cref="DefaultObjectMethodBehaviour"/></value>
		/// <exception cref="ArgumentException">set value is not defined in <see cref="EObjectMethodBehaviour"/></exception>
		/// <seealso cref="EObjectMethodBehaviour"/>
		[PublicAPI]
		public EObjectMethodBehaviour ObjectMethodBehaviour {
			get => _ObjectMethodBehaviour;
			set {
				if (!Enum.IsDefined(typeof(EObjectMethodBehaviour), value)) {
					throw new ArgumentException($"!{nameof(Enum)}.{nameof(Enum.IsDefined)}(typeof({nameof(EObjectMethodBehaviour)}), {nameof(value)})");
				}

				_ObjectMethodBehaviour = value;
			}
		}

		/// <summary>
		///     <para>
		///         This property decides whether the generated type and the contained methods should be marked as sealed.
		///     </para>
		///     <para>
		///         This includes the get/set methods of properties as well as the add/remove methods of events.
		///     </para>
		/// </summary>
		/// <value><see cref="DefaultSealTypeAndMethods"/></value>
		[PublicAPI]
		public bool SealTypeAndMethods { get; set; } = DefaultSealTypeAndMethods;

		private Type _HandlerType = DefaultHandlerType;

		private EObjectMethodBehaviour _ObjectMethodBehaviour = DefaultObjectMethodBehaviour;

		internal ImmutableGenerationOptions ToImmutableOptions() => ImmutableGenerationOptions.FromOptions(this);

		/// <summary>
		///     Specifies the types of behaviour that is available for methods defined by <see cref="Object"/> in generated types.
		/// </summary>
		/// <seealso cref="Bind"/>
		/// <seealso cref="Forward"/>
		/// <seealso cref="Ignore"/>
		[PublicAPI]
		public enum EObjectMethodBehaviour : byte {
			/// <summary>
			///     Binds methods defined by <see cref="Object"/> to <see cref="IDynamicHandler.TryInvokeMethod(System.Reflection.MethodInfo,object?[],out object?)"/>.
			/// </summary>
			/// <seealso cref="EObjectMethodBehaviour"/>
			Bind,

			/// <summary>
			///     Calls the methods defined on <see cref="Object"/> on the contained instance of <see cref="IDynamicHandler"/> and returns the result.
			/// </summary>
			/// <seealso cref="EObjectMethodBehaviour"/>
			Forward,

			/// <summary>
			///     Ignores methods defined by <see cref="Object"/> completely.
			/// </summary>
			/// <seealso cref="EObjectMethodBehaviour"/>
			Ignore
		}
	}
}
