namespace ZincFramework
{
    namespace Security
    {
        public enum E_CheckCode_Type
        {
            Crc_CCITT_16 = 1,
            //����ʽ0x1021,��ʼֵ0xFFFF 
            Crc_XMODEM_16 = 2,
            //����ʽ0x1021����ʼֵ0x0000
            Crc_MODBUS_16 = 3,
            //����ʽ0x8005����ʼֵ0xFFFF
            Crc_Standard_32 = 4,
            //����ʽ0x04C11DB7����ʼֵ0xFFFFFFFF
            Crc_Castagnoli_32 = 5,
            //����ʽ0x1EDC6F41����ʼֵ0xFFFFFFFF
            Crc_Koopman_32 = 6,

            Hash_SHA1 = 0xA1,
            Hash_SHA256 = 256,
            Hash_SHA384 = 384,
            Hash_SHA512 = 512
        }
    }
}