using System;
using System.Reflection;
using ZincFramework.Serialization;


namespace ZincFramework.Binary.Serialization
{
    public static class ReflectionExtension 
    {
        public static bool TryGetConstructor(this Type type, out ConstructorInfo constructorInfo)
        {
            ConstructorInfo[] constructorInfos = type.GetConstructors();


            int nowWeight = int.MinValue;
            constructorInfo = null;
            for (int i = 0; i < constructorInfos.Length; i++) 
            {
                if (constructorInfos[i].IsDefined(typeof(BinaryConstructor)))
                {
                    BinaryConstructor binaryConstructor = constructorInfos[i].GetCustomAttribute<BinaryConstructor>();
                    if(constructorInfo == null)
                    {
                        constructorInfo = constructorInfos[i];
                        nowWeight = binaryConstructor.Weight;
                    }
                    else
                    {
                        constructorInfo = nowWeight < binaryConstructor.Weight ? 
                            constructorInfos[i] : constructorInfo;
                    }
                }
            }

            return constructorInfo == null;
        }
    }
}
