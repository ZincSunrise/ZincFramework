using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace ZincFramework.Binary.Serialization.Metadata
{
    public sealed partial class BinaryTypeInfo<T> : BinaryTypeInfo
    {
        internal void Serialize(T obj, Stream stream)
        {
            ByteWriter byteWriter = ByteWriterPool.GetCachedWriter(_serializerOption, out PooledBufferWriter pooledBufferWriter);
            
            WrapperConverter.Write(obj, byteWriter, _serializerOption);
            pooledBufferWriter.WriteInStream(stream);

            ByteWriterPool.ReturnWriterAndBuffer(byteWriter, pooledBufferWriter);
        }

        internal void Serialize(T obj, ByteWriter byteWriter)
        {
            WrapperConverter.Write(obj, byteWriter, _serializerOption);
        }

        internal async ValueTask SerializeAsync(T obj, Stream stream, CancellationToken cancellationToken)
        {
            ByteWriter byteWriter = ByteWriterPool.GetCachedWriter(_serializerOption, out PooledBufferWriter pooledBufferWriter);

            WrapperConverter.Write(obj, byteWriter, _serializerOption);
            await pooledBufferWriter.WriteInStreamAsync(stream, cancellationToken);
            pooledBufferWriter.Dispose();
        }

        public override void SerializeAsObject(object obj, Stream stream) => Serialize((T)obj, stream);

        public override void SerializeAsObject(object obj, ByteWriter byteWriter) => Serialize((T)obj, byteWriter);

        public override async ValueTask SerializeAsObjectAsync(object obj, Stream stream, CancellationToken cancellationToken)
        {
            await SerializeAsync((T)obj, stream, cancellationToken);
        }
    }
}
