using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using ZincFramework.Events;
using ZincFramework.Binary.Serialization;
using ZincFramework.Serialization;




namespace ZincFramework
{
    namespace Load
    {
        public class UnityWebManager : BaseSafeSingleton<UnityWebManager> 
        {
            private readonly string _defaultURL = "http://127.0.0.1:11451";
            private UnityWebManager()
            {

            }

            public void GetAssetAsync<T>(string uri, ZincAction<T> callback) where T : class
            {
                MonoManager.Instance.StartCoroutine(R_GetAssetAsync<T>(new Uri(uri ?? _defaultURL), callback));
            }

            public void GetAssetAsync<T>(Uri uri, ZincAction<T> callback) where T : class
            {
                MonoManager.Instance.StartCoroutine(R_GetAssetAsync<T>(uri, callback));
            }
            

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T">如果是FileInfo则要保存到本地</typeparam>
            /// <param name="uri"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            private IEnumerator R_GetAssetAsync<T>(Uri uri, ZincAction<T> callback) where T : class
            {
                UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
                yield return unityWebRequest.SendWebRequest();

                Type type = typeof(T);
                switch (type)
                {
                    case Type when type == typeof(AssetBundle):
                        callback.Invoke(DownloadHandlerAssetBundle.GetContent(unityWebRequest) as T);
                        break;
                    case Type when type == typeof(Texture):
                        callback.Invoke(DownloadHandlerTexture.GetContent(unityWebRequest) as T);
                        break;
                    case Type when type == typeof(AudioClip):
                        callback.Invoke(DownloadHandlerAudioClip.GetContent(unityWebRequest) as T);
                        break;
                    case Type when type == typeof(byte[]):
                        callback.Invoke(unityWebRequest.downloadHandler.data as T);
                        break;
                    case Type when type == typeof(string):
                        callback.Invoke(unityWebRequest.downloadHandler.text as T);
                        break;
                    case Type when type == typeof(FileInfo):
                        callback.Invoke(null);
                        break;
                }
            }

            public void GetDataAsync<T>(string uri, ZincAction<T> callback) where T : ISerializable
            {
                MonoManager.Instance.StartCoroutine(R_GetDataAsync<T>(new Uri(uri ?? _defaultURL), callback));
            }

            public void GetDataAsync<T>(Uri uri, ZincAction<T> callback) where T : ISerializable
            {
                MonoManager.Instance.StartCoroutine(R_GetDataAsync<T>(uri, callback));
            }

            private IEnumerator R_GetDataAsync<T>(Uri uri, ZincAction<T> callback) where T : ISerializable
            {
                UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
                yield return unityWebRequest.SendWebRequest();

                DownloadHandler downloadHandler = unityWebRequest.downloadHandler;

                callback?.Invoke(BinarySerializer.Deserialize<T>(downloadHandler.data));
            }

            public void UploadDataAsync(Uri uri, ISerializable serializable ,ZincAction<UnityWebRequest.Result> zincAction = null)
            {
                MonoManager.Instance.StartCoroutine(R_UploadDataAsync(uri, serializable, zincAction));
            }

            public void UploadDataAsync(string uri, ISerializable serializable, ZincAction<UnityWebRequest.Result> zincAction = null)
            {
                MonoManager.Instance.StartCoroutine(R_UploadDataAsync(new Uri(uri ?? _defaultURL), serializable, zincAction));
            }

            private IEnumerator R_UploadDataAsync(Uri uri, ISerializable serializable, ZincAction<UnityWebRequest.Result> zincAction = null)
            {
                byte[] buffer = serializable.Serialize();
                List<IMultipartFormSection> multipartFormSections = new List<IMultipartFormSection>() { new MultipartFormDataSection(buffer) };

                UnityWebRequest unityWebRequest = UnityWebRequest.Post(uri, multipartFormSections);
              
                yield return unityWebRequest.SendWebRequest();

                zincAction?.Invoke(unityWebRequest.result);
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    LogUtility.LogError(unityWebRequest.result + "上传失败");
                }
            }

            public void UploadFileAsync(Uri uri, string fileName, string localPath, ZincAction<UnityWebRequest.Result> zincAction = null)
            {
                MonoManager.Instance.StartCoroutine(R_UploadFileAsync(uri, fileName, localPath, zincAction));
            }

            public void UploadFileAsync(string uri, string fileName, string localPath, ZincAction<UnityWebRequest.Result> zincAction = null)
            {
                MonoManager.Instance.StartCoroutine(R_UploadFileAsync(new Uri(uri ?? _defaultURL), fileName, localPath, zincAction));
            }

            private IEnumerator R_UploadFileAsync(Uri uri, string fileName, string localPath, ZincAction<UnityWebRequest.Result> zincAction = null)
            {
                List<IMultipartFormSection> multipartFormSections;
                using (FileStream fileStream = File.OpenRead(localPath))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    multipartFormSections = new List<IMultipartFormSection>() { new MultipartFormFileSection(fileName, buffer) };
                }

                UnityWebRequest unityWebRequest = UnityWebRequest.Post(uri, multipartFormSections);

                yield return unityWebRequest.SendWebRequest();

                zincAction?.Invoke(unityWebRequest.result);
                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    LogUtility.LogError(unityWebRequest.result + "上传失败");
                }
            }
        }
    }
}

