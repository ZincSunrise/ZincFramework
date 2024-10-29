using System.Collections.Generic;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class DictionaryConverter<TKey, TValue> : IDictionaryConverter<Dictionary<TKey, TValue>, TKey, TValue>
    {
        public override Dictionary<TKey, TValue> Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = byteReader.ReadInt32();
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

            _keyTypeInfo ??= GetTypeInfo<TKey>(serializerOption);
            _valueTypeInfo ??= GetTypeInfo<TValue>(serializerOption);

            for (int i = 0; i < count; i++) 
            {
                dict.Add(_keyTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption), _valueTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption));
            }

            return dict;
        }

        public override void Write(Dictionary<TKey, TValue> data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(data.Count);

            _keyTypeInfo ??= GetTypeInfo<TKey>(serializerOption);
            _valueTypeInfo ??= GetTypeInfo<TValue>(serializerOption);

            foreach (var keyValuePair in data)
            {
                _keyTypeInfo.WrapperConverter.Write(keyValuePair.Key, byteWriter, serializerOption);
                _valueTypeInfo.WrapperConverter.Write(keyValuePair.Value, byteWriter, serializerOption);
            }
        }
    }
}