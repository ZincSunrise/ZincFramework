using System.Collections.Generic;

namespace ZincFramework
{
    namespace Security
    {
        namespace CheckCode
        {
            public enum E_Crc_Type
            {
                CCITT_16 = 1,
                //多项式0x1021,初始值0xFFFF 
                XMODEM_16 = 2,
                //多项式0x1021，初始值0x0000
                MODBUS_16 = 3,
                //多项式0x8005，初始值0xFFFF
                Standard_32 = 4,
                //多项式0x04C11DB7，初始值0xFFFFFFFF
                Castagnoli_32 = 5,
                //多项式0x1EDC6F41，初始值0xFFFFFFFF
                Koopman_32 = 6,
                //多项式0x741B8CD7，初始值0xFFFFFFFF
            }


            public abstract class Crc
            {
                public abstract int CheckCodeLength { get; }

                public abstract byte[] GenerateCode(byte[] bytes);
            }
        }
    }
}

