using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using ZincFramework.Binary;
using ZincFramework.Security.Cryptography;



namespace ZincFramework
{
    namespace Security
    {
        public static class CryptogramTool
        {
            [MenuItem("GameTool/Cryptogram/CreateRSAKey")]
            private static void CreateRSAKey()
            {
                RSA rsa = RSA.Create();
                rsa.KeySize = 2048;
                
                string xmlString = rsa.ToXmlString(true);

                byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
                ICrypto crypto = new SimpleCrypto();
                crypto.Encrypt(bytes);

                File.WriteAllBytes(Application.dataPath + "/Scripts/MyFrameWork/Resources/Security/RSAKeyCode.xml", bytes);
                AssetDatabase.Refresh();
            }


            [MenuItem("GameTool/Cryptogram/CreateAesKey")]
            private static void CreateAesKey()
            {
                using (RSA rSA = RSA.Create())
                {
                    string path = Application.dataPath + "/Scripts/MyFrameWork/Resources/Security/RSAKeyCode.xml";

                    if (!File.Exists(path))
                    {
                        CreateRSAKey();
                    }

                    byte[] encryptRSAKey = File.ReadAllBytes(path);
                    ICrypto crypto = new SimpleCrypto();
                    crypto.Decrypt(encryptRSAKey);

                    string xmlString = Encoding.UTF8.GetString(encryptRSAKey);
                    rSA.FromXmlString(xmlString);

                    using (Aes aes = Aes.Create())
                    {
                        aes.GenerateKey();
                        byte[] aesKey = aes.Key;
                        byte[] encryptKey = rSA.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA1);

                        LogUtility.Log(Path.Combine(FrameworkPaths.SavePath, "SaveCore" + BinaryDataManager.Extension));
                        File.WriteAllBytes(Path.Combine(FrameworkPaths.SavePath, "SaveCore" + BinaryDataManager.Extension), encryptKey);
                    }
                }   
            }
        }
    }
}
