using System;
using System.Collections.Generic;
using ZincFramework.Security.CheckCode;

namespace ZincFramework
{
    namespace Security
    {
        namespace Strategy
        {
            public interface ICryptoStrategy
            {
                private readonly static List<Func<int, ICryptoStrategy>> _strategyFuncs = new List<Func<int, ICryptoStrategy>>();

                static ICryptoStrategy()
                {
                    _strategyFuncs.Add(new Func<int, ICryptoStrategy>(x => new CrcStrategy(x)));
                    _strategyFuncs.Add(new Func<int, ICryptoStrategy>(x => new HashStrategy(x)));
                }

                int CheckCodeLength { get; }

                bool VerifyData(byte[] verifyingData, byte[] checkcode);

                byte[] GetCheckCode(byte[] bytes);


                public ReadOnlySpan<byte> GetLastCheckCodeSpan(byte[] encryptedDatas)
                {
                    return new ReadOnlySpan<byte>(encryptedDatas, encryptedDatas.Length - CheckCodeLength, CheckCodeLength);
                }

                public ArraySegment<byte> GetLastCheckCode(byte[] encryptedDatas)
                {
                    return new ArraySegment<byte>(encryptedDatas, encryptedDatas.Length - CheckCodeLength, CheckCodeLength);
                }


                public static ICryptoStrategy Create(E_CheckCode_Type checkCodeType) => checkCodeType switch
                {
                    E_CheckCode_Type.Crc_CCITT_16 => _strategyFuncs[0].Invoke((int)E_Crc_Type.CCITT_16),
                    E_CheckCode_Type.Crc_XMODEM_16 => _strategyFuncs[0].Invoke((int)E_Crc_Type.XMODEM_16),
                    E_CheckCode_Type.Crc_MODBUS_16 => _strategyFuncs[0].Invoke((int)E_Crc_Type.MODBUS_16),
                    E_CheckCode_Type.Crc_Standard_32 => _strategyFuncs[0].Invoke((int)E_Crc_Type.Standard_32),
                    E_CheckCode_Type.Crc_Castagnoli_32 => _strategyFuncs[0].Invoke((int)E_Crc_Type.Castagnoli_32),
                    E_CheckCode_Type.Crc_Koopman_32 => _strategyFuncs[0].Invoke((int)E_Crc_Type.Koopman_32),
                    E_CheckCode_Type.Hash_SHA1 => _strategyFuncs[1].Invoke((int)E_Hash_Type.SHA1),
                    E_CheckCode_Type.Hash_SHA256 => _strategyFuncs[1].Invoke((int)E_Hash_Type.SHA256),
                    E_CheckCode_Type.Hash_SHA384 => _strategyFuncs[1].Invoke((int)E_Hash_Type.SHA384),
                    E_CheckCode_Type.Hash_SHA512 => _strategyFuncs[1].Invoke((int)E_Hash_Type.SHA512),
                    _ => _strategyFuncs[1].Invoke((int)E_Hash_Type.SHA256),
                };
            }
        }
    }
}
