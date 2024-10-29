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
            Span<byte> bytes = stackalloc byte[(int)stream.Length];
            stream.Read(bytes);

            ByteReader byteReader = new ByteReader(bytes, _serializerOption.GetReaderOption());
            T data = WrapperConverter.Read(ref byteReader, _serializerOption);
            return data;
        }

        internal T Deserialize(ref ByteReader byteReader)
        {
            return WrapperConverter.Read(ref byteReader, _serializerOption);
        }

        internal async ValueTask<T> DeserializeAsync(Stream stream, CancellationToken cancellationToken)
        {
            BufferReader bufferReader = new BufferReader(_serializerOption.DefaultBufferSize);
            await bufferReader.ReadFromStreamAsync(stream, cancellationToken);
            
            bufferReader.Dispose();
            return DeserializeInAsync(ref bufferReader);
        } 

        private T DeserializeInAsync(ref BufferReader bufferReader)
        {
            ByteReader byteReader = new ByteReader(bufferReader.OutputMemory, _serializerOption.GetReaderOption());
            return WrapperConverter.Read(ref byteReader, _serializerOption);
        }

        public override object DeserializeAsObject(Stream stream) => Deserialize(stream);

        public override object DeserializeAsObject(ref ByteReader byteReader) => Deserialize(ref byteReader);

        public override async ValueTask<object> DeserializeAsObjectAsync(Stream stream, CancellationToken cancellationToken)
        {
            return await DeserializeAsync(stream, cancellationToken).ConfigureAwait(false);
        }
    }
}
