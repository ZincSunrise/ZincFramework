using System;
using System.Text;
using System.Runtime.InteropServices;
using ZincFramework.Serialization;



namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public class ArrayWriter<T> : IEnumerableWriter<T[], T>
    {
        public override bool CanWrite(string typeStr) => (typeStr.Contains("Array", StringComparison.CurrentCultureIgnoreCase) || typeStr.Contains("[]")) && typeStr.Contains(typeof(T).Name, StringComparison.CurrentCultureIgnoreCase);

        private readonly static char[] _splitCharacters = { '/' , '|'};

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public override void WriteBinary(string str, Serialization.ByteWriter byteWriter, SerializerOption serializerOption)
        {
            if (str == string.Empty)
            {
                byteWriter.WriteInt32(0);
                return;
            }

            string[] elements = str.Split(_splitCharacters);
            Type elementType = typeof(T);

            if (elementType.IsPrimitive || WriterFactory.IsBittable(elementType))
            {
                byteWriter.WriteInt32(elements.Length);
                bool pre = byteWriter.WriterOption.IsUsingVariant;
                byteWriter.WriterOption.IsUsingVariant = false;

                for (int i = 0; i < elements.Length; i++)
                {
                    WriterFactory.Instance.GetWriter(elementType.Name).WriteBinary(elements[i], byteWriter, serializerOption);
                }

                byteWriter.WriterOption.IsUsingVariant = pre;
            }
            else
            {
                byteWriter.WriteInt32(elements.Length);
                for (int i = 0; i < elements.Length; i++)
                {
                    WriterFactory.Instance.GetWriter(elementType.Name).WriteBinary(elements[i], byteWriter, serializerOption);
                }
            }
        }

        public override string GetExcelString(object obj)
        {
            _stringBuilder.Clear();
            Array array = (Array)obj;
            Type elementType = typeof(T);

            for (int i = 0; i < array.Length; i++) 
            {
                string str = WriterFactory.Instance.GetWriter(elementType.Name).GetExcelString(array.GetValue(i));
                if (i != array.Length - 1)
                {
                    _stringBuilder.Append(str + '/');
                }
                else
                {
                    _stringBuilder.Append(str);
                }
            }

            return _stringBuilder.ToString();
        }

        public override void WriteWriteMethod(ref MethodWriter methodWriter)
        {
            base.WriteWriteMethod(ref methodWriter);
            Type elementType = typeof(T);

            if (WriterFactory.IsSimpleValue(methodWriter.Current.Type) || WriterFactory.IsBittable(elementType))
            {
                methodWriter.WriteInList($"byteWriter.WriteArray({methodWriter.Current.Name});");
            }
            else
            {
                methodWriter.WriteWriteArray();
            }

            methodWriter.EndWriteIfReference();
            methodWriter.WriteElseBlock();        
        }

        public override void WriteReadMethod(ref MethodWriter methodWriter)
        {
            #region ะดสýื้
            base.WriteReadMethod(ref methodWriter);
            Type elementType = typeof(T);

            if (WriterFactory.IsSimpleValue(methodWriter.Current.Type) || WriterFactory.IsBittable(elementType))
            {
                methodWriter.WriteInList($"(_setMap[code] as SetAction<{methodWriter.ClassType}, {elementType.FullName}[]>).Invoke(this, byteReader.ReadArray<{elementType.FullName}>());");
            }
            else
            {
                methodWriter.WriteReadArray();
            }
            #endregion

            methodWriter.EndWriteIfReference();
        }
    }
}
