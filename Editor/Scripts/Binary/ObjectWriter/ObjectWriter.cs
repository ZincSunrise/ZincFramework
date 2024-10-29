using System;
using System.Reflection;
using System.Text;
using ZincFramework.Serialization;


namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class ObjectWriter<T> : ReferenceWriter<T>
    {
        private readonly string _type;

        private PropertyInfo[] _properties;

        public ObjectWriter(string type) 
        {
            _type = type;
            Type objType = Type.GetType(_type);
            if(type == null)
            {
                throw new ArgumentException($"没有对应{_type}的类，请写出全名");
            }

            _properties = objType.GetProperties();
        }

        public override bool CanWrite(string typeStr) => typeStr == _type;

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            string[] strs = str.Split('/');
            for(int i = 0; i < _properties.Length; i++)
            {
                try
                {
                    WriterFactory.Instance.GetWriter(_properties[i].PropertyType.Name).WriteBinary(strs[i], byteWriter, serializerOption);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException($"类型数目{_properties.Length}和数据长度{strs.Length}不一样");
                }
                catch 
                {
                    throw new ArgumentException($"类型{_properties[i].PropertyType.Name}不匹配字符串{strs[i]}");
                }
            }
        }

        public override string GetExcelString(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _properties.Length; i++)
            {
                string str = WriterFactory.Instance.GetWriter(_properties[i].PropertyType.Name).GetExcelString(_properties[i].GetValue(obj));
                if (i != _properties.Length - 1)
                {
                    stringBuilder.Append(str + '/');
                }
                else
                {
                    stringBuilder.Append(str);
                }
            }

            return stringBuilder.ToString();
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            base.WriteWriteMethod(ref methodWriter);

            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.WritePrimitiveInList("{0}.Write(byteWriter, serializerOption)");

                methodWriter.EndWriteIfReference();
                methodWriter.WriteElseBlock();
            }
            else
            {
                methodWriter.WritePrimitiveInList("{0}.Write(byteWriter, serializerOption)");
            }
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            base.WriteWriteMethod(ref methodWriter);

            if (methodWriter.Current.IsWriteMember)
            {
                methodWriter.WriteInList($"{methodWriter.Current.Type} data = new ();");
                methodWriter.WriteInList("data.Read(ref byteReader, serializerOption);");
                methodWriter.ReadPrimitiveInList("data");

                methodWriter.EndWriteIfReference();
                methodWriter.WriteElseBlock();
            }
            else
            {
                methodWriter.WriteInList($"{methodWriter.Current.Name} = new ();");
                methodWriter.WriteInList($"{methodWriter.Current.Name}.Read(ref byteReader, serializerOption);");
            }
        }
    }
}
