using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Binary.Serialization;


namespace ZincFramework.Serialization
{
    public struct MethodWriter
    {
        public readonly string ClassType { get; }

        public readonly int Count => _memberWriteInfos.Count;

        public List<string> MethodStates { get; }

        public readonly MemberWriteInfo Current => _memberWriteInfos.Peek();


        private Stack<MemberWriteInfo> _memberWriteInfos;

        public MethodWriter(string classType, List<string> methodStates)
        {
            ClassType = classType;
            MethodStates = methodStates;
            _memberWriteInfos = new Stack<MemberWriteInfo>();
        }

        public void PushToWriteWrite(MemberWriteInfo memberWriteInfo)
        {
            _memberWriteInfos ??= new Stack<MemberWriteInfo>();

            _memberWriteInfos.Push(memberWriteInfo);

            WriterFactory.Instance.GetWriter(memberWriteInfo).WriteWriteMethod(ref this);
            MethodStates.Add(string.Empty);

            _memberWriteInfos.Pop();
        }

        public void PushToWriteRead(MemberWriteInfo memberWriteInfo)
        {
            _memberWriteInfos ??= new Stack<MemberWriteInfo>();
            _memberWriteInfos.Push(memberWriteInfo);

            WriterFactory.Instance.GetWriter(memberWriteInfo).WriteReadMethod(ref this);
            MethodStates.Add(string.Empty);

            _memberWriteInfos.Pop();
        }


        public readonly void WritePrimitiveInList(string typeName)
        {
            string str = string.Format("byteWriter.Write{0}({1});", typeName, Current.Name);
            MethodStates.Add(new string('\t', Current.IndentSize) + str);
        }

        public readonly void ReadPrimitiveInList(string type) 
        {
            string str = $"(_setMap[code] as SetAction<{ClassType}, {Current.Type}>).Invoke(this, byteReader.Read{type}());";
            MethodStates.Add(new string('\t', Current.IndentSize) + str);
        }

        public readonly void WriteSimpleList(string str)
        {
            str = string.Format(str, Current.Name);
            MethodStates.Add(new string('\t', Current.IndentSize) + str);
        }

        public readonly void ReadSimpleInList(string methodString)
        {
            string str = $"(_setMap[code] as SetAction<{ClassType}, {Current.Type}>).Invoke(this, {methodString});";
            MethodStates.Add(new string('\t', Current.IndentSize) + str);
        }

        public readonly void WriteInList(string str)
        {
            MethodStates.Add(new string('\t', Current.IndentSize) + str);
        }

        public readonly void BeginWriteIfReference()
        {
            WriteInList(SerializeWriteUtility.WriteNullCondition(Current.Name));
            WriteInList("{");
            Current.IndentSize = Current.IndentSize + 1;
            WriteInList(SerializeWriteUtility.GetWriteCode(Current.Name));
        }

        public readonly void BeginReadIfReference()
        {
            WriteInList(SerializeWriteUtility.ReadNullCondition);
            WriteInList("{");
            Current.IndentSize = Current.IndentSize + 1;
            WriteInList(SerializeWriteUtility.GetCodeStr);
        }

        public readonly void EndWriteIfReference()
        {
            Current.IndentSize = Current.IndentSize - 1;
            WriteInList("}");
        }

        public readonly void WriteElseBlock()
        {
            WriteInList(SerializeWriteUtility.Else);
            WriteInList("{");

            Current.IndentSize = Current.IndentSize + 1;
            WriteInList(SerializeWriteUtility.WriteNullCode);
            Current.IndentSize = Current.IndentSize - 1;

            WriteInList("}");
        }

        public void WriteWriteCollection(params MemberWriteInfo[] willWrites)
        {
            var (_, name, _) = Current;

            WriteInList(SerializeWriteUtility.WriteLength(name, "Count"));
            WriteInList(SerializeWriteUtility.ForeachHead("item", name));
            WriteInList("{");

            for (var i = 0; i < willWrites.Length; i++) 
            {
                willWrites[i].IndentSize = Current.IndentSize + 1;
                PushToWriteWrite(willWrites[i]);
            }

            WriteInList("}");
        }


        public void WriteReadCollection(params MemberWriteInfo[] willWrites)
        {
            var (_, _, type) = Current;
            WriteInList(SerializeWriteUtility.ReadLength);

            string trueType = $"{type}<{string.Join(',', Current.GenericTypes)}>";

            WriteInList($"{trueType} temp = new (count);");
            WriteInList(SerializeWriteUtility.LoopHead("count"));
            WriteInList("{");

            for (var i = 0; i < willWrites.Length; i++)
            {
                willWrites[i].IndentSize = Current.IndentSize + 1;
                PushToWriteRead(willWrites[i]);
            }

            WriteInList("}");
            WriteInList($"(_setMap[code] as SetAction<{ClassType}, {trueType}>).Invoke(this, temp);");
        }



        public void WriteWriteArray()
        {
            var (indentSize, name, type) = Current;

            WriteInList(SerializeWriteUtility.WriteLength(name, "Length"));
            WriteInList(SerializeWriteUtility.ArrayLoopHead(name, "Length"));
            WriteInList("{");

            PushToWriteWrite(new MemberWriteInfo(indentSize + 1, $"{name}[i]", type));
            WriteInList("}");
        }

        public void WriteReadArray()
        {
            var (indentSize, _, type) = Current;
            WriteInList(SerializeWriteUtility.ReadLength);
            
            WriteInList($"{type}[] tempArray = count == 0 ? Array.Empty<{type}>() : new {type}[count];");
            WriteInList(SerializeWriteUtility.ArrayLoopHead("tempArray", "Length"));
            WriteInList("{");

            PushToWriteRead(new MemberWriteInfo(indentSize + 1, "tempArray[i]", type));

            WriteInList("}");
            WriteInList($"(_setMap[code] as SetAction<{ClassType}, {type}[]>).Invoke(this, tempArray);");
        }
    }
}
