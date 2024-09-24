using System;
using System.IO;
using System.Buffers;
using System.Threading.Tasks;
using ZincFramework.DataPool;



namespace ZincFramework
{
    namespace Binary
    {
        namespace Serialization
        {
            public partial class ByteWriter : IDisposable, IAsyncDisposable, IResumable
            {
                public BinaryWriterOption WriterOption { get; private set; }


                private IBufferWriter<byte> _bufferWriter;


                private ArrayBufferWriter<byte> _arrayBufferWriter;


                private Stream _stream;

                public ByteWriter()
                {

                }

                public ByteWriter(BinaryWriterOption binaryWriterOption)
                {
                    _bufferWriter = _arrayBufferWriter = new ArrayBufferWriter<byte>();
                    WriterOption = binaryWriterOption;
                }

                public ByteWriter(IBufferWriter<byte> bufferWriter, BinaryWriterOption binaryWriterOption)
                {
                    _bufferWriter = bufferWriter;
                    WriterOption = binaryWriterOption;
                }

                public void Reset(IBufferWriter<byte> bufferWriter)
                {
                    _bufferWriter = bufferWriter;
                    if(_arrayBufferWriter != null)
                    {
                        _arrayBufferWriter?.Clear();
                        _arrayBufferWriter = null;
                    }
                }

                public void Reset(Stream stream)
                {
                    _stream = stream;
                    _arrayBufferWriter?.Clear();
                }

                public void Reset()
                {
                    _arrayBufferWriter?.Clear();
                }

                public void OnReturn()
                {
                    _bufferWriter = null;
                    _arrayBufferWriter = null;
                    _stream = null;
                }

                public void OnRent()
                {
                    
                }

                public void Dispose()
                {
                    _arrayBufferWriter?.Clear();
                    OnReturn();
                    WriterOption = null;
                }

                public async ValueTask DisposeAsync()
                {
                    _arrayBufferWriter?.Clear();
                    OnReturn();
                    WriterOption = null;
                    await new ValueTask();
                }

                public void SetOption(BinaryWriterOption option)
                {
                    WriterOption = option;
                }
            }
        }
    }
}