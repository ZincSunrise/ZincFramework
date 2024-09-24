using System;
using System.Reflection;
using ZincFramework.Serialization;
using static GameSystem.ThreePuzzle.GameTile.BaseTile;


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
