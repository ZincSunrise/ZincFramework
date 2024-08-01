using System;
using UnityRandom = UnityEngine.Random;



namespace ZincFramework
{
    namespace Security
    {
        namespace Cryptography
        {
            public class SimpleRSA : ICrypto
            {
                private readonly long _encodeKey;
                private readonly long _decodeKey;
                private readonly long _n;

                public byte[] Key => BitConverter.GetBytes(_encodeKey + _decodeKey);


                public SimpleRSA()
                {
                    (_encodeKey, _decodeKey, _n) = GetRSAKeyPair();
                }

                public long Encrypt(long value)
                {
                    return MathUtility.FastPower(value, _encodeKey, _n);
                }

                public long Decrypt(long value)
                {
                    return MathUtility.FastPower(value, _decodeKey, _n);
                }

                byte[] ICrypto.Encrypt(byte[] bytes)
                {
                    throw new System.NotImplementedException();
                }

                byte[] ICrypto.Decrypt(byte[] bytes)
                {
                    throw new System.NotImplementedException();
                }

                private (long a, long b) RandomKeyValuePair()
                {
                    return (UnityRandom.Range(1024, 65536), UnityRandom.Range(1024, 65536));
                }

                private (long encodeKey, long decodeKey, long n) GetRSAKeyPair()
                {
                    (long a, long b) = RandomKeyValuePair();
                    long eularFuc;
                    long decodeKey = 0;
                    long encodeKey = 0;
                    bool isRestart = true;

                    while (isRestart)
                    {
                        encodeKey = UnityRandom.Range(0, 65536);
                        if (!MathUtility.IsPrimeNumber(encodeKey))
                        {
                            isRestart = true;
                            continue;
                        }

                        (a, b) = RandomKeyValuePair();

                        if (!MathUtility.IsPrimeNumber(a) || !MathUtility.IsPrimeNumber(b))
                        {
                            isRestart = true;
                            continue;
                        }

                        eularFuc = (a - 1) * (b - 1);

                        if (MathUtility.GetGcd(eularFuc, encodeKey) != 1)
                        {
                            isRestart = true;
                            continue;
                        }

                        if (decodeKey == 1 || decodeKey >= eularFuc)
                        {
                            isRestart = true;
                            continue;
                        }

                        isRestart = false;
                        decodeKey = MathUtility.GetInverse(encodeKey, eularFuc);
                    }

                    return (encodeKey, decodeKey, a * b);
                }
            }
        }
    }
}
