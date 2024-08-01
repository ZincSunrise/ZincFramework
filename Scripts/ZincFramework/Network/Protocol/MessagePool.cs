using System;
using System.Collections.Generic;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Cache;
using ZincFramework.Serialization.Runtime;



namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            internal class MessageInfo
            {
                
                public Func<object> MessageConstructor { get; private set; }
                public Func<IHandleMessage> HandlderConstructor { get; private set; }

                public MessageInfo(Func<object> messageConstructor, Func<IHandleMessage> handlderConstructor)
                {
                    MessageConstructor = messageConstructor;
                    HandlderConstructor = handlderConstructor;
                }
            }

            public static class MessagePool
            {
                private static readonly Dictionary<int, MessageInfo> _massageInfos = new Dictionary<int, MessageInfo>();

                static MessagePool()
                {

                }

                public static BaseMessage GetBaseMessage(int id)
                {
                    if(_massageInfos.TryGetValue(id, out MessageInfo massageInfo))
                    {
                        return massageInfo.MessageConstructor.Invoke() as BaseMessage;
                    }
                    return null;
                }

                public static IHandleMessage GetBaseHandler(int id, BaseMessage baseMessage)
                {
                    if (_massageInfos.TryGetValue(id, out MessageInfo massageInfo))
                    {
                        IHandleMessage handleMessage = massageInfo.HandlderConstructor.Invoke();
                        handleMessage.Message = baseMessage;
                        return handleMessage;
                    }
                    return null;
                }

                public static void RegistMessage<T, K>() where T : BaseMessage where K : IHandleMessage
                {
                    Type type = typeof(T);
                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, SerializeConfig.Property);
                    }

                    if (!_massageInfos.ContainsKey(typeCache.SerializableCode))
                    {
                        Func<IHandleMessage> handlerConstructor = ExpressionTool.GetConstructor<IHandleMessage>(typeof(K));
                        _massageInfos.Add(typeCache.SerializableCode, new MessageInfo(typeCache.Constructor, handlerConstructor));
                    }
                }

                public static void UnRegist<T, K>() where T : BaseMessage where K : IHandleMessage
                {
                    Type type = typeof(T);

                    if (!SerializationCachePool.TryGetTypeCache(type, out var typeCache))
                    {
                        typeCache = SerializationCachePool.CreateTypeCache(type, SerializeConfig.Property);
                    }

                    if (_massageInfos.ContainsKey(typeCache.SerializableCode))
                    {
                        _massageInfos.Remove(typeCache.SerializableCode);
                    }
                }
            }
        }
    }
}

