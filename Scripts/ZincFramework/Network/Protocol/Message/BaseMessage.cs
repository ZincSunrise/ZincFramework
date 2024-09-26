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
            /// ���л������裬��д��ǰ���룬��д��ǰ��ĳ��ȣ����д������
            /// һ���̳��˸��࣬�ͱ����������ľ�̬���캯���е���һ��SerializationCache��ע�᷽��
            /// </summary>
            [ZincSerializable()]
            public abstract class BaseMessage : ISerializable
            {
                public abstract int SerializableCode { get; }

                public virtual int TypeLength => _typeLength == 0 ? GetTypeLength() : _typeLength;


                private int _typeLength = 0;

                /// <summary>
                /// ���л������裬��д��ǰ���룬��д��ǰ��ĳ��ȣ����д������
                /// д��ĳ��ȵ�ʱ����Ҫʹ��GetLength����
                /// дһ����ʱ������������д��һ��������֣���д������ľ�������
                /// ����ı��������������ܹ�ʹ��NetSerializer��ķ����л�����,����Ҫ�Զ��巴���л�����
                /// </summary>
                /// <returns></returns>
                public abstract void Write(ByteWriter byteWriter);

                /// <summary>
                /// �����л������裬�ȶ��뵱ǰ��ı��룬�ٶ���ǰ��ĳ��ȣ�����������
                /// ��������֮ǰ����Ҫ�������ǵ����֣��ٶ�����������
                /// ���������л����򱻸ı䣬��ô��һ��Ҫ�ı䷴���л�������Ĺ���
                /// </summary>
                /// <param name="bytes"></param>
                public abstract void Read(ref ByteReader byteReader);

                /// <summary>
                /// ���л���ʱ�����Ҫ�����������
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
                /// ��д�������Ҫд��ͷ��Ϣ�ĳ���
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


