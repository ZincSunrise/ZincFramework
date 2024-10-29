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
                //����ʽ0x1021,��ʼֵ0xFFFF 
                XMODEM_16 = 2,
                //����ʽ0x1021����ʼֵ0x0000
                MODBUS_16 = 3,
                //����ʽ0x8005����ʼֵ0xFFFF
                Standard_32 = 4,
                //����ʽ0x04C11DB7����ʼֵ0xFFFFFFFF
                Castagnoli_32 = 5,
                //����ʽ0x1EDC6F41����ʼֵ0xFFFFFFFF
                Koopman_32 = 6,
                //����ʽ0x741B8CD7����ʼֵ0xFFFFFFFF
            }


            public abstract class Crc
            {
                public abstract int CheckCodeLength { get; }

                public abstract byte[] GenerateCode(byte[] bytes);
            }
        }
    }
}

