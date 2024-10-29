namespace ZincFramework
{
    namespace Security
    {
        public enum E_CheckCode_Type
        {
            Crc_CCITT_16 = 1,
            //多项式0x1021,初始值0xFFFF 
            Crc_XMODEM_16 = 2,
            //多项式0x1021，初始值0x0000
            Crc_MODBUS_16 = 3,
            //多项式0x8005，初始值0xFFFF
            Crc_Standard_32 = 4,
            //多项式0x04C11DB7，初始值0xFFFFFFFF
            Crc_Castagnoli_32 = 5,
            //多项式0x1EDC6F41，初始值0xFFFFFFFF
            Crc_Koopman_32 = 6,

            Hash_SHA1 = 0xA1,
            Hash_SHA256 = 256,
            Hash_SHA384 = 384,
            Hash_SHA512 = 512
        }
    }
}