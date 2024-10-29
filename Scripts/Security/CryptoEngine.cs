using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using ZincFramework.Binary;
using ZincFramework.Events;
using ZincFramework.LoadServices;
using ZincFramework.Security.Strategy;


namespace ZincFramework
{
    namespace Security
    {
        //���ܹ�����
        public static class CryptoEngine
        {
            /// <summary>
            /// ����ʹ��ʹ��Crc��Standard_32ģʽ, ��ϣ�㷨��SHA-256
            /// </summary>
            /// <param name="data"></param>
            /// <param name="isUseCrc"></param>
            /// <returns></returns>
            public static byte[] Encrypt(byte[] data, E_CheckCode_Type checkCodeType)
            {
                using (Aes aes = CryptogramUtility.GetSimpleAes())
                {
                    aes.Key = DecryptAesKey();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();
                            cryptoStream.Close();


                            byte[] encryptedDatas = memoryStream.ToArray();

                            ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                            byte[] checkCode = cryptoStrategy.GetCheckCode(encryptedDatas);

                            byte[] combinedData = new byte[encryptedDatas.Length + checkCode.Length];

                            Buffer.BlockCopy(encryptedDatas, 0, combinedData, 0, encryptedDatas.Length);
                            Buffer.BlockCopy(checkCode, 0, combinedData, encryptedDatas.Length, checkCode.Length);
                            return combinedData;
                        }
                    }
                }
            }

            public static byte[] Decrypt(byte[] encryptedData, E_CheckCode_Type checkCodeType, ZincAction verifyDefeatAction = null)
            {
                using (Aes aes = CryptogramUtility.GetSimpleAes())
                {
                    aes.Key = DecryptAesKey();

                    ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                    ArraySegment<byte> checkCode = cryptoStrategy.GetLastCheckCode(encryptedData);

                    ArraySegment<byte> encryptData = new ArraySegment<byte>(encryptedData, 0, encryptedData.Length - checkCode.Count);
                    if (!cryptoStrategy.VerifyData(encryptData.ToArray(), checkCode.ToArray()))
                    {
                        verifyDefeatAction?.Invoke();
                        LogUtility.LogError("���ݱ��۸�");
                        //throw new ArgumentException("���ݱ��۸�");
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ICryptoTransform cryptoTransform = aes.CreateDecryptor();


                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            // ����������д�� CryptoStream�������Զ����ܲ�д�� MemoryStream  
                            cryptoStream.Write(encryptData);
                            cryptoStream.FlushFinalBlock(); // ���ô˷�����ȷ���������ݶ�������ˢ�µ��ײ���  
                        }

                        // �� MemoryStream ��ȡ���ܺ������  
                        byte[] decryptData = memoryStream.ToArray();
                        return decryptData;
                    }
                }
            }


            /// <summary>
            /// ����ʹ��ʹ��Crc��Standard_32ģʽ, ��ϣ�㷨��SHA-256
            /// </summary>
            /// <param name="data"></param>
            /// <param name="isUseCrc"></param>
            /// <returns></returns>
            public static byte[] EncryptComplex(byte[] data, E_CheckCode_Type checkCodeType)
            {
                using (Aes aes = CryptogramUtility.GetComplexAes())
                {
                    aes.Key = DecryptAesKey();
                    aes.GenerateIV();

                    ICryptoTransform cryptoTransform = aes.CreateEncryptor();

                    using (MemoryStream memoryStream = new MemoryStream())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();

                        byte[] encryptedDatas = memoryStream.ToArray();

                        ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                        byte[] checkCode = cryptoStrategy.GetCheckCode(encryptedDatas);

                        byte[] combinedData = new byte[encryptedDatas.Length + aes.IV.Length + checkCode.Length];

                        int offset = 0;

                        Buffer.BlockCopy(encryptedDatas, 0, combinedData, offset, encryptedDatas.Length);
                        offset += encryptedDatas.Length;

                        Buffer.BlockCopy(aes.IV, 0, combinedData, offset, aes.IV.Length);
                        offset += aes.IV.Length;

                        Buffer.BlockCopy(checkCode, 0, combinedData, offset, checkCode.Length);
                        return combinedData;
                    }
                }
            }


            public static byte[] DecryptComplex(byte[] encryptedData, E_CheckCode_Type checkCodeType, ZincAction verifyDefeatAction = null)
            {
                using (Aes aes = CryptogramUtility.GetComplexAes())
                {
                    aes.Key = DecryptAesKey();

                    ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                    ArraySegment<byte> checkCode = cryptoStrategy.GetLastCheckCode(encryptedData);

                    ArraySegment<byte> IV = new ArraySegment<byte>(encryptedData, encryptedData.Length - checkCode.Count - 16, 16);

                    ArraySegment<byte> encryptData = new ArraySegment<byte>(encryptedData, 0, encryptedData.Length - checkCode.Count - 16);

                    if (!cryptoStrategy.VerifyData(encryptData.ToArray(), checkCode.ToArray()))
                    {
                        verifyDefeatAction?.Invoke();
                        LogUtility.LogError("���ݱ��۸�");
                        //throw new ArgumentException("���ݱ��۸�");
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        aes.IV = IV.ToArray();
                        ICryptoTransform cryptoTransform = aes.CreateDecryptor();
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            // ����������д�� CryptoStream�������Զ����ܲ�д�� MemoryStream  
                            cryptoStream.Write(encryptData);
                            cryptoStream.FlushFinalBlock(); // ���ô˷�����ȷ���������ݶ�������ˢ�µ��ײ���  
                        }

                        // �� MemoryStream ��ȡ���ܺ������  
                        byte[] decryptData = memoryStream.ToArray();
                        return decryptData;
                    }
                }
            }


            public static async void EncryptAsync(byte[] data, ZincAction<byte[]> callback, E_CheckCode_Type checkCodeType)
            {
                using (Aes aes = CryptogramUtility.GetSimpleAes())
                {
                    aes.Key = DecryptAesKey();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            await cryptoStream.WriteAsync(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();
                            cryptoStream.Close();

                            byte[] encryptedDatas = memoryStream.ToArray();
                            ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                            ArraySegment<byte> checkCode = cryptoStrategy.GetLastCheckCode(encryptedDatas);

                            byte[] combinedData = new byte[encryptedDatas.Length + checkCode.Count];

                            Buffer.BlockCopy(encryptedDatas, 0, combinedData, 0, encryptedDatas.Length);
                            Buffer.BlockCopy(checkCode.ToArray(), 0, combinedData, encryptedDatas.Length, checkCode.Count);

                            callback?.Invoke(combinedData);
                        }
                    }
                }
            }

            public static async void DecryptAsync(byte[] encryptedData, ZincAction<byte[]> callback, E_CheckCode_Type checkCodeType, ZincAction verifyDefeatAction = null)
            {
                using (Aes aes = CryptogramUtility.GetSimpleAes())
                {
                    aes.Key = DecryptAesKey();

                    ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                    ArraySegment<byte> checkCode = cryptoStrategy.GetLastCheckCode(encryptedData);
                    ArraySegment<byte> encryptData = new ArraySegment<byte>(encryptedData, 0, encryptedData.Length - checkCode.Count);

                    if (!cryptoStrategy.VerifyData(encryptData.ToArray(), checkCode.ToArray()))
                    {
                        verifyDefeatAction?.Invoke();
                        LogUtility.LogError("���ݱ��۸�");
                        //throw new ArgumentException("���ݱ��۸�");
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ICryptoTransform cryptoTransform = aes.CreateDecryptor();
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            // ����������д�� CryptoStream�������Զ����ܲ�д�� MemoryStream  
                            await cryptoStream.WriteAsync(encryptData);
                            cryptoStream.FlushFinalBlock(); // ���ô˷�����ȷ���������ݶ�������ˢ�µ��ײ���  
                        }

                        // �� MemoryStream ��ȡ���ܺ������  
                        byte[] decryptData = memoryStream.ToArray();
                    }
                }
            }


            /// <summary>
            /// ����ʹ��ʹ��Crc��Standard_32ģʽ, ��ϣ�㷨��SHA-256
            /// </summary>
            /// <param name="data"></param>
            /// <param name="isUseCrc"></param>
            /// <returns></returns>
            public static async void EncryptComplexAsync(byte[] data, E_CheckCode_Type checkCodeType)
            {
                using (Aes aes = CryptogramUtility.GetComplexAes())
                {
                    aes.Key = DecryptAesKey();
                    aes.GenerateIV();

                    ICryptoTransform cryptoTransform = aes.CreateEncryptor();

                    using (MemoryStream memoryStream = new MemoryStream())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        await cryptoStream.WriteAsync(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();

                        byte[] encryptedDatas = memoryStream.ToArray();

                        ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                        ArraySegment<byte> checkCode = cryptoStrategy.GetCheckCode(encryptedDatas);

                        byte[] combinedData = new byte[encryptedDatas.Length + aes.IV.Length + checkCode.Count];

                        int offset = 0;

                        Buffer.BlockCopy(encryptedDatas, 0, combinedData, offset, encryptedDatas.Length);
                        offset += encryptedDatas.Length;

                        Buffer.BlockCopy(aes.IV, 0, combinedData, offset, aes.IV.Length);
                        offset += aes.IV.Length;

                        Buffer.BlockCopy(checkCode.ToArray(), 0, combinedData, offset, checkCode.Count);
                    }
                }
            }

            public static async void DecryptComplexAsync(byte[] encryptedData, E_CheckCode_Type checkCodeType, ZincAction verifyDefeatAction = null)
            {
                using (Aes aes = CryptogramUtility.GetComplexAes())
                {
                    aes.Key = DecryptAesKey();

                    ICryptoStrategy cryptoStrategy = ICryptoStrategy.Create(checkCodeType);
                    ArraySegment<byte> checkCode = cryptoStrategy.GetLastCheckCode(encryptedData);

                    ArraySegment<byte> IV = new ArraySegment<byte>(encryptedData, encryptedData.Length - checkCode.Count - 16, 16);

                    ArraySegment<byte> encryptData = new ArraySegment<byte>(encryptedData, 0, encryptedData.Length - checkCode.Count - 16);

                    if (!cryptoStrategy.VerifyData(encryptData.ToArray(), checkCode.ToArray()))
                    {
                        verifyDefeatAction?.Invoke();
                        LogUtility.LogError("���ݱ��۸�");
                        //throw new ArgumentException("���ݱ��۸�");
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        aes.IV = IV.ToArray();
                        ICryptoTransform cryptoTransform = aes.CreateDecryptor();
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            // ����������д�� CryptoStream�������Զ����ܲ�д�� MemoryStream  
                            await cryptoStream.WriteAsync(encryptData);
                            cryptoStream.FlushFinalBlock(); // ���ô˷�����ȷ���������ݶ�������ˢ�µ��ײ���  
                        }

                        // �� MemoryStream ��ȡ���ܺ������  
                        byte[] decryptData = memoryStream.ToArray();
                    }
                }
            }

            private static byte[] DecryptAesKey()
            {
                byte[] decryptedKey;
                using (RSA rsa = RSA.Create())
                {
                    TextAsset textAsset = ResourcesManager.Instance.Load<TextAsset>("Security/RSAKeyCode");
                    byte[] encryptedRSAKey = textAsset.bytes;

                    using (FileStream fileStream = File.OpenRead(Path.Combine(FrameworkPaths.SavePath, "SaveCore" + BinaryDataManager.Extension)))
                    {
                        byte[] encryptKey = new byte[fileStream.Length];
                        fileStream.Read(encryptKey, 0, encryptKey.Length);

                        encryptedRSAKey = CryptogramUtility.SimpleDecrypt(encryptedRSAKey);
                        rsa.FromXmlString(Encoding.UTF8.GetString(encryptedRSAKey));
                        decryptedKey = rsa.Decrypt(encryptKey, RSAEncryptionPadding.OaepSHA1);
                    }
                }

                return decryptedKey;
            }
        }
    }
}

