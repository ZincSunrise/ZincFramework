using System;
using System.Text;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization.Metadata;
using ZincFramework.Binary.Serialization.MetaModule;



namespace ZincFramework
{
   namespace Binary
    {
        namespace Serialization
        {
            public partial class SerializerOption
            {
                public static SerializerOption Default { get; } = new SerializerOption() 
                { 
                    IsGiveupOrdinal = true,
                    IsUsingVariant = true
                };

                public static SerializerOption Message { get; } = new SerializerOption()
                { 
                    Encoding = Encoding.UTF8,
                    IsGiveupOrdinal = false,
                    IsUsingVariant = true
                };

                
                public IList<BinaryConverter> Converters 
                {
                    get
                    {
                        return _converters ??= new List<BinaryConverter>();
                    }
                    set
                    {
                        _converters = value;
                    }
                }

                private BinaryTypeInfo _lastTypeInfo;

                private IList<BinaryConverter> _converters;

                private BinaryWriterOption _writerOption;
      
                private IMetaModule _metaModule;

                public bool IsIncludeField { get; set; } = false;

                public bool IsIncludeStaticValue { get; set; } = false;

                public Encoding Encoding { get; set; } = Encoding.Unicode;

                public bool IsIgnoreNullValue { get; set; } = true;

                public bool IsIgnoreReadOnly { get; set; } = true;

                public int DefaultBufferSize { get; set; } = 1024;

                /// <summary>
                /// 此项代表是否放弃序列号
                /// </summary>
                public bool IsGiveupOrdinal { get; set; } = true;

                /// <summary>
                /// 此项代表是否启用变长编码, 存储的文件大小会减小，但是存储时间会变长, 对数组不生效
                /// </summary>
                public bool IsUsingVariant { get; set; } = false;


                internal BinaryTypeInfo GetTypeInfo(Type type)
                {
                    if(_lastTypeInfo?.CacheType == type)
                    {
                        return _lastTypeInfo; 
                    }

                    return _lastTypeInfo = _serializeCacheInfo.GetOrAddCacheInfo(type, this);
                }

                internal BinaryTypeInfo<T> GetTypeInfo<T>()
                {
                    if(_lastTypeInfo is BinaryTypeInfo<T> typeInfo)
                    {
                        return typeInfo;
                    }

                    return (_lastTypeInfo = _serializeCacheInfo.GetOrAddCacheInfo<T>(this)) as BinaryTypeInfo<T>;
                }

                internal BinaryTypeInfo<T> GetTypeInfo<T>(Func<T> factory)
                {
                    if (factory == null)
                    {
                        return GetTypeInfo<T>();
                    }

                    if (_lastTypeInfo is BinaryTypeInfo<T> typeInfo)
                    {
                        return typeInfo;
                    }
                    return (_lastTypeInfo = _serializeCacheInfo.GetOrAddCacheInfo(factory, this)) as BinaryTypeInfo<T>;
                }

                internal bool TryGetTypeInfo(Type type, out BinaryTypeInfo binaryTypeInfo)
                {
                    if(_serializeCacheInfo == null)
                    {
                        binaryTypeInfo = null;
                        return false;
                    }

                    return _serializeCacheInfo.TryGetCacheInfo(type, out binaryTypeInfo);
                }


                public BinaryConverter FindBinaryConverter(Type type) 
                {
                    for (int i = 0; i < Converters.Count; i++)
                    {
                        if (Converters[i].CanConvert(type))
                        {
                            return Converters[i];
                        }
                    }

                    return null;
                }

                public BinaryWriterOption GetWriterOption()
                {
                    _writerOption ??= new BinaryWriterOption();

                    _writerOption.IsUsingVariant = IsUsingVariant;
                    _writerOption.Encoding = this.Encoding;
                    _writerOption.DefaultBufferSize = this.DefaultBufferSize;
                    return _writerOption;
                }

                public BinaryReaderOption GetReaderOption()
                {
                    return new BinaryReaderOption()
                    {
                        IsUsingVariant = IsUsingVariant,
                        Encoding = this.Encoding,
                    };
                }

                internal BinaryTypeInfo CreateTypeInfo(Type type) 
                {
                    _metaModule ??= new DefaultMetaModule();
                    return _metaModule.CreateTypeInfo(type, this);
                }

                internal BinaryTypeInfo<T> CreateTypeInfo<T>()
                {
                    _metaModule ??= new DefaultMetaModule();
                    return _metaModule.CreateTypeInfo<T>(this);
                }

                internal BinaryTypeInfo<T> CreateTypeInfo<T>(Func<T> factory)
                {
                    _metaModule ??= new DefaultMetaModule();
                    return _metaModule.CreateTypeInfo<T>(factory, this);
                }
            }
        }
    }
}