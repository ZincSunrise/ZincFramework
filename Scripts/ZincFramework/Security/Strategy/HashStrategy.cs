using System.Security.Cryptography;



namespace ZincFramework
{
    namespace Security
    {
        namespace Strategy
        {
            public enum E_Hash_Type
            {
                SHA1 = 0xA1,
                SHA256 = 256,
                SHA384 = 384,
                SHA512 = 512,
            }

            public readonly struct HashStrategy : ICryptoStrategy
            {
                private readonly HashAlgorithm _algorithm;

                public HashStrategy(int hashType)
                {
                    switch ((E_Hash_Type)hashType)
                    {
                        case E_Hash_Type.SHA1:
                            _algorithm = SHA1.Create();
                            break;
                        case E_Hash_Type.SHA256:
                            _algorithm = SHA256.Create();
                            break;
                        case E_Hash_Type.SHA384:
                            _algorithm = SHA384.Create();
                            break;
                        case E_Hash_Type.SHA512:
                            _algorithm = SHA512.Create();
                            break;
                        default:
                            _algorithm = SHA256.Create();
                            break;
                    }
                }

                public int CheckCodeLength => _algorithm.HashSize / 8;

                public byte[] GetCheckCode(byte[] bytes)
                {
                    return _algorithm.ComputeHash(bytes);
                }

                public bool VerifyData(byte[] verifyingData, byte[] checkcode)
                {
                    return ArrayListUtility.Compare(_algorithm.ComputeHash(verifyingData), checkcode);
                }
            }
        }
    }
}
