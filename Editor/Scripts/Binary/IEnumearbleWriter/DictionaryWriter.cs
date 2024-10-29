using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using ZincFramework.Serialization;


namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class DictionaryWriter<Tkey, TValue> : ReferenceWriter<Dictionary<Tkey, TValue>>
    {
        public StrProfileWriter KeyWriter 
        { 
            get => _keyWriter ??= WriterFactory.Instance.GetWriter(typeof(Tkey).Name);
            set => _keyWriter = value; 
        }

        public StrProfileWriter ValueWriter
        {
            get => _valueWriter ??= WriterFactory.Instance.GetWriter(typeof(TValue).Name);
            set => _valueWriter = value;
        }

        private StrProfileWriter _keyWriter;

        private StrProfileWriter _valueWriter;

        public static DictionaryWriter<Tkey, TValue> CreateDicWriter(string typeStr)
        {
            string[] strings = typeStr.Split('/');
            Contract.Assert(strings.Length > 2, $"{typeStr}必须能够分为三个类型才能成为字典");
            return new DictionaryWriter<Tkey, TValue>(strings[1], strings[2]);
        }

        private readonly string _key;
        private readonly string _value;

        private DictionaryWriter(string key, string value)
        {
            _key = key;
            _value = value;
        }

        public override bool CanWrite(string typeStr) => typeStr.Contains("Dictionary", StringComparison.OrdinalIgnoreCase) && typeStr.Contains(_key) && typeStr.Contains(_value);

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            string[] values = str.Split('/');
            byteWriter.WriteInt32(values.Length);
            foreach (string value in values) 
            {
                ReadOnlySpan<char> span = value;
                int index = span.IndexOf(',');
                WriterFactory.Instance.GetWriter(_key).WriteBinary(new string(span[..index].Trim()), byteWriter, serializerOption);
                WriterFactory.Instance.GetWriter(_value).WriteBinary(new string(span[index..].Trim()), byteWriter, serializerOption);
            }
        }

        public override string GetExcelString(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            IDictionary dictionary = obj as IDictionary;

            int index = 0;
            foreach(var key in dictionary.Keys)
            {
                object value = dictionary[key];
                string keyString = WriterFactory.Instance.GetWriter(_key).GetExcelString(key);
                string valueString = WriterFactory.Instance.GetWriter(_value).GetExcelString(value);

                if (index == dictionary.Count - 1)
                {
                    stringBuilder.Append(keyString + ',' + valueString);
                }
                else
                {
                    stringBuilder.Append(keyString + ',' + valueString + '/');
                }
                index++;
            }

            return stringBuilder.ToString();
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            base.WriteReadMethod(ref methodWriter);
            methodWriter.WriteWriteCollection(new MemberWriteInfo(methodWriter.Current.IndentSize, "item.Key", _key),
                new MemberWriteInfo(methodWriter.Current.IndentSize, "item.Value", _value));
            methodWriter.EndWriteIfReference();

            methodWriter.WriteElseBlock();
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            base.WriteReadMethod(ref methodWriter);
            methodWriter.WriteReadCollection(new MemberWriteInfo(methodWriter.Current.IndentSize, "item.Key", _key),
                 new MemberWriteInfo(methodWriter.Current.IndentSize, "item.Value", _value));
            methodWriter.EndWriteIfReference();
        }
    }
}