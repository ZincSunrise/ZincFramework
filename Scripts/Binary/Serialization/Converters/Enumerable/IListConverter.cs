using System.Collections;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class IListConverter<TList> : IEnumerableConverter<TList, object> where TList : IList
    {
        public override TList Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            TList list = (TList)serializerOption.GetTypeInfo(ConvertType).CreateInstance();
            int count = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);

            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            for (int i = 0; i < count; i++)
            {
                list.Add(_elementTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption));
            }

            return list;

        }

        public override void Write(TList data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            SimpleConverters.Int32Converter.Write(data.Count, byteWriter, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);

            for (int i = 0; i < data.Count; i++)
            {
                _elementTypeInfo.WrapperConverter.Write(data[i], byteWriter, serializerOption);
            }

        }
    }
}