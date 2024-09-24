using System;



namespace ZincFramework.Binary.Serialization
{
    public abstract class BinaryConverterAttribute : Attribute
    {
        public Type ConvertType { get; set; }

        public abstract BinaryConverter GetConverter();
    }
}