using System;


namespace ZincFramework.Binary.Serialization
{
    public static class ReflectionExtension 
    {
        public static bool TryGetDefination(this Type type, Type targetType)
        {
            while (type != typeof(object) || type != null) 
            {
                if (type.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }
    }
}
