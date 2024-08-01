namespace ZincFramework
{
    namespace Security
    {
        namespace Cryptography
        {
            public interface ICrypto
            {
                byte[] Key { get; }

                byte[] Encrypt(byte[] bytes);

                byte[] Decrypt(byte[] bytes);
            }
        }
    }
}
