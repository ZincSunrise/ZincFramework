using System;
using System.Collections.Generic;
using System.Reflection;
using ZincFramework.Serialization.Exceptions;
using ZincFramework.Serialization.Runtime;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Cache
        {
            public class TypeCache
            {
                public Type CacheType { get; }

                public Func<object> Constructor { get; }

                public int SerializableCode { get; }

                public MemberConfig[] MemberConfigs { get; }

                public SerializeConfig SerializeConfig { get; }

                internal TypeCache(Type type, SerializeConfig serializeConfig)
                {
                    CacheType = type;
                    if (type.IsDefined(typeof(ZincSerializable)))
                    {
                        ZincSerializable zincSerializable = type.GetCustomAttribute<ZincSerializable>(false) ?? throw new NonSerializableAttributeException();
                        SerializableCode = zincSerializable.SerializableCode;
                    }

                    SerializeConfig = serializeConfig;
                    Constructor = ExpressionTool.GetConstructor(type);

                    List<MemberConfig> memberConfigs = new List<MemberConfig>();

                    if ((serializeConfig & SerializeConfig.Field) != 0)
                    {
                        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                        for (int i = 0; i < fieldInfos.Length; i++) 
                        {
                            if (ConfigurationFactory.TryGetMemberConfig(fieldInfos[i], out MemberConfig memberConfig))
                            {
                                memberConfigs.Add(memberConfig);
                            }
                        }

                        if ((serializeConfig & SerializeConfig.NonPublic) != 0)
                        {
                            fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                            for (int i = 0; i < fieldInfos.Length; i++)
                            {
                                if (ConfigurationFactory.TryGetPrivateMemberConfig(fieldInfos[i], out MemberConfig memberConfig))
                                {
                                    memberConfigs.Add(memberConfig);
                                }
                            }
                        }
                    }

                    if ((serializeConfig & SerializeConfig.Property) != 0)
                    {
                        PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        for (int i = 0; i < propertyInfos.Length; i++)
                        {
                            if (ConfigurationFactory.TryGetMemberConfig(propertyInfos[i], out MemberConfig memberConfig))
                            {
                                memberConfigs.Add(memberConfig);
                            }
                        }


                        if ((serializeConfig & SerializeConfig.NonPublic) != 0)
                        {
                            propertyInfos = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
                            for (int i = 0; i < propertyInfos.Length; i++)
                            {
                                if (ConfigurationFactory.TryGetMemberConfig(propertyInfos[i], out MemberConfig memberConfig))
                                {
                                    memberConfigs.Add(memberConfig);
                                }
                            }
                        }
                    }

                    MemberConfigs = memberConfigs.ToArray();
                }


                public object CreateInstance()
                {
                    return Constructor?.Invoke();
                }
            }
        }
    }
}

