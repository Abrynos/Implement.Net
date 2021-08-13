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
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Implement.Net.Extensions;
using Implement.Net.Reflection;
using JetBrains.Annotations;

namespace Implement.Net {
	/// <summary>
	///     Contains methods to dynamically implement any interface at runtime and handle any calls in an instance of <see cref="IDynamicHandler"/>. Caches generated types on a per-instance basis.
	/// </summary>
	public sealed class TypeFactory {
		private const BindingFlags RelevantFlagsForBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		internal static readonly string GeneratedTypeNamespace = typeof(TypeFactory).Namespace + ".Generated";

		private readonly ConcurrentDictionary<(Type Interface, ImmutableGenerationOptions Options), Type> Cache = new ();

		private readonly ModuleBuilder ModuleBuilder;

		/// <summary>
		///     Initializes a new instance of the <see cref="TypeFactory"/>.
		/// </summary>
		[PublicAPI]
		public TypeFactory() {
			AssemblyName assemblyName = new ($"{GeneratedTypeNamespace}.{Guid.NewGuid():N}");
			AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
			ModuleBuilder = assemblyBuilder.DefineDynamicModule("Main");
		}

		/// <summary>
		///     Creates a type implementing <typeparamref name="TInterface"/> and respecting the configured <paramref name="options"/>.
		/// </summary>
		/// <param name="options">The options to be applied during generation of the new type.</param>
		/// <typeparam name="TInterface">The interface to be implemented.</typeparam>
		/// <returns>A new type implementing <typeparamref name="TInterface"/>. It contains one constructor taking an instance of the type specified in <see cref="GenerationOptions.HandlerType"/>.</returns>
		/// <exception cref="ArgumentException"><typeparamref name="TInterface"/> is not an interface</exception>
		/// <exception cref="ArgumentException"><typeparamref name="TInterface"/> is not public</exception>
		/// <exception cref="ArgumentException"><typeparamref name="TInterface"/> contains one or more unbound generic methods</exception>
		/// <exception cref="ArgumentException"><typeparamref name="TInterface"/> is equal to <see cref="IDisposable"/></exception>
		/// <exception cref="ArgumentException"><typeparamref name="TInterface"/> is equal to <see cref="IAsyncDisposable"/></exception>
		/// <seealso cref="GenerationOptions"/>
		[PublicAPI]
		public Type CreateType<TInterface>(GenerationOptions? options = null) where TInterface : class => CreateType(typeof(TInterface), options);

		/// <summary>
		///     Creates a type implementing the interface specified in <paramref name="interfaceType"/> and respecting the configured <paramref name="options"/>.
		/// </summary>
		/// <param name="interfaceType">The interface to be implemented.</param>
		/// <param name="options">The options to be applied during generation of the new type.</param>
		/// <returns>A new type implementing <paramref name="interfaceType"/>. It contains one constructor taking an instance of the type specified in <see cref="GenerationOptions.HandlerType"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <c>null</c></exception>
		/// <exception cref="ArgumentException"><paramref name="interfaceType"/> is not an interface</exception>
		/// <exception cref="ArgumentException"><paramref name="interfaceType"/> is not public</exception>
		/// <exception cref="ArgumentException"><paramref name="interfaceType"/> is an unbound generic type</exception>
		/// <exception cref="ArgumentException"><paramref name="interfaceType"/> contains one or more unbound generic methods</exception>
		/// <exception cref="ArgumentException"><paramref name="interfaceType"/> is equal to <see cref="IDisposable"/></exception>
		/// <exception cref="ArgumentException"><paramref name="interfaceType"/> is equal to <see cref="IAsyncDisposable"/></exception>
		/// <seealso cref="GenerationOptions"/>
		[PublicAPI]
		public Type CreateType(Type interfaceType, GenerationOptions? options = null) => CreateTypeInternal(interfaceType, (options ?? new GenerationOptions()).ToImmutableOptions());

		/// <summary>
		///     Attempts to create a type implementing <typeparamref name="TInterface"/>.
		/// </summary>
		/// <param name="result">A new type implementing <typeparamref name="TInterface"/>. It contains one constructor taking an instance of <see cref="IDynamicHandler"/>.</param>
		/// <typeparam name="TInterface">The interface to be implemented.</typeparam>
		/// <returns><c>true</c> if creation was a success; <c>false</c> otherwise;</returns>
		/// <seealso cref="CreateType{TInterface}(GenerationOptions)"/>
		[PublicAPI]
		public bool TryCreateType<TInterface>(out Type? result) where TInterface : class => TryCreateType<TInterface>(new GenerationOptions(), out result);

		/// <summary>
		///     Attempts to create a type implementing <typeparamref name="TInterface"/> and respecting the configured <paramref name="options"/>.
		/// </summary>
		/// <param name="options">The options to be applied during generation of the new type.</param>
		/// <param name="result">A new type implementing <typeparamref name="TInterface"/>. It contains one constructor taking an instance of the type specified in <see cref="GenerationOptions.HandlerType"/>.</param>
		/// <typeparam name="TInterface">The interface to be implemented.</typeparam>
		/// <returns><c>true</c> if creation was a success; <c>false</c> otherwise</returns>
		/// <seealso cref="CreateType{TInterface}(GenerationOptions)"/>
		/// <seealso cref="GenerationOptions"/>
		[PublicAPI]
		public bool TryCreateType<TInterface>(GenerationOptions? options, out Type? result) where TInterface : class => TryCreateType(typeof(TInterface), options, out result);

		/// <summary>
		///     Attempts to create a type implementing the interface specified in <paramref name="interfaceType"/> and respecting the configured <paramref name="options"/>.
		/// </summary>
		/// <param name="interfaceType"> The interface to be implemented. </param>
		/// <param name="options">The options to be applied during generation of the new type.</param>
		/// <param name="result">A new type implementing <paramref name="interfaceType"/>. It contains one constructor taking an instance of the type specified in <see cref="GenerationOptions.HandlerType"/>.</param>
		/// <returns> <c>true</c>if creation was a success; <c>false</c> otherwise</returns>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <c>null</c></exception>
		/// <seealso cref="CreateType(Type,GenerationOptions)"/>
		/// <seealso cref="GenerationOptions"/>
		[PublicAPI]
		public bool TryCreateType(Type interfaceType, GenerationOptions? options, out Type? result) {
			if (interfaceType == null) {
				throw new ArgumentNullException(nameof(interfaceType));
			}

			try {
				result = CreateType(interfaceType, options);

				return true;
			} catch (Exception) {
				result = null;

				return false;
			}
		}

		/// <summary>
		///     Attempts to create a type implementing the interface specified in <paramref name="interfaceType"/>.
		/// </summary>
		/// <param name="interfaceType">The interface to be implemented.</param>
		/// <param name="result">A new type implementing <paramref name="interfaceType"/>. It contains one constructor taking an instance of <see cref="IDynamicHandler"/>.</param>
		/// <returns><c>true</c> if creation was a success; <c>false</c> otherwise</returns>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <c>null</c></exception>
		/// <seealso cref="CreateType(Type,GenerationOptions)"/>
		[PublicAPI]
		public bool TryCreateType(Type interfaceType, out Type? result) => TryCreateType(interfaceType, new GenerationOptions(), out result);

		private Type CreateTypeInternal(Type interfaceType, ImmutableGenerationOptions options) => interfaceType switch {
			null => throw new ArgumentNullException(nameof(interfaceType)),
			{ IsInterface: false } => throw new ArgumentException($"!{nameof(interfaceType)}.{nameof(interfaceType.IsInterface)}"),
			{ IsPublic: false } => throw new ArgumentException($"!{nameof(interfaceType)}.{nameof(interfaceType.IsPublic)}"),
			{ } iface when iface == Types.IDisposable => throw new ArgumentException($"{nameof(interfaceType)} == typeof({nameof(IDisposable)})"),
			{ } iface when iface == Types.IAsyncDisposable => throw new ArgumentException($"{nameof(interfaceType)} == typeof({nameof(IAsyncDisposable)})"),
			{ IsGenericType: true, IsConstructedGenericType: false } => throw new ArgumentException($"{nameof(interfaceType)}.{nameof(interfaceType.IsGenericType)} && !{nameof(interfaceType)}.{nameof(interfaceType.IsConstructedGenericType)}"),
			_ => options.IgnoreCache
				? GenerateNewType(interfaceType, options)
				: Cache.GetOrAdd((interfaceType, options), key => GenerateNewType(key.Interface, key.Options))
		};

		private Type GenerateNewType(Type interfaceType, ImmutableGenerationOptions options) {
			TypeAttributes attributes = TypeAttributes.Public;

			if (options.SealTypeAndMethods) {
				attributes |= TypeAttributes.Sealed;
			}

			TypeBuilder typeBuilder = ModuleBuilder.DefineType($"{GeneratedTypeNamespace}._{Guid.NewGuid():N}", attributes);
			typeBuilder.SetParent(Types.Object);
			typeBuilder.AddInterfaceImplementation(interfaceType);

			FieldBuilder handlerField = typeBuilder.DefineField("_Handler", options.HandlerType, FieldAttributes.Private | FieldAttributes.InitOnly);

			CreateConstructor(typeBuilder, handlerField);

			bool isHandlerDisposable = options.HandlerType.IsAssignableTo(Types.IDisposable);

			if (isHandlerDisposable || interfaceType.IsAssignableTo(Types.IDisposable) || options.EnforceDisposable) {
				typeBuilder.AddInterfaceImplementation(Types.IDisposable);
				ImplementDispose(typeBuilder, handlerField, !isHandlerDisposable);
			}

			if (options.ObjectMethodBehaviour != GenerationOptions.EObjectMethodBehaviour.Ignore) {
				ImplementObjectMethods(typeBuilder, handlerField, options);
			}

			foreach (MethodInfo baseMethod in interfaceType.GetMethodsRecursive(RelevantFlagsForBinding)
														   .Where(method => method.DeclaringType != Types.IDisposable)
														   .Where(method => method.IsVirtual)
														   .Where(method => !method.IsSpecialName)
														   .Where(method => !method.IsFinal)) {
				BindMethod(typeBuilder, baseMethod, handlerField, options.SealTypeAndMethods);
			}

			foreach (PropertyInfo baseProperty in interfaceType.GetPropertiesRecursive(RelevantFlagsForBinding)) {
				BindProperty(typeBuilder, baseProperty, handlerField, options.SealTypeAndMethods);
			}

			foreach (EventInfo baseEvent in interfaceType.GetEventsRecursive(RelevantFlagsForBinding)) {
				BindEvent(typeBuilder, baseEvent, handlerField, options.SealTypeAndMethods);
			}

			return typeBuilder.CreateType()!;
		}

		private static void BindEvent(TypeBuilder typeBuilder, EventInfo baseEvent, FieldInfo handlerField, bool markFinal) {
			EventBuilder eventBuilder = typeBuilder.DefineEvent(baseEvent.Name, baseEvent.Attributes, baseEvent.EventHandlerType!);

			if (baseEvent.CanAdd()) {
				eventBuilder.SetAddOnMethod(BindEventModificationMethod(typeBuilder, baseEvent, baseEvent.GetAddMethod(true)!, handlerField, Methods.IDynamicHandler.TryAddEventHandler, markFinal));
			}

			if (baseEvent.CanRemove()) {
				eventBuilder.SetRemoveOnMethod(BindEventModificationMethod(typeBuilder, baseEvent, baseEvent.GetRemoveMethod(true)!, handlerField, Methods.IDynamicHandler.TryRemoveEventHandler, markFinal));
			}

			// As of now neither the C# nor the VisualBasic compiler generate a raise method according to documentation.
			// If they do at any point in the future we might have to look into this
			// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.getraisemethod
		}

		// ReSharper disable once SuggestBaseTypeForParameter
		private static MethodBuilder BindEventModificationMethod(TypeBuilder typeBuilder, EventInfo baseEvent, MethodInfo baseMethod, FieldInfo handlerField, MethodInfo handlingMethod, bool markFinal) => OverrideMethod(
			typeBuilder,
			baseMethod,
			generator => {
				// bool loc_0;
				LocalBuilder successLocal = generator.DeclareLocal(Types.Boolean);

				// loc_0 = this.[handlerField].[handlingMethod]([baseEvent.DeclaringType], [baseEvent.Name], [value]);
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, handlerField);

				generator.EmitLoadType(baseEvent.DeclaringType!);
				generator.Emit(OpCodes.Ldstr, baseEvent.Name);
				generator.Emit(OpCodes.Ldarg_1);

				generator.EmitCall(OpCodes.Callvirt, handlingMethod, null);
				generator.Emit(OpCodes.Stloc, successLocal);

				// if (!loc_0) throw new NotImplementedException();
				generator.EmitThrowIfLocalIsFalse(successLocal, Types.NotImplementedException);

				// return;
				generator.Emit(OpCodes.Ret);
			},
			markFinal
		);

		private static void BindMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, FieldInfo handlerField, bool markFinal) => OverrideMethod(
			typeBuilder,
			baseMethod,
			generator => {
				// object? loc_0;
				LocalBuilder resultLocal = generator.DeclareLocal(Types.Object);

				// bool loc_1;
				LocalBuilder successLocal = generator.DeclareLocal(Types.Boolean);

				// List<object?> loc_2;
				LocalBuilder parameterLocal = generator.DeclareLocal(Types.ObjectList);

				// loc_2 = new List<object?>();
				generator.Emit(OpCodes.Newobj, Constructors.ObjectList);
				generator.Emit(OpCodes.Stloc, parameterLocal);

				foreach (ParameterInfo parameter in baseMethod.GetParameters().OrderBy(parameter => parameter.Position)) {
					// loc_2.Add([parameter]);
					generator.Emit(OpCodes.Ldloc, parameterLocal);
					generator.Emit(OpCodes.Ldarg, parameter.Position + 1); // ParameterInfo starts indexing with 0; ILCode argument 0 is the object instance on which the method is called

					if (parameter.ParameterType.IsValueType) {
						generator.Emit(OpCodes.Box, parameter.ParameterType);
					}

					generator.EmitCall(OpCodes.Callvirt, Methods.ObjectList.Add, null);
				}

				// loc_1 = this.[handlerField].TryInvokeMethod(MethodInfo.GetMethodFromHandle([baseMethod]), loc_2.ToArray(), out loc_0);
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, handlerField);

				generator.Emit(OpCodes.Ldtoken, baseMethod);

				if (baseMethod.DeclaringType!.IsGenericType) {
					generator.Emit(OpCodes.Ldtoken, baseMethod.ReflectedType!);
					generator.EmitCall(OpCodes.Call, Methods.MethodBase.GetMethodFromHandleWithDeclaringType, null);
				} else {
					generator.EmitCall(OpCodes.Call, Methods.MethodBase.GetMethodFromHandle, null);
				}

				generator.Emit(OpCodes.Ldloc, parameterLocal);
				generator.EmitCall(OpCodes.Callvirt, Methods.ObjectList.ToArray, null);

				generator.Emit(OpCodes.Ldloca_S, resultLocal);

				generator.EmitCall(OpCodes.Callvirt, Methods.IDynamicHandler.TryInvokeMethod, null);

				generator.Emit(OpCodes.Stloc, successLocal);

				// if (!loc_1) throw new NotImplementedException();
				generator.EmitThrowIfLocalIsFalse(successLocal, Types.NotImplementedException);

				if (baseMethod.ReturnType == Types.Void) {
					generator.Emit(OpCodes.Ret);

					return;
				}

				// if (loc_0 != null && !loc_0.GetType().IsAssignableTo([baseMethod.ReturnType])) throw new InvalidCastException();
				EmitThrowIfLocalIsNotNullAndNotAssignable(generator, resultLocal, baseMethod.ReturnType);

				// return loc_0;
				EmitLoadAndUnboxIfNecessary(generator, resultLocal, baseMethod.ReturnType);
				generator.Emit(OpCodes.Ret);
			},
			markFinal
		);

		private static void BindProperty(TypeBuilder typeBuilder, PropertyInfo baseProperty, FieldInfo handlerField, bool markFinal) {
			PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(baseProperty.Name, baseProperty.Attributes & ~PropertyAttributes.HasDefault, baseProperty.PropertyType, null);

			if (baseProperty.CanRead) {
				propertyBuilder.SetGetMethod(BindPropertyGetMethod(typeBuilder, baseProperty, baseProperty.GetGetMethod(true)!, handlerField, markFinal));
			}

			if (baseProperty.CanWrite) {
				propertyBuilder.SetSetMethod(BindPropertySetMethod(typeBuilder, baseProperty, baseProperty.GetSetMethod(true)!, handlerField, markFinal));
			}
		}

		private static MethodBuilder BindPropertyGetMethod(TypeBuilder typeBuilder, PropertyInfo property, MethodInfo baseMethod, FieldInfo handlerField, bool markFinal) => OverrideMethod(
			typeBuilder,
			baseMethod,
			generator => {
				// object? loc_0;
				LocalBuilder resultLocal = generator.DeclareLocal(Types.Object);

				// bool loc_1;
				LocalBuilder successLocal = generator.DeclareLocal(Types.Boolean);

				// loc_1 = this.[handlerField].TryGetProperty([property.DeclaringType], [property.Name], out loc_0);
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, handlerField);

				generator.EmitLoadType(property.DeclaringType!);
				generator.Emit(OpCodes.Ldstr, property.Name);
				generator.Emit(OpCodes.Ldloca_S, resultLocal);

				generator.EmitCall(OpCodes.Callvirt, Methods.IDynamicHandler.TryGetProperty, null);
				generator.Emit(OpCodes.Stloc, successLocal);

				// if (!loc_1) throw new NotImplementedException();
				generator.EmitThrowIfLocalIsFalse(successLocal, Types.NotImplementedException);

				// if (loc_0 != null && !loc_0.GetType().IsAssignableTo([property.PropertyType])) throw new InvalidCastException();
				EmitThrowIfLocalIsNotNullAndNotAssignable(generator, resultLocal, property.PropertyType);

				// return loc_0;
				EmitLoadAndUnboxIfNecessary(generator, resultLocal, property.PropertyType);
				generator.Emit(OpCodes.Ret);
			},
			markFinal
		);

		private static MethodBuilder BindPropertySetMethod(TypeBuilder typeBuilder, PropertyInfo property, MethodInfo baseMethod, FieldInfo handlerField, bool markFinal) => OverrideMethod(
			typeBuilder,
			baseMethod,
			generator => {
				// bool loc_0;
				LocalBuilder successLocal = generator.DeclareLocal(Types.Boolean);

				// loc_0 = this.[handlerField].TrySetProperty([property.DeclaringType], [property.Name], [value]);
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, handlerField);

				generator.EmitLoadType(property.DeclaringType!);
				generator.Emit(OpCodes.Ldstr, property.Name);
				generator.Emit(OpCodes.Ldarg_1);

				if (property.PropertyType.IsValueType) {
					generator.Emit(OpCodes.Box, property.PropertyType);
				}

				generator.EmitCall(OpCodes.Callvirt, Methods.IDynamicHandler.TrySetProperty, null);
				generator.Emit(OpCodes.Stloc, successLocal);

				// if (!loc_0) throw new NotImplementedException();
				generator.EmitThrowIfLocalIsFalse(successLocal, Types.NotImplementedException);

				// return;
				generator.Emit(OpCodes.Ret);
			},
			markFinal
		);

		private static void CreateConstructor(TypeBuilder typeBuilder, FieldInfo field) {
			const string parameterName = "handler";
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { field.FieldType });

			ParameterBuilder fieldParameter = constructorBuilder.DefineParameter(1, ParameterAttributes.In, parameterName);

			ILGenerator generator = constructorBuilder.GetILGenerator();

			Label successLabel = generator.DefineLabel();

			// if ([fieldParameter] == null) {
			generator.Emit(OpCodes.Ldarg, fieldParameter.Position);
			generator.Emit(OpCodes.Ldnull);
			generator.Emit(OpCodes.Ceq);
			generator.Emit(OpCodes.Brfalse_S, successLabel);

			// throw new ArgumentNullException("handler");
			generator.Emit(OpCodes.Ldstr, parameterName);
			generator.Emit(OpCodes.Newobj, Constructors.ArgumentNullException);
			generator.Emit(OpCodes.Throw);

			// }
			generator.MarkLabel(successLabel);

			// this.[handlerField] = [fieldParameter];
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg, fieldParameter.Position);
			generator.Emit(OpCodes.Stfld, field);

			// return;
			generator.Emit(OpCodes.Ret);
		}

		private static void EmitLoadAndUnboxIfNecessary(ILGenerator generator, LocalBuilder local, Type type) {
			generator.Emit(OpCodes.Ldloc, local);

			// if the result is not a value type we are done already
			if (!type.IsValueType) {
				return;
			}

			Label notNull = generator.DefineLabel();

			// if (loc_x == null) {
			generator.Emit(OpCodes.Ldnull);
			generator.Emit(OpCodes.Ceq);
			generator.Emit(OpCodes.Brfalse, notNull);

			// If we get a null reference for a value type we will return the default value for that type
			// loc_x = Activator.CreateInstance([type]);
			generator.EmitLoadType(type);
			generator.Emit(OpCodes.Call, Methods.Activator.CreateInstance);
			generator.Emit(OpCodes.Stloc, local);

			// }
			generator.MarkLabel(notNull);

			// unbox(loc_x);
			generator.Emit(OpCodes.Ldloc, local);
			generator.Emit(OpCodes.Unbox_Any, type);
		}

		private static void EmitThrowIfLocalIsNotNullAndNotAssignable(ILGenerator generator, LocalBuilder local, Type type) {
			Label isNull = generator.DefineLabel();

			// if ([local] != null) {
			generator.Emit(OpCodes.Ldloc, local);
			generator.Emit(OpCodes.Ldnull);
			generator.Emit(OpCodes.Ceq);
			generator.Emit(OpCodes.Brtrue, isNull);

			// if (![local].GetType().IsAssignableTo([type]))
			generator.Emit(OpCodes.Ldloc, local);
			generator.Emit(OpCodes.Callvirt, Methods.Object.GetType);
			generator.EmitLoadType(type);
			generator.Emit(OpCodes.Call, Methods.Type.IsAssignableTo);
			generator.Emit(OpCodes.Brtrue, isNull);

			// throw new InvalidCastException();
			generator.ThrowException(Types.InvalidCastException);

			// }
			generator.MarkLabel(isNull);
		}

		private static void ForwardMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, FieldInfo handlerField, bool markFinal) => OverrideMethod(
			typeBuilder,
			baseMethod,
			generator => {
				// return this.[handlerField].[baseMethod]([parameters]);
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, handlerField);

				foreach (ParameterInfo parameter in baseMethod.GetParameters().OrderBy(parameter => parameter.Position)) {
					generator.Emit(OpCodes.Ldarg, parameter.Position + 1); // ParameterInfo starts indexing with 0; ILCode argument 0 is the object instance on which the method is called
				}

				generator.EmitCall(OpCodes.Callvirt, baseMethod, null);

				generator.Emit(OpCodes.Ret);
			},
			markFinal
		);

		private static void ImplementDispose(TypeBuilder typeBuilder, FieldInfo handlerField, bool checkInstance) => OverrideMethod(
			typeBuilder,
			Methods.IDisposable.Dispose,
			generator => {
				Label doNothingLabel = generator.DefineLabel();

				// if (this.[field] != null) {
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, handlerField);
				generator.Emit(OpCodes.Ldnull);
				generator.Emit(OpCodes.Ceq);
				generator.Emit(OpCodes.Brtrue, doNothingLabel);

				if (checkInstance) {
					// if (this.[field] is IDisposable) {
					generator.Emit(OpCodes.Ldarg_0);
					generator.Emit(OpCodes.Ldfld, handlerField);
					generator.Emit(OpCodes.Isinst, Types.IDisposable);
					generator.Emit(OpCodes.Ldnull);
					generator.Emit(OpCodes.Cgt_Un);
					generator.Emit(OpCodes.Brfalse_S, doNothingLabel);
				}

				// this.[field].Dispose();
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, handlerField);
				generator.EmitCall(OpCodes.Callvirt, Methods.IDisposable.Dispose, null);

				// }
				generator.MarkLabel(doNothingLabel);

				// return;
				generator.Emit(OpCodes.Ret);
			}
		);

		private static void ImplementObjectMethods(TypeBuilder typeBuilder, FieldInfo handlerField, ImmutableGenerationOptions options) {
			Action<TypeBuilder, MethodInfo, FieldInfo, bool>? action = options.ObjectMethodBehaviour switch {
				GenerationOptions.EObjectMethodBehaviour.Bind => BindMethod,
				GenerationOptions.EObjectMethodBehaviour.Forward => ForwardMethod,
				GenerationOptions.EObjectMethodBehaviour.Ignore => null,
				_ => throw new InvalidOperationException()
			};

			if (action == null) {
				return;
			}

			foreach (MethodInfo baseMethod in Types.Object.GetMethods(RelevantFlagsForBinding)
												   .Where(method => method.IsVirtual)
												   .Where(method => !method.IsSpecialName)
												   .Where(method => !method.IsFinal)) {
				action(typeBuilder, baseMethod, handlerField, options.SealTypeAndMethods);
			}
		}

		private static MethodBuilder OverrideMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Action<ILGenerator>? generatorCallback = null, bool markFinal = false) {
			if (baseMethod.IsGenericMethod && !baseMethod.IsConstructedGenericMethod) {
				throw new ArgumentException($"{nameof(baseMethod)}.{nameof(baseMethod.IsGenericMethod)} && !{nameof(baseMethod)}.{nameof(baseMethod.IsConstructedGenericMethod)}");
			}

			generatorCallback ??= generator => generator.ThrowException(Types.NotImplementedException);

			MethodAttributes attributes = baseMethod.Attributes & ~MethodAttributes.Abstract;

			if (markFinal) {
				attributes |= MethodAttributes.Final;
			} else {
				attributes |= MethodAttributes.Virtual;
			}

			ParameterInfo[] parameters = baseMethod.GetParameters();
			MethodBuilder methodBuilder = typeBuilder.DefineMethod(baseMethod.Name, attributes, baseMethod.ReturnType, parameters.Select(parameter => parameter.ParameterType).ToArray());

			foreach ((ParameterInfo Info, ParameterBuilder Builder) tuple in parameters.Select((parameter, index) => (Info: parameter, Builder: methodBuilder.DefineParameter(index + 1, parameter.Attributes, parameter.Name)))) {
				(ParameterInfo parameterInfo, ParameterBuilder parameterBuilder) = tuple;

				if ((parameterInfo.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault) {
					parameterBuilder.SetConstant(parameterInfo.DefaultValue);
				}
			}

			generatorCallback.Invoke(methodBuilder.GetILGenerator());

			if (baseMethod.IsVirtual || baseMethod.IsAbstract) {
				typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
			}

			return methodBuilder;
		}
	}
}
