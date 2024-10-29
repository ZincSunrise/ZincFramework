using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Runtime
        {
            public static class ExpressionTool
            {
                internal static Dictionary<Type, MethodInfo> PrimitiveConvertMethods { get; } = new Dictionary<Type, MethodInfo>(12) 
                {
                    { typeof(int), typeof(IConvertible).GetMethod($"ToInt32", BindingFlags.Instance | BindingFlags.Public)},
                    { typeof(float), typeof(IConvertible).GetMethod($"ToSingle", BindingFlags.Instance | BindingFlags.Public)},
                    { typeof(bool), typeof(IConvertible).GetMethod($"ToBoolean", BindingFlags.Instance | BindingFlags.Public)},
                };

                public static Func<object, object> GetPropertyGetMethod(PropertyInfo propertyInfo)
                {
                    Type declaringType = propertyInfo.DeclaringType;

                    var target = Expression.Parameter(typeof(object), "target");

                    var callMethod = Expression.Call(Expression.Convert(target, declaringType), propertyInfo.GetGetMethod() ?? propertyInfo.GetGetMethod(true));
                    var convert = Expression.Convert(callMethod, typeof(object));

                    return Expression.Lambda<Func<object, object>>(convert, target).Compile();
                }

                public static Func<object, T> GetPropertyGetMethod<T>(PropertyInfo propertyInfo)
                {
                    Type declaringType = propertyInfo.DeclaringType;

                    var target = Expression.Parameter(typeof(object), "target");

                    var callMethod = Expression.Call(Expression.Convert(target, declaringType), propertyInfo.GetGetMethod() ?? propertyInfo.GetGetMethod(true));
                    var convert = Expression.Convert(callMethod, typeof(T));

                    return Expression.Lambda<Func<object, T>>(convert, target).Compile();
                }

                public static Action<object, object> GetPropertySetMethod(PropertyInfo propertyInfo)
                {
                    Type propertyType = propertyInfo.PropertyType;
                    Type declaringType = propertyInfo.DeclaringType;

                    var target = Expression.Parameter(typeof(object), "target");
                    var value = Expression.Parameter(typeof(object), "value");

                    Expression convertExp;
                    if (propertyType.IsPrimitive)
                    {
                        if (!PrimitiveConvertMethods.TryGetValue(propertyType, out var info))
                        {
                            info = typeof(IConvertible).GetMethod($"To{propertyType.Name}", BindingFlags.Instance | BindingFlags.Public);
                            PrimitiveConvertMethods.Add(propertyType, info);
                        }

                        convertExp = Expression.Call(Expression.Convert(value, typeof(IConvertible)), info, Expression.Constant(null, typeof(IFormatProvider)));
                    }
                    else
                    {
                        convertExp = Expression.Convert(value, propertyType);
                    }


                    var callExp = Expression.Call(Expression.Convert(target, declaringType), propertyInfo.GetSetMethod() ?? propertyInfo.GetGetMethod(true), convertExp);
                    return Expression.Lambda<Action<object, object>>(callExp, target, value).Compile();
                }

                public static Action<object, T> GetPropertySetMethod<T>(PropertyInfo propertyInfo)
                {
                    Type declaringType = propertyInfo.DeclaringType;

                    var target = Expression.Parameter(typeof(object), "target");
                    var value = Expression.Parameter(typeof(T), "value");

                    var callExp = Expression.Call(Expression.Convert(target, declaringType), propertyInfo.GetSetMethod() ?? propertyInfo.GetGetMethod(true), value);
                    return Expression.Lambda<Action<object, T>>(callExp, target, value).Compile();
                }


                #region 表达式树构造函数生成相关
                /// <summary>
                /// 返回值类型不一定和参数的Type相同，有可能是继承关系
                /// </summary>
                /// <typeparam name="T">返回值类型</typeparam>
                /// <param name="type">参数传入的Type</param>
                /// <param name="bindingFlags"></param>
                /// <returns></returns>
                public static Func<T> GetConstructor<T>(Type type, params BindingFlags[] bindingFlags)
                {
                    BindingFlags bindingFlag = BindingFlags.Default;

                    if (bindingFlags != null)
                    {
                        for (int i = 0; i < bindingFlags.Length; i++)
                        {
                            bindingFlag |= bindingFlags[i];
                        }
                    }

                    ConstructorInfo constructorInfo;
                    if (bindingFlag == BindingFlags.Default)
                    {
                        constructorInfo = type.GetConstructor(Type.EmptyTypes);
                    }
                    else
                    {
                        constructorInfo = type.GetConstructor(bindingFlag, null, Type.EmptyTypes, null);
                    }

                    return Expression.Lambda<Func<T>>(Expression.New(constructorInfo)).Compile();
                }

                public static Func<object> GetConstructor(Type type, params BindingFlags[] bindingFlags)
                {
                    BindingFlags bindingFlag = BindingFlags.Default;

                    if (bindingFlags != null)
                    {
                        for (int i = 0; i < bindingFlags.Length; i++)
                        {
                            bindingFlag |= bindingFlags[i];
                        }
                    }
                    ConstructorInfo constructorInfo = type.GetConstructor(bindingFlag, null, Type.EmptyTypes, null);
                    return Expression.Lambda<Func<object>>(Expression.New(constructorInfo)).Compile();
                }

                public static Func<object> GetConstructor(Type type)
                {
                    ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes) ??
                        throw new ArgumentNullException($"传入的{type.Name}类没有构造函数"); ;

                    return Expression.Lambda<Func<object>>(Expression.New(constructorInfo)).Compile();
                }

                /// <summary>
                /// 返回值类型不一定和参数的Type相同，有可能是继承关系
                /// </summary>
                /// <typeparam name="T">参数类型</typeparam>
                /// <typeparam name="K">返回值类型</typeparam>
                /// <param name="type">参数传入的Type</param>
                /// <param name="bindingFlags">标识符</param>
                /// <returns></returns>
                public static Func<T, K> GetConstructor<T, K>(Type type, params BindingFlags[] bindingFlags)
                {
                    BindingFlags bindingFlag = BindingFlags.Default;

                    if (bindingFlags != null)
                    {
                        for (int i = 0; i < bindingFlags.Length; i++)
                        {
                            bindingFlag |= bindingFlags[i];
                        }
                    }

                    Type[] types = new Type[1];
                    types[0] = typeof(T);

                    ConstructorInfo constructorInfo = type.GetConstructor(bindingFlag, null, types, null);

                    Expression newExpression;
                    ParameterExpression[] parameterExpressions = new ParameterExpression[1];
                    parameterExpressions[0] = Expression.Parameter(types[0]);

                    newExpression = Expression.New(constructorInfo, parameterExpressions);
                    Expression<Func<T, K>> lamda = Expression.Lambda<Func<T, K>>(newExpression, parameterExpressions);

                    return lamda.Compile();
                }


                /// <summary>
                /// 返回值类型不一定和参数的Type相同，有可能是继承关系
                /// </summary>
                /// <typeparam name="T">参数类型</typeparam>
                /// <typeparam name="L">另一个参数类型</typeparam>
                /// <typeparam name="K">返回值类型</typeparam>
                /// <param name="type">参数传入的Type</param>
                /// <param name="bindingFlags">标识符</param>
                /// <returns></returns>
                public static Func<T, L, K> GetConstructor<T, L, K>(Type type, params BindingFlags[] bindingFlags)
                {
                    BindingFlags bindingFlag = BindingFlags.Default;

                    if (bindingFlags != null)
                    {
                        for (int i = 0; i < bindingFlags.Length; i++)
                        {
                            bindingFlag |= bindingFlags[i];
                        }
                    }
                    int length = 2;

                    Type[] types = new Type[length];
                    ParameterExpression[] parameterExpressions = new ParameterExpression[length];
                    types[0] = typeof(T);
                    types[1] = typeof(L);

                    ConstructorInfo constructorInfo = type.GetConstructor(bindingFlag, null, types, null);
                    Expression newExpression;

                    for (int i = 0; i < parameterExpressions.Length; i++)
                    {
                        parameterExpressions[i] = Expression.Parameter(types[i]);
                    }

                    newExpression = Expression.New(constructorInfo, parameterExpressions);
                    Expression<Func<T, L, K>> lamda = Expression.Lambda<Func<T, L, K>>(newExpression, parameterExpressions);

                    return lamda.Compile();
                }
                #endregion
            }
        }
    }
}