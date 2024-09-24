using System.Collections.Generic;
using ZincFramework.DataPool;



namespace ZincFramework.Binary.Serialization
{
    public static class ByteWriterPool
    {
        public static ByteWriter GetCachedWriter(SerializerOption serializerOption, out PooledBufferWriter pooledBufferWriter)
        {
            pooledBufferWriter = DataPoolManager.RentInfo(() => new PooledBufferWriter(serializerOption.DefaultBufferSize));

            ByteWriter byteWriter = DataPoolManager.RentInfo<ByteWriter>();
            byteWriter.Reset(pooledBufferWriter);
            byteWriter.SetOption(serializerOption.GetWriterOption());
            return byteWriter;
        }

        public static void ReturnWriter(ByteWriter writer, PooledBufferWriter pooledBuffer) 
        {
            DataPoolManager.ReturnInfo(writer);
            DataPoolManager.ReturnInfo(pooledBuffer);
        }
    }
}