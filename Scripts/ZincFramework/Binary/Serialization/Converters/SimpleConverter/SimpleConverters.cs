using System;

namespace ZincFramework.Binary.Serialization.Converters
{
    public static class SimpleConverters
    {
        public static BinaryConverter<byte> ByteConverter => _byteConverter ??= new ByteConverter();

        private static BinaryConverter<byte> _byteConverter;

        public static BinaryConverter<sbyte> SByteConverter => _SByte16Converter ??= new SByteConverter();

        private static BinaryConverter<sbyte> _SByte16Converter;

        public static BinaryConverter<short> Int16Converter => _int16Converter ??= new Int16Converter();

        private static BinaryConverter<short> _int16Converter;

        public static BinaryConverter<ushort> UInt16Converter => _uInt16Converter ??= new UInt16Converter();

        private static BinaryConverter<ushort> _uInt16Converter;

        public static BinaryConverter<char> CharConverter => _charConverter ??= new CharConverter();

        private static BinaryConverter<char> _charConverter;

        public static BinaryConverter<bool> BooleanConverter => _boolConverter ??= new BooleanConverter();

        private static BinaryConverter<bool> _boolConverter;

        public static BinaryConverter<int> Int32Converter => _int32Converter ??= new Int32Converter();

        private static BinaryConverter<int> _int32Converter;

        public static BinaryConverter<uint> UInt32Converter => _uInt32Converter ??= new UInt32Converter();

        private static BinaryConverter<uint> _uInt32Converter;

        public static BinaryConverter<float> SingleConverter => _singleConverter ??= new SingleConverter();

        private static BinaryConverter<float> _singleConverter;

        public static BinaryConverter<long> Int64Converter => _int64Converter ??= new Int64Converter();

        private static BinaryConverter<long> _int64Converter;

        public static BinaryConverter<ulong> UInt64Converter => _uInt64Converter ??= new UInt64Converter();

        private static BinaryConverter<ulong> _uInt64Converter;

        public static BinaryConverter<double> DoubleConverter => _doubleConverter ??= new DoubleConverter();

        private static BinaryConverter<double> _doubleConverter;

        public static BinaryConverter<DateTime> DateTimeConverter => _dateTimeConverter ??= new DateTimeConverter();

        private static BinaryConverter<DateTime> _dateTimeConverter;

        public static BinaryConverter<TimeSpan> TimeSpanConverter => _timeSpanConverter ??= new TimeSpanConverter();

        private static BinaryConverter<TimeSpan> _timeSpanConverter;

        public static BinaryConverter<Uri> UriConverter => _uriConverter ??= new UriConverter();

        private static BinaryConverter<Uri> _uriConverter;

        public static BinaryConverter<string> StringConverter => _stringConverter ??= new StringConverter();

        private static BinaryConverter<string> _stringConverter;
    }
}

