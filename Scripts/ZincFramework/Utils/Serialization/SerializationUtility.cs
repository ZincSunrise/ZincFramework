using System;
using System.Reflection;




namespace ZincFramework
{
    namespace Serialization
    {
        public static class SerializationUtility
        {
            public static Func<TClass, TReturn> GetGetFuc<TClass, TReturn>(PropertyInfo propertyInfo)
            {
                return (Func<TClass, TReturn>)propertyInfo.GetGetMethod().CreateDelegate(typeof(Func<TClass, TReturn>));
            }

            public static Action<TClass, TSet> GetSetAction<TClass, TSet>(PropertyInfo propertyInfo)
            {
                return (Action<TClass, TSet>)propertyInfo.GetSetMethod().CreateDelegate(typeof(Action<TClass, TSet>));
            }
        }
    }
}
