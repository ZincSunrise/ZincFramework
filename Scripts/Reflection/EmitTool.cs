using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Emit;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZincFramework.Refelction
{
    //使用IL2CPP将无法使用该类
    public static class EmitTool
    {
        #region PropertyInfo
        /// <summary>
        /// 仅支持无参构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<object> GetDefaultConstructor(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            Debug.Assert(constructorInfo != null);

            DynamicMethod dynamicMethod = new DynamicMethod($"new{constructorInfo.Name}", constructorInfo.DeclaringType, Type.EmptyTypes);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
            ilGenerator.Emit(OpCodes.Ret);

            return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
        }

        /// <summary>
        /// 仅支持无参构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T> GetDefaultConstructor<T>()
        {
            ConstructorInfo constructorInfo = typeof(T).GetConstructor(Type.EmptyTypes);
            DynamicMethod dynamicMethod = new DynamicMethod($"new{constructorInfo.Name}", constructorInfo.DeclaringType, Type.EmptyTypes);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
            ilGenerator.Emit(OpCodes.Ret);

            return (Func<T>)dynamicMethod.CreateDelegate(typeof(Func<T>));
        }

        public static Func<object, T> GetGetAccessor<T>(PropertyInfo propertyInfo)
        {
#if UNITY_EDITOR

            if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP)
            {
                throw new NotSupportedException("IL2CPP编译不支持任何有关Emit的方法");
            }
#endif
            Type type = typeof(T);
            Type objType = typeof(object);
            Type declaringType = propertyInfo.DeclaringType;

            var method = new DynamicMethod($"<{declaringType.Name}Set_Method>", type, new Type[] { objType });
            var generator = method.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, declaringType);
            generator.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod() ?? propertyInfo.GetGetMethod(true));
            generator.Emit(OpCodes.Ret);

            return (Func<object, T>)method.CreateDelegate(typeof(Func<object, T>));
        }

        public static Func<object, object> GetGetAccessor(PropertyInfo propertyInfo)
        {
#if UNITY_EDITOR
            if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP)
            {
                throw new NotSupportedException("IL2CPP编译不支持任何有关Emit的方法");
            }
#endif

            Type declaringType = propertyInfo.DeclaringType;
            Type propertyType = propertyInfo.PropertyType;

            DynamicMethod dynamicMethod = new DynamicMethod("<get_GetMethod>", typeof(object), new Type[] { typeof(object) });
            ILGenerator il = dynamicMethod.GetILGenerator();

            il.DeclareLocal(propertyType);
            Label label = il.DefineLabel();

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, declaringType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod() ?? propertyInfo.GetGetMethod(true));

            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, label);

            il.MarkLabel(label);
            il.Emit(OpCodes.Ldloc_0);

            if (propertyType.IsValueType)
            {
                il.Emit(OpCodes.Box, propertyType);
            }

            il.Emit(OpCodes.Ret);
            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }

        /// <summary>
        /// 可以适用于枚举和普通字面量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static Action<object, T> GetSetAccessor<T>(PropertyInfo propertyInfo)
        {
#if UNITY_EDITOR
            if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP)
            {
                throw new NotSupportedException("IL2CPP编译不支持任何有关Emit的方法");
            }
#endif

            Type declearingType = propertyInfo.DeclaringType;
            Type valueType = typeof(T);

            DynamicMethod dynamicMethod = new DynamicMethod("<set_SetProperty>", typeof(void), new Type[] { typeof(object), valueType });
            ILGenerator generator = dynamicMethod.GetILGenerator();

            dynamicMethod.DefineParameter(1, ParameterAttributes.None, "this");
            dynamicMethod.DefineParameter(2, ParameterAttributes.None, "value");

            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, declearingType);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod() ?? propertyInfo.GetSetMethod(true));
            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ret);


            return (Action<object, T>)dynamicMethod.CreateDelegate(typeof(Action<object, T>));
        }

        public static Action<object, object> GetSetAccessor(PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            return propertyType.IsPrimitive ? GetPrimitiveSet(propertyInfo, propertyType) : GetOtherSet(propertyInfo, propertyType);
        }

        private static Action<object, object> GetPrimitiveSet(PropertyInfo propertyInfo, Type propertyType)
        {
            Type declearingType = propertyInfo.DeclaringType;

            Type convertiable = typeof(IConvertible);
            MethodInfo castMethod = convertiable.GetMethod("To" + propertyType.Name, BindingFlags.Public | BindingFlags.Instance);

            DynamicMethod dynamicMethod = new DynamicMethod("<set_SetProperty>", typeof(void), new Type[] { typeof(object), typeof(object) });
            ILGenerator generator = dynamicMethod.GetILGenerator();


            dynamicMethod.DefineParameter(1, ParameterAttributes.None, "this");
            dynamicMethod.DefineParameter(2, ParameterAttributes.None, "value");

            generator.DeclareLocal(propertyType);
            generator.DeclareLocal(typeof(bool));

            generator.Emit(OpCodes.Nop);

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Isinst, propertyType);

            var transLabel1 = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse_S, transLabel1);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Unbox_Any, propertyType);
            generator.Emit(OpCodes.Stloc_0);

            generator.Emit(OpCodes.Ldc_I4_1);

            var transLabel2 = generator.DefineLabel();
            generator.Emit(OpCodes.Br_S, transLabel2);

            generator.MarkLabel(transLabel1);
            generator.Emit(OpCodes.Ldc_I4_0);

            generator.MarkLabel(transLabel2);
            generator.Emit(OpCodes.Stloc_1);

            generator.Emit(OpCodes.Ldloc_1);

            var elseBeginLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse_S, elseBeginLabel);
            generator.Emit(OpCodes.Nop);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, declearingType);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod() ?? propertyInfo.GetSetMethod(true));
            generator.Emit(OpCodes.Nop);

            generator.Emit(OpCodes.Nop);
            var returnLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Br_S, returnLabel);

            generator.MarkLabel(elseBeginLabel);
            generator.Emit(OpCodes.Nop);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, declearingType);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Castclass, convertiable);
            generator.Emit(OpCodes.Ldnull);
            generator.Emit(OpCodes.Callvirt, castMethod);
            generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod() ?? propertyInfo.GetSetMethod(true));

            generator.Emit(OpCodes.Nop);

            generator.Emit(OpCodes.Nop);

            generator.MarkLabel(returnLabel);
            generator.Emit(OpCodes.Ret);
            return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
        }

        private static Action<object, object> GetOtherSet(PropertyInfo propertyInfo, Type propertyType)
        {
            Type declearingType = propertyInfo.DeclaringType;

            DynamicMethod dynamicMethod = new DynamicMethod("<set_SetProperty>", typeof(void), new Type[] { typeof(object), typeof(object) });
            ILGenerator generator = dynamicMethod.GetILGenerator();

            dynamicMethod.DefineParameter(1, ParameterAttributes.None, "this");
            dynamicMethod.DefineParameter(2, ParameterAttributes.None, "value");

            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, declearingType);
            generator.Emit(OpCodes.Ldarg_1);

            generator.Emit(propertyType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, propertyType);

            MethodInfo methodInfo = propertyInfo.GetSetMethod() ?? propertyInfo.GetSetMethod(true) ??
                throw new ArgumentNullException($"传入的{declearingType.Name}类的{propertyInfo.Name}没有setter");

            generator.Emit(OpCodes.Callvirt, methodInfo);
            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ret);


            return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
        }

        #endregion

        #region FieldInfo
        public static Func<object, object> GetGetField(FieldInfo fieldInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod($"<get_{fieldInfo.Name}field>", typeof(object), new Type[] { typeof(object) }, true);
            ILGenerator il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.CastOrUnBox(fieldInfo.DeclaringType);

            il.Emit(OpCodes.Ldfld, fieldInfo);
            il.CastOrBox(fieldInfo.FieldType);
            il.Emit(OpCodes.Ret);

            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }

        public static Action<object, object> GetSetField(FieldInfo fieldInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod($"<set_{fieldInfo.Name}field>", typeof(void), new Type[] { typeof(object), typeof(object) }, true);
            ILGenerator il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.CastOrUnBox(fieldInfo.DeclaringType);

            il.Emit(OpCodes.Ldarg_1);
            il.CastOrUnBox(fieldInfo.FieldType);

            il.Emit(OpCodes.Stfld, fieldInfo);
            il.Emit(OpCodes.Ret);

            return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
        }

        public static Func<object, TR> GetGetField<TR>(FieldInfo fieldInfo)
        {
            Contract.Assert(fieldInfo.FieldType == typeof(TR), $"{fieldInfo.FieldType.Name}, {typeof(TR)}");

            DynamicMethod dynamicMethod = new DynamicMethod($"<get_{fieldInfo.Name}field>", typeof(TR), new Type[] { typeof(object) }, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.CastOrUnBox(fieldInfo.DeclaringType);

            il.Emit(OpCodes.Ldfld, fieldInfo);
            il.Emit(OpCodes.Ret);

            return (Func<object, TR>)dynamicMethod.CreateDelegate(typeof(Func<object, TR>));
        }

        public static Action<object, TS> GetSetField<TS>(FieldInfo fieldInfo)
        {
            Contract.Assert(fieldInfo.FieldType == typeof(TS));

            DynamicMethod dynamicMethod = new DynamicMethod($"<set_{fieldInfo.Name}field>", typeof(void), new Type[] { typeof(object), typeof(TS) }, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.CastOrBox(fieldInfo.DeclaringType);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fieldInfo);
            il.Emit(OpCodes.Ret);

            return (Action<object, TS>)dynamicMethod.CreateDelegate(typeof(Action<object, TS>));
        }

        public static Func<T, TR> GetGetField<T, TR>(FieldInfo fieldInfo)
        {
            Contract.Assert(fieldInfo.FieldType == typeof(TR));

            DynamicMethod dynamicMethod = new DynamicMethod($"<get_{fieldInfo.Name}field>", typeof(TR), new Type[] { typeof(T) }, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldInfo);
            il.Emit(OpCodes.Ret);

            return (Func<T, TR>)dynamicMethod.CreateDelegate(typeof(Func<T, TR>));
        }

        public static Action<T, TS> GetSetField<T, TS>(FieldInfo fieldInfo)
        {
            Contract.Assert(fieldInfo.FieldType == typeof(TS));

            DynamicMethod dynamicMethod = new DynamicMethod($"<set_{fieldInfo.Name}field>", typeof(void), new Type[] { typeof(T), typeof(TS) }, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fieldInfo);
            il.Emit(OpCodes.Ret);

            return (Action<T, TS>)dynamicMethod.CreateDelegate(typeof(Action<T, TS>));
        }
        #endregion

        private static void CastOrBox(this ILGenerator iLGenerator, Type targetType)
        {
            Contract.Assert(targetType != null);
            if (targetType.IsValueType)
            {
                iLGenerator.Emit(OpCodes.Box, targetType);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Castclass, targetType);
            }
        }

        private static void CastOrUnBox(this ILGenerator iLGenerator, Type targetType)
        {
            Contract.Assert(targetType != null);
            if (targetType.IsValueType)
            {
                iLGenerator.Emit(OpCodes.Unbox_Any, targetType);
            }
            else
            {
                iLGenerator.Emit(OpCodes.Castclass, targetType);
            }
        }
    }
}

