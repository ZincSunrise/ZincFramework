using System.Collections;
using System.Collections.Generic;



namespace ZincFramework
{
    namespace Security
    {
        namespace Cryptography
        {
            public struct SimpleCrypto : ICrypto
            {
                public byte[] Key => _key;

                private byte[] _key;

                public byte[] Decrypt(byte[] bytes)
                {
                    return _key;
                }

                public byte[] Encrypt(byte[] bytes)
                {
                    return _key;
                }

                public void GetRandomKey()
                {
                    _key = new byte[32];
                }

                public void SetKey(byte[] bytes) 
                {
                    _key = bytes;
                }
            }
        }
    }
}

