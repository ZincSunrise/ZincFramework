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
            [BinarySerializable()]
            public abstract record BaseMessage : ISerializable, IConvertable, IAppend
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
                public virtual byte[] Serialize()
                {
                    return BinarySerializer.Serialize(this);
                }

                /// <summary>
                /// �����л������裬�ȶ��뵱ǰ��ı��룬�ٶ���ǰ��ĳ��ȣ�����������
                /// ��������֮ǰ����Ҫ�������ǵ����֣��ٶ�����������
                /// ���������л����򱻸ı䣬��ô��һ��Ҫ�ı䷴���л�������Ĺ���
                /// </summary>
                /// <param name="bytes"></param>
                public virtual void Deserialize(byte[] bytes)
                {

                }

                /// <summary>
                /// ��������൱��û��ͷ�ļ������л�����
                /// </summary>
                /// </summary>
                /// <returns></returns>
                public virtual void Append(byte[] buffer, ref int nowIndex)
                {

                }

                /// <summary>
                /// ��������൱��û��ͷ�ļ��ķ����л�����
                /// </summary>
                /// <param name="bytes"></param>
                public virtual void Convert(byte[] bytes, ref int nowIndex)
                {

                }

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


