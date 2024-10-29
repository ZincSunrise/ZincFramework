using System;
using System.Collections.Generic;
using ZincFramework.Serialization;
using ZincFramework.Binary.Serialization.ProfileWriters;



namespace ZincFramework.Binary.Serialization
{
    public partial class WriterFactory : BaseSafeSingleton<WriterFactory>
    {
        private readonly List<StrProfileWriter> _otherWriters = new List<StrProfileWriter>();

        private WriterFactory() 
        {
            InitialDictionary();
            AddCustomWriter();
        }

        public StrProfileWriter GetWriter(string typeString)
        {
            if(!_baseProfileWriters.TryGetValue(typeString, out var writer))
            {
                writer = _otherWriters.Find(x => x.CanWrite(typeString));

                if (writer == null)
                {
                    if(typeString.Contains("Array", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] typeStrs = typeString.Split('/');
                        string elementType = string.Compare(typeStrs[0], "Array", true) == 0 ? typeStrs[1] : typeStrs[0];
                        writer = CreateArrayWriter(elementType);

                        _otherWriters.Add(writer);
                    }
                    else if (typeString.Contains("[]"))
                    {
                        int index = typeString.IndexOf("[]");
                        writer = CreateArrayWriter(typeString[..index]);

                        _otherWriters.Add(writer);
                    }
                    else if (typeString.Contains("E_"))
                    {
                        writer = new EnumWriter(typeString);
                        _otherWriters.Add(writer);
                    }                   
                    else
                    {
                        foreach (var baseWriter in _baseProfileWriters.Values)
                        {
                            if (baseWriter.CanWrite(typeString))
                            {
                                writer = baseWriter;
                            }
                        }
                    }
                }
            }

            return writer ?? throw new NotSupportedException($"不支持该{typeString}类");
        }

        public StrProfileWriter GetWriter(MemberWriteInfo memberWriteInfo)
        {
            StrProfileWriter strProfileWriter;
            if (memberWriteInfo.IsArray)
            {
                strProfileWriter = CreateArrayWriter(memberWriteInfo.Type);
                _otherWriters.Add(strProfileWriter);
            }
            else if (memberWriteInfo.IsEnum)
            {
                strProfileWriter = new EnumWriter(memberWriteInfo.Type);
                _otherWriters.Add(strProfileWriter);
            }
            else if (!_baseProfileWriters.TryGetValue(memberWriteInfo.Type, out strProfileWriter))
            {
                strProfileWriter = _otherWriters.Find(x => x.CanWrite(memberWriteInfo.Type));
                if (strProfileWriter == null)
                {
                    foreach (var baseWriter in _baseProfileWriters.Values)
                    {
                        if (baseWriter.CanWrite(memberWriteInfo.Type))
                        {
                            strProfileWriter = baseWriter;
                        }
                    }
                }
            }

            return strProfileWriter ?? throw new NotSupportedException($"不支持该{memberWriteInfo.Type}类");
        }

        private StrProfileWriter CreateArrayWriter(string type)
        {
            Type elementType = GetWriter(type).GetWriteType();
            Type writerType = typeof(ArrayWriter<>).MakeGenericType(elementType);
            return Activator.CreateInstance(writerType) as StrProfileWriter;
        }
    }
}
