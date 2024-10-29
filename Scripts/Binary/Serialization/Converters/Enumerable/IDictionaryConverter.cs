using System;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization.Metadata;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class IDictionaryConverter<TDictionary, TKey, TValue> : BinaryConverter<TDictionary> where TDictionary : IDictionary<TKey, TValue>
    {
        public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.Dictionary;

        public override Type KeyType => typeof(TKey);

        public override Type ValueType => typeof(TValue);


        protected BinaryTypeInfo<TKey> _keyTypeInfo;

        protected BinaryTypeInfo<TValue> _valueTypeInfo;

        protected BinaryTypeInfo<T> GetTypeInfo<T>(SerializerOption serializerOption)
        {
            return serializerOption.GetTypeInfo<T>();
        }

        public override TDictionary Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = byteReader.ReadInt32();
            TDictionary dict = serializerOption.GetTypeInfo<TDictionary>().CreateInstance();

            _keyTypeInfo ??= GetTypeInfo<TKey>(serializerOption);
            _valueTypeInfo ??= GetTypeInfo<TValue>(serializerOption);

            for (int i = 0; i < count; i++)
            {
                dict.Add(_keyTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption), _valueTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption));
            }

            return dict;
        }

        public override void Write(TDictionary data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(data.Count);

            _keyTypeInfo ??= GetTypeInfo<TKey>(serializerOption);
            _valueTypeInfo ??= GetTypeInfo<TValue>(serializerOption);

            foreach (var key in data.Keys)
            {
                _keyTypeInfo.WrapperConverter.Write((TKey)key, byteWriter, serializerOption);
                _valueTypeInfo.WrapperConverter.Write((TValue)data[key], byteWriter, serializerOption);
            }
        }
    }
}
