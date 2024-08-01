using System;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace Network
    {
        namespace Http
        {
            public class HttpManager : BaseSafeSingleton<HttpManager> 
            {
                public static string RemoteIP = "http://81.71.155.200:11451/";

                private HttpManager() { }

                private NetworkCredential InitializeCredential(string account, string password)
                {
                    using (SecureString secureString = new SecureString())
                    {
                        for (int i = 0; i < password.Length; i++)
                        {
                            secureString.AppendChar(password[i]);
                        }
                        password = null;
                        return new NetworkCredential(account, secureString);
                    }
                }


                public async void DownloadFileAsync(string locatePath, string remotePath, ZincAction<bool> callback = null, string account = "", string password = "" ,int bufferSize = 4096)
                {
                    try
                    {
                        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(Path.Combine(RemoteIP, remotePath))) as HttpWebRequest;
                        httpWebRequest.Method = WebRequestMethods.Http.Get;
                        httpWebRequest.Timeout = 2000;
                        httpWebRequest.Credentials = InitializeCredential(account, password);
                        httpWebRequest.PreAuthenticate = true;
                        HttpWebResponse httpWebResponse = await httpWebRequest.GetResponseAsync() as HttpWebResponse;

                        if(httpWebResponse.StatusCode == HttpStatusCode.OK)
                        {
                            await using (Stream stream = httpWebResponse.GetResponseStream())
                            {
                                await using (FileStream fileStream = File.Create(locatePath))
                                {
                                    byte[] buffer = new byte[bufferSize];
                                    int contentLength;
                                    while ((contentLength = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                    {
                                        fileStream.Write(buffer, 0, contentLength);
                                    }
                                }
                            }
                            callback?.Invoke(true);
                        }
                        else
                        {
                            LogUtility.LogError(httpWebResponse.StatusCode + httpWebResponse.StatusCode.ToString());
                            callback?.Invoke(false);
                        }
                    }
                    catch(WebException e)
                    {
                        LogUtility.LogError(e.Message + e.Status);
                        callback?.Invoke(false);
                    }
                }

                public async void UploadFileAsync(string locatePath, string remoteDirectory, string fileName, ZincAction<bool> callback = null, string account = "", string password = "", int bufferSize = 4096)
                {
                    try
                    {
                        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(Path.Combine(RemoteIP, remoteDirectory))) as HttpWebRequest;
                        httpWebRequest.Method = WebRequestMethods.Http.Post;
                        httpWebRequest.Timeout = 500000;

                        httpWebRequest.Credentials = InitializeCredential(account, password);
                        httpWebRequest.PreAuthenticate = true;

                        DateTime dateTime = DateTime.Now;
                        string dateStr = dateTime.ToString();
                        httpWebRequest.ContentType = $"multipart/form-data;boundary={dateStr}";

                        string head = $"--{dateStr}\r\n" +
                            $"Content-Disposition:form-data;name=\"file\";filename={fileName}\r\n" +
                            "Content-Type:application/octet-stream\r\n\r\n";

                        byte[] beginBytes = Encoding.UTF8.GetBytes(head);
                        byte[] endBytes = Encoding.UTF8.GetBytes($"\r\n--{dateStr}--\r\n");
                        
                        await using (FileStream fileStream = File.OpenRead(locatePath))
                        {
                            httpWebRequest.ContentLength = endBytes.Length + beginBytes.Length + fileStream.Length;
                            await using (Stream stream = await httpWebRequest.GetRequestStreamAsync())
                            {
                                await stream.WriteAsync(beginBytes, 0, beginBytes.Length);

                                byte[] buffer = new byte[bufferSize];
                                int contentLength;
                                while ((contentLength = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    await stream.WriteAsync(buffer, 0, contentLength);
                                }

                                await stream.WriteAsync(endBytes, 0, endBytes.Length);
                            }
                        }


                        HttpWebResponse httpWebResponse = await httpWebRequest.GetResponseAsync() as HttpWebResponse;
                        if(httpWebResponse.StatusCode == HttpStatusCode.OK)
                        {
                            callback?.Invoke(true);
                        }
                        else
                        {
                            LogUtility.LogError(httpWebResponse.StatusCode.ToString());
                            callback?.Invoke(false);
                        }
                    }
                    catch (WebException e)
                    {
                        LogUtility.LogError(e.Message + e.Status);
                        callback?.Invoke(false);
                    }
                }
            }
        }
    }
}

