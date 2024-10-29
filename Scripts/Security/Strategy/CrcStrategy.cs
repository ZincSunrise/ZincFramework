using ZincFramework.Security.CheckCode;


namespace ZincFramework
{
    namespace Security
    {
        namespace Strategy
        {
            public readonly struct CrcStrategy : ICryptoStrategy
            {
                private readonly Crc _crc;

                public int CheckCodeLength => _crc.CheckCodeLength;

                public CrcStrategy(int crcType)
                {
                    _crc = CrcFactory.Create((E_Crc_Type)crcType);
                }

                public byte[] GetCheckCode(byte[] bytes)
                {
                    return _crc.GenerateCode(bytes);
                }


                public bool VerifyData(byte[] verifyingData, byte[] checkcode)
                {
                    return ArrayListUtility.Compare(_crc.GenerateCode(verifyingData), checkcode);
                }
            }
        }
    }
}

