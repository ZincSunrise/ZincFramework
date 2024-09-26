using ZincFramework.Binary;
using ZincFramework.Binary.Serialization;
using ZincFramework.Serialization;


namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            /// <summary>
            /// 序列化三步骤，先写当前编码，在写当前类的长度，最后写各个类
            /// 一旦继承了该类，就必须在这个类的静态构造函数中调用一次SerializationCache的注册方法
            /// </summary>
            [ZincSerializable()]
            public abstract class BaseMessage : ISerializable
            {
                public abstract int SerializableCode { get; }

                public virtual int TypeLength => _typeLength == 0 ? GetTypeLength() : _typeLength;


                private int _typeLength = 0;

                /// <summary>
                /// 序列化三步骤，先写当前编码，在写当前类的长度，最后写各个类
                /// 写类的长度的时候，需要使用GetLength方法
                /// 写一个类时，先向数组里写入一个类的名字，在写入其类的具体内容
                /// 如果改变了这条规则，则不能够使用NetSerializer里的反序列化方法,必须要自定义反序列化方法
                /// </summary>
                /// <returns></returns>
                public abstract void Write(ByteWriter byteWriter);

                /// <summary>
                /// 反序列化三步骤，先读入当前类的编码，再读当前类的长度，最后读各个类
                /// 读各个类之前首先要读出他们的名字，再读入数据内容
                /// 如果你的序列化规则被改变，那么请一定要改变反序列化方法里的规则
                /// </summary>
                /// <param name="bytes"></param>
                public abstract void Read(ref ByteReader byteReader);

                /// <summary>
                /// 序列化的时候必须要调用这个方法
                /// </summary>
                /// <returns></returns>
                public virtual int GetTypeLength()
                {
                    if(_typeLength == 0)
                    {
                        _typeLength = TypeMeasurer.GetTypeLength(this, this.GetType());
                    }

                    return _typeLength;
                }

                /// <summary>
                /// 重写这个方法要写入头信息的长度
                /// </summary>
                /// <returns></returns>
                public virtual int GetBytesLength()
                {
                    return TypeMeasurer.GetTypeLength(this, GetType()) + HeadInfo.HeadLength;
                }
            }
        }
    }
}


