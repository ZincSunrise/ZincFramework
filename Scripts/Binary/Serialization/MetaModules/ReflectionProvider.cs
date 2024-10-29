using System;
using System.Diagnostics;
using System.Reflection;
using ZincFramework.Serialization.Runtime;



namespace ZincFramework.Binary.Serialization.MetaModule
{
    public class ReflectionProvider : IAccessorsProvider
    {
        public Func<object> GetConstructor(Type type)
        {
            Debug.Assert(type != null);
            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            return () => constructorInfo.Invoke(null);
        }

        public (Func<object, object>, Action<object, object>) GetField(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        public (Func<object, T>, Action<object, T>) GetField<T>(FieldInfo fieldInfo)
        {
            throw new NotImplementedException();
        }

        public (Func<TOwner, TValue>, Action<TOwner, TValue>) GetField<TOwner, TValue>(FieldInfo fieldInfo)
        {
            Func<TOwner, TValue> getter = EmitTool.GetGetField<TOwner, TValue>(fieldInfo);
            Action<TOwner, TValue> setter = EmitTool.GetSetField<TOwner, TValue>(fieldInfo);

            return (getter, setter);
        }


        public (Func<object, object>, Action<object, object>) GetPropertyAccessors(PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        public (Func<object, T>, Action<object, T>) GetPropertyAccessors<T>(PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        public (Func<TOwner, TValue>, Action<TOwner, TValue>) GetPropertyAccessors<TOwner, TValue>(PropertyInfo propertyInfo)
        {
            Func<TOwner, TValue> getter = (Func<TOwner, TValue>)propertyInfo.GetGetMethod(true).CreateDelegate(typeof(Func<TOwner, TValue>));
            Action<TOwner, TValue> setter = (Action<TOwner, TValue>)propertyInfo.GetSetMethod(true).CreateDelegate(typeof(Action<TOwner, TValue>));

            return (getter, setter);
        }

        public Action<TList, int> SetSizeField<TList>(FieldInfo fieldInfo)
        {
            return (list, size) => fieldInfo.SetValue(list, size);
        }
    }
}
