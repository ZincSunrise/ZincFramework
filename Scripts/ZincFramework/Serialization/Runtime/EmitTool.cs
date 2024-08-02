using System;
using System.Reflection;
using System.Reflection.Emit;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZincFramework
{
    namespace Serialization
    {
        namespace Runtime
        {
            //使用IL2CPP将无法使用该类
            public static class EmitTool
            {
                public static Func<object, T> GetPropertyGetMethod<T>(PropertyInfo propertyInfo)
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
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());
                    generator.Emit(OpCodes.Ret);

                    return (Func<object, T>)method.CreateDelegate(typeof(Func<object, T>));
                }

                public static Func<object, object> GetPropertyGetMethod(PropertyInfo propertyInfo)
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
                    il.Emit(OpCodes.Callvirt, propertyInfo.GetGetMethod());

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


                public static Action<object, T> GetPropertySetMethod<T>(PropertyInfo propertyInfo)
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
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.Emit(OpCodes.Nop);
                    generator.Emit(OpCodes.Ret);


                    return (Action<object, T>)dynamicMethod.CreateDelegate(typeof(Action<object, T>));
                }

                public static Action<object, object> GetPropertySetMethod(PropertyInfo propertyInfo)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    return propertyType.IsPrimitive ? WritePrimitiveSet(propertyInfo, propertyType) : WriteOtherSet(propertyInfo, propertyType);
                }

                private static Action<object, object> WritePrimitiveSet(PropertyInfo propertyInfo, Type propertyType)
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
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
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
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());

                    generator.Emit(OpCodes.Nop);

                    generator.Emit(OpCodes.Nop);

                    generator.MarkLabel(returnLabel);
                    generator.Emit(OpCodes.Ret);
                    return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
                }

                private static Action<object, object> WriteOtherSet(PropertyInfo propertyInfo, Type propertyType)
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

                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.Emit(OpCodes.Nop);
                    generator.Emit(OpCodes.Ret);


                    return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
                }
            }
        }
    }
}

