using System;
using System.Reflection;


namespace ZincFramework
{
    public class BaseSafeSingleton<T> where T : class
    {
        protected static Lazy<T> _instance = new Lazy<T>(()=>
        {
            ConstructorInfo constructorInfo = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic
                    , null, Type.EmptyTypes, null);
            return constructorInfo.Invoke(null) as T;
        });

        public static T Instance => _instance.Value;
    }
}