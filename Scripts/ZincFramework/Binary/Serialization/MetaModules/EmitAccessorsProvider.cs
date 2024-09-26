using System;
using System.Reflection;
using ZincFramework.Serialization.Runtime;



namespace ZincFramework.Binary.Serialization.MetaModule
{
    public class EmitAccessorsProvider : IAccessorsProvider
    {
        public Func<object> GetConstructor(Type type)
        {
            return EmitTool.GetDefaultConstructor(type);
        }



        public (Func<object, object>, Action<object, object>) GetField(FieldInfo fieldInfo)
        {
            Func<object, object> getter = EmitTool.GetGetField(fieldInfo);
            Action<object, object> setter = EmitTool.GetSetField(fieldInfo);
            return (getter, setter);
        }

        public (Func<object, T>, Action<object, T>) GetField<T>(FieldInfo fieldInfo)
        {
            Func<object, T> getter = EmitTool.GetGetField<T>(fieldInfo);
            Action<object, T> setter = EmitTool.GetSetField<T>(fieldInfo);
            return (getter, setter);
        }

        public (Func<TOwner, TValue>, Action<TOwner, TValue>) GetField<TOwner, TValue>(FieldInfo fieldInfo)
        {
            Func<TOwner, TValue> getter = EmitTool.GetGetField<TOwner, TValue>(fieldInfo);
            Action<TOwner, TValue> setter = EmitTool.GetSetField<TOwner, TValue>(fieldInfo);

            return (getter, setter);
        }

        public (Func<object, object>, Action<object, object>) GetPropertyAccessors(PropertyInfo propertyInfo)
        {
            Func<object, object> getter = EmitTool.GetGetAccessor(propertyInfo);
            Action<object, object> setter = EmitTool.GetSetAccessor(propertyInfo);

            return (getter, setter);
        }

        public (Func<object, T>, Action<object, T>) GetPropertyAccessors<T>(PropertyInfo propertyInfo)
        {
            Func<object, T> getter = EmitTool.GetGetAccessor<T>(propertyInfo);
            Action<object, T> setter = EmitTool.GetSetAccessor<T>(propertyInfo);
            return (getter, setter);
        }

        public (Func<TOwner, TValue>, Action<TOwner, TValue>) GetPropertyAccessors<TOwner, TValue>(PropertyInfo propertyInfo)
        {
            Func<TOwner, TValue> getter = (Func<TOwner, TValue>)propertyInfo.GetGetMethod(true).CreateDelegate(typeof(Func<TOwner, TValue>));
            Action<TOwner, TValue> setter = (Action<TOwner, TValue>)propertyInfo.GetSetMethod(true).CreateDelegate(typeof(Action<TOwner, TValue>));

            return (getter, setter);
        }

        public Action<TList, int> SetSizeField<TList>(FieldInfo fieldInfo)
        {
            return EmitTool.GetSetField<TList, int>(fieldInfo);
        }
    }
}