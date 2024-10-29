using ZincFramework.Binary.Serialization;


namespace ZincFramework.Localization.Serialization
{
    public class LocalizationConverter : BinaryConverter<LocalizationInfo>
    {
        public override LocalizationInfo Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            LocalizationInfo localizationInfo = new LocalizationInfo();
            localizationInfo.Read(ref byteReader, serializerOption);
            return localizationInfo;
        }

        public override void Write(LocalizationInfo data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            data.Write(byteWriter, serializerOption);
        }
    }
}