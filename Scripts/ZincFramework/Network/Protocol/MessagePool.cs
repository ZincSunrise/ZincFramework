using System;
using System.Collections.Concurrent;
using ZincFramework.Binary.Serialization;
using ZincFramework.Binary.Serialization.Metadata;
using ZincFramework.Serialization.Runtime;



namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public static class MessagePool
            {
                private static readonly ConcurrentDictionary<int, MessageInfo> _massageInfos = new ConcurrentDictionary<int, MessageInfo>();

                internal class MessageInfo
                {
                    public Func<BaseMessage> MessageConstructor { get; private set; }

                    public Func<IHandleMessage> HandlderConstructor { get; private set; }

                    public MessageInfo(Func<BaseMessage> messageConstructor, Func<IHandleMessage> handlderConstructor)
                    {
                        MessageConstructor = messageConstructor;
                        HandlderConstructor = handlderConstructor;
                    }
                }

                static MessagePool()
                {

                }

                public static BaseMessage GetBaseMessage(int id)
                {
                    if(_massageInfos.TryGetValue(id, out MessageInfo massageInfo))
                    {
                        return massageInfo.MessageConstructor.Invoke();
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

                public static void RegistMessage(Type messageType, Type handlderType)
                {
                    SerializerOption serializerOption = SerializerOption.Message;
                    BinaryTypeInfo binaryTypeInfo = serializerOption.GetTypeInfo(messageType);

                    if (!_massageInfos.ContainsKey(binaryTypeInfo.SerializableCode))
                    {
                        var ctor = ExpressionTool.GetConstructor<IHandleMessage>(handlderType);
                        _massageInfos.TryAdd(binaryTypeInfo.SerializableCode, new MessageInfo(() => binaryTypeInfo.CreateInstance() as BaseMessage, ctor));
                    }
                }

                public static void RegistMessage<T, K>() where T : BaseMessage, new() where K : IHandleMessage, new()
                {
                    RegistMessage(() => new T(), () => new K());
                }

                public static void RegistMessage<T, K>(Func<T> messageFactory, Func<K> handlerFactory) where T : BaseMessage where K : IHandleMessage
                {
                    SerializerOption serializerOption = SerializerOption.Message;
                    BinaryTypeInfo binaryTypeInfo = serializerOption.GetTypeInfo(messageFactory);

                    if (!_massageInfos.ContainsKey(binaryTypeInfo.SerializableCode))
                    {
                        _massageInfos.TryAdd(binaryTypeInfo.SerializableCode, new MessageInfo(messageFactory, () => handlerFactory.Invoke()));
                    }
                }

                public static void UnRegist<T, K>() where T : BaseMessage where K : IHandleMessage
                {
                    SerializerOption serializerOption = SerializerOption.Message;
                    BinaryTypeInfo binaryTypeInfo = serializerOption.GetTypeInfo<T>();

                    _massageInfos.TryRemove(binaryTypeInfo.SerializableCode, out _);
                }
            }
        }
    }
}

