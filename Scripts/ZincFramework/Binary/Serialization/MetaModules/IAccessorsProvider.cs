using System;
using System.Reflection;


namespace ZincFramework.Binary.Serialization.MetaModule
{
    public interface IAccessorsProvider
    {
        (Func<object, object>, Action<object, object>) GetPropertyAccessors(PropertyInfo propertyInfo);

        (Func<object, T>, Action<object, T>) GetPropertyAccessors<T>(PropertyInfo propertyInfo);

        (Func<TOwner, TValue>, Action<TOwner, TValue>) GetPropertyAccessors<TOwner, TValue>(PropertyInfo propertyInfo);


        (Func<object, object>, Action<object, object>) GetField(FieldInfo fieldInfo);

        (Func<object, T>, Action<object, T>) GetField<T>(FieldInfo fieldInfo);

        (Func<TOwner, TValue>, Action<TOwner, TValue>) GetField<TOwner, TValue>(FieldInfo fieldInfo);

        Action<TList, int> SetSizeField<TList>(FieldInfo fieldInfo);

        Func<object> GetConstructor(Type type);
    }

}