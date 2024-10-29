using System;
using System.Security.Cryptography;
using ZincFramework.Security.Strategy;
using UnityRandom = UnityEngine.Random;

namespace ZincFramework
{
    public static class CryptogramUtility
    {
        public static HashAlgorithm GetHashAlgorithm(E_Hash_Type hashType) => hashType switch
        {
            E_Hash_Type.SHA256 => SHA256.Create(),
            E_Hash_Type.SHA1 => SHA1.Create(),
            E_Hash_Type.SHA384 => SHA384.Create(),
            E_Hash_Type.SHA512 => SHA512.Create(),
            _ => throw new NotImplementedException()
        };

        public static Aes GetSimpleAes()
        {
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            return aes;
        }

        public static Aes GetComplexAes()
        {
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            return aes;
        }

        public static int GetRandomCode()
        {
            return UnityRandom.Range(0, 65536);
        }

        public static int SimpleEncrypt(int value, int randomCode)
        {
            value += 10;
            value ^= randomCode;
            value *= 2;
            return value;
        }

        public static int SimpleDecrypt(int value, int randomCode)
        {
            value /= 2;
            value ^= randomCode;
            value -= 114514;
            return value;
        }

        public static byte[] SimpleEncrypt(byte[] value)
        {
            for (int i = 0;i < value.Length; i++)
            {
                value[i] ^= 0b10100111;
                value[i] ^= 0b10111011;
                value[i] ^= 0b10001001;
            }
            return value;
        }

        public static byte[] SimpleDecrypt(byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                value[i] ^= 0b10100111;
                value[i] ^= 0b10111011;
                value[i] ^= 0b10001001;
            }
            return value;
        }
    }
}

