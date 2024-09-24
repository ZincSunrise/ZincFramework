using System;
using System.Data;
using System.Xml;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace TypeWriter
        {
            public readonly struct MemberWriteInfo
            {
                public string Name { get; }

                public string Type { get; }

                public string OrdinalNumber { get; }

                public bool IsArray { get; }

                public bool IsEnum { get; }

                public string[] GenericTypes { get; }


                public MemberWriteInfo(DataTable dataTable, DataColumn dataColumn, bool isProperty)
                {
                    string name = dataTable.Rows[0][dataColumn].ToString();

                    Name = isProperty ? TextUtility.UpperFirstChar(name) : name;
                    Type = dataTable.Rows[1][dataColumn].ToString();

                    IsArray = Type.Contains("Array", StringComparison.OrdinalIgnoreCase);
                    IsEnum = Type.Contains("E_");

                    if (IsArray)
                    {
                        Type = Type.Replace("Array/", "", StringComparison.OrdinalIgnoreCase);
                    }

                    OrdinalNumber = dataTable.Rows[4][dataColumn].ToString();
                    GenericTypes = null;
                }

                public MemberWriteInfo(string name, string type, string ordinalNumber, bool isProperty)
                {
                    Name = isProperty ? TextUtility.UpperFirstChar(name) : name;
                    Type = type;

                    IsArray = Type.Contains("Array", StringComparison.OrdinalIgnoreCase);
                    IsEnum = Type.Contains("E_");

                    if (IsArray)
                    {
                        Type = Type.Replace("Array/", "", StringComparison.OrdinalIgnoreCase);
                    }

                    OrdinalNumber = ordinalNumber;
                    GenericTypes = null;
                }


                public MemberWriteInfo(XmlNode fieldNode, bool isEnum, bool isProperty)
                {
                    string name = fieldNode.Attributes["name"].Value;
                    Name = isProperty ? TextUtility.UpperFirstChar(name) : name;
                    Type = fieldNode.Attributes["type"].Value;


                    if (!SingleTypeWriter.IsSingleValue(Type) && Type != "string")
                    {
                        Type = TextUtility.UpperFirstChar(Type);
                    }

                    OrdinalNumber = fieldNode.Attributes["ordinalNumber"].Value;

                    IsArray = string.Compare(Type, "Array", true) == 0;
                    IsEnum = isEnum;

                    if (IsArray)
                    {
                        Type = fieldNode.Attributes["T"].Value;
                        GenericTypes = null;
                    }
                    else if (fieldNode.Attributes["T"] != null)
                    {
                        GenericTypes = new string[] { fieldNode.Attributes["T"].Value };
                    }
                    else if (fieldNode.Attributes["T1"] != null && fieldNode.Attributes["T2"] != null)
                    {
                        GenericTypes = new string[] { fieldNode.Attributes["T1"].Value, fieldNode.Attributes["T2"].Value };
                    }
                    else
                    {
                        GenericTypes = null;
                    }
                }
            }
        }
    }
}