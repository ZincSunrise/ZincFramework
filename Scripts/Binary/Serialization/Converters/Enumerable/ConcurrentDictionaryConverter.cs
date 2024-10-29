using System.Collections.Concurrent;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class ConcurrentDictionaryConverter<TKey, TValue> : IDictionaryConverter<ConcurrentDictionary<TKey, TValue>, TKey, TValue>
    {
        public override ConcurrentDictionary<TKey, TValue> Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = byteReader.ReadInt32();
            ConcurrentDictionary<TKey, TValue> dict = new ConcurrentDictionary<TKey, TValue>();

            _keyTypeInfo ??= GetTypeInfo<TKey>(serializerOption);
            _valueTypeInfo ??= GetTypeInfo<TValue>(serializerOption);

            for (int i = 0; i < count; i++)
            {
                dict.TryAdd(_keyTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption), _valueTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption));
            }

            return dict;
        }

        public override void Write(ConcurrentDictionary<TKey, TValue> data, ByteWriter byteWriter, SerializerOption serializerOption)
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
