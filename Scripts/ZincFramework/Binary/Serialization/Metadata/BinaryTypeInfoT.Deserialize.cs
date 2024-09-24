using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace ZincFramework.Binary.Serialization.Metadata
{
    public sealed partial class BinaryTypeInfo<T> : BinaryTypeInfo
    {
        internal T Deserialize(Stream stream)
        {
            T data = TypeConstructor.Invoke();

            Span<byte> bytes = stackalloc byte[(int)stream.Length];
            stream.Read(bytes);

            ByteReader byteReader = new ByteReader(bytes, _serializerOption.GetReaderOption());
            ReadAndSetProperties(data, ref byteReader);

            return data;
        }

        internal T Deserialize(ref ByteReader byteReader)
        {
            T data = TypeConstructor.Invoke();
            ReadAndSetProperties(data, ref byteReader);

            return data;
        }

        internal async ValueTask<T> DeserializeAsync(Stream stream, CancellationToken cancellationToken)
        {
            T result = TypeConstructor.Invoke();

            BufferReader bufferReader = new BufferReader(_serializerOption.DefaultBufferSize);
            await bufferReader.ReadFromStreamAsync(stream, cancellationToken);
            SerializeInAsync(result, ref bufferReader);

            bufferReader.Dispose();
            return result;
        }

        private void SerializeInAsync(T data, ref BufferReader bufferReader)
        {
            ByteReader byteReader = new ByteReader(bufferReader.OutputMemory, _serializerOption.GetReaderOption());
            ReadAndSetProperties(data, ref byteReader);
        }

        public override object DeserializeAsObject(Stream stream) => Deserialize(stream);

        public override object DeserializeAsObject(ref ByteReader byteReader) => Deserialize(ref byteReader);

        public override async ValueTask<object> DeserializeAsObjectAsync(Stream stream, CancellationToken cancellationToken)
        {
            return await DeserializeAsync(stream, cancellationToken).ConfigureAwait(false);
        }
    }
}
