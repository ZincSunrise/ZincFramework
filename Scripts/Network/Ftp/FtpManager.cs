using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using ZincFramework.Events;

namespace ZincFramework
{
    namespace Network
    {
        namespace Ftp
        {
            public class FtpManager : BaseSafeSingleton<FtpManager>
            {
                private byte[] _buffer = new byte[1024];
                public static string FtpPath = "ftp://127.0.0.1/";
                private FtpManager() { }

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

                /// <summary>
                /// �ϴ��ļ���FTP������
                /// </summary>
                /// <param name="account">�˺�</param>
                /// <param name="password">����</param>
                /// <param name="remotePath">Զ��·��������Ҫ����Զ��IP��ַ</param>
                /// <param name="locatePath">����·��</param>
                /// <param name="zincAction"></param>
                /// <param name="bufferSize"></param>
                /// <param name="keepAlive"></param>
                public async void UploadFileAsync(string account, string password, string remotePath, string locatePath, ZincAction zincAction = null, int bufferSize = 1024, bool keepAlive = false)
                {
                    try
                    {
                        FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(Path.Combine(FtpPath, remotePath))) as FtpWebRequest;
                        //����ƾ֤
                        ftpWebRequest.Credentials = InitializeCredential(account, password);
                        //���ô���
                        ftpWebRequest.Proxy = null;
                        //���÷���
                        ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                        ftpWebRequest.UseBinary = true;

                        ftpWebRequest.KeepAlive = keepAlive;

                        if (bufferSize != _buffer.Length)
                        {
                            _buffer = new byte[bufferSize];
                        }

                        using (Stream stream = await ftpWebRequest.GetRequestStreamAsync())
                        {
                            using (FileStream fileStream = File.OpenRead(locatePath))
                            {
                                int contentLength;
                                while ((contentLength = fileStream.Read(_buffer, 0, _buffer.Length)) > 0)
                                {
                                    await stream.WriteAsync(_buffer, 0, contentLength);
                                    contentLength = await fileStream.ReadAsync(_buffer, 0, _buffer.Length);
                                }
                                stream.Close();
                                fileStream.Close();
                            }
                        }
                        zincAction?.Invoke();
                    }
                    catch (Exception e)
                    {
                        LogUtility.LogError(e.Message);
                    }
                }

                public async void DownloadFileAsync(string account, string password, string remotePath, string locatePath, ZincAction zincAction = null, int bufferSize = 1024, bool keepAlive = false)
                {
                    try
                    {
                        FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(Path.Combine(FtpPath, remotePath))) as FtpWebRequest;
                        //����ƾ֤
                        ftpWebRequest.Credentials = InitializeCredential(account, password);
                        //���ô���
                        ftpWebRequest.Proxy = null;
                        //���÷���
                        ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                        ftpWebRequest.UseBinary = true;

                        ftpWebRequest.KeepAlive = keepAlive;

                        if (bufferSize != _buffer.Length)
                        {
                            _buffer = new byte[bufferSize];
                        }

                        FtpWebResponse ftpWebResponse = await ftpWebRequest.GetResponseAsync() as FtpWebResponse;

                        using (FileStream fileStream = File.Create(locatePath))
                        {
                            using (Stream stream = ftpWebResponse.GetResponseStream())
                            {
                                int contentLength;
                                while ((contentLength = await stream.ReadAsync(_buffer, 0, _buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(_buffer, 0, contentLength);
                                }
                                stream.Close();
                                fileStream.Close();
                            }
                        }
                        zincAction?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        LogUtility.LogError(ex.Message);
                    }
                }

                public async void DeleteFileAsync(string account, string password, string filePath, ZincAction<bool> zincAction = null, int bufferSize = 1024, bool keepAlive = false)
                {
                    try
                    {
                        FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(Path.Combine(FtpPath, filePath))) as FtpWebRequest;
                        //����ƾ֤
                        ftpWebRequest.Credentials = InitializeCredential(account, password);
                        //���ô���
                        ftpWebRequest.Proxy = null;
                        //���÷���
                        ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                        ftpWebRequest.UseBinary = true;

                        ftpWebRequest.KeepAlive = keepAlive;

                        if (bufferSize != _buffer.Length)
                        {
                            _buffer = new byte[bufferSize];
                        }

                        await ftpWebRequest.GetResponseAsync();
                        zincAction?.Invoke(true);
                    }
                    catch (Exception ex)
                    {
                        LogUtility.LogError(ex.Message);
                        zincAction?.Invoke(false);
                    }
                }

                public async void CreateDirectoryAsync(string account, string password, string directoryPath, ZincAction<bool> zincAction = null, int bufferSize = 1024, bool keepAlive = false)
                {
                    try
                    { 
                        FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(Path.Combine(FtpPath, directoryPath))) as FtpWebRequest;
                        //����ƾ֤
                        ftpWebRequest.Credentials = InitializeCredential(account, password);
                        //���ô���
                        ftpWebRequest.Proxy = null;
                        //���÷���
                        ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                        ftpWebRequest.UseBinary = true;

                        ftpWebRequest.KeepAlive = keepAlive;

                        if (bufferSize != _buffer.Length)
                        {
                            _buffer = new byte[bufferSize];
                        }

                        await ftpWebRequest.GetResponseAsync();
                        zincAction?.Invoke(true);
                    }
                    catch (Exception ex)
                    {
                        LogUtility.LogError(ex.Message);
                        zincAction?.Invoke(false);
                    }
                }

                public async void DeleteDirectoryAsync(string account, string password, string directoryPath, ZincAction<bool> zincAction = null, int bufferSize = 1024, bool keepAlive = false)
                {
                    try
                    {
                        FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(Path.Combine(FtpPath, directoryPath))) as FtpWebRequest;
                        //����ƾ֤
                        ftpWebRequest.Credentials = InitializeCredential(account, password);
                        //���ô���
                        ftpWebRequest.Proxy = null;
                        //���÷���
                        ftpWebRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                        ftpWebRequest.UseBinary = true;

                        ftpWebRequest.KeepAlive = keepAlive;

                        if (bufferSize != _buffer.Length)
                        {
                            _buffer = new byte[bufferSize];
                        }

                        await ftpWebRequest.GetResponseAsync();
                        zincAction?.Invoke(true);
                    }
                    catch (Exception ex)
                    {
                        LogUtility.LogError(ex.Message);
                        zincAction?.Invoke(false);
                    }
                }

                /// <summary>
                /// �����Ҫ��ȡ��Ŀ¼�ʹ����ַ���
                /// </summary>
                /// <param name="account"></param>
                /// <param name="password"></param>
                /// <param name="directoryPath">�����Ҫ��ȡ��Ŀ¼�ʹ����ַ���</param>
                /// <param name="zincAction"></param>
                /// <param name="bufferSize"></param>
                /// <param name="keepAlive"></param>
                public async void GetDirectoryListAsync(string account, string password, string directoryPath, ZincAction<List<string>> zincAction = null, int bufferSize = 1024, bool keepAlive = false)
                {
                    try
                    {
                        FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(Path.Combine(FtpPath, directoryPath))) as FtpWebRequest;
                        //����ƾ֤
                        ftpWebRequest.Credentials = InitializeCredential(account, password);
                        //���ô���
                        ftpWebRequest.Proxy = null;
                        //���÷���
                        ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                        ftpWebRequest.UseBinary = true;

                        ftpWebRequest.KeepAlive = keepAlive;

                        if (bufferSize != _buffer.Length)
                        {
                            _buffer = new byte[bufferSize];
                        }
                        FtpWebResponse ftpWebResponse = await ftpWebRequest.GetResponseAsync() as FtpWebResponse;

                        using (StreamReader streamReader = new StreamReader(ftpWebResponse.GetResponseStream()))
                        {
                            string line;
                            List<string> lines = new List<string>();
                            while((line = streamReader.ReadLine()) != null)
                            {
                                lines.Add(line);
                            }
                            zincAction?.Invoke(lines);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtility.LogError(ex.Message);
                        zincAction?.Invoke(null);
                    }
                }

                /// <summary>
                /// �����Ҫ��ȡ��Ŀ¼�ʹ����ַ���
                /// </summary>
                /// <param name="account"></param>
                /// <param name="password"></param>
                /// <param name="directoryPath">�����Ҫ��ȡ��Ŀ¼�ʹ����ַ���</param>
                /// <param name="zincAction"></param>
                /// <param name="bufferSize"></param>
                /// <param name="keepAlive"></param>
                public async void GetFileSize(string account, string password, string filePath, ZincAction<long> zincAction = null, int bufferSize = 1024, bool keepAlive = false)
                {
                    try
                    {
                        FtpWebRequest ftpWebRequest = FtpWebRequest.Create(new Uri(Path.Combine(FtpPath, filePath))) as FtpWebRequest;
                        //����ƾ֤
                        ftpWebRequest.Credentials = InitializeCredential(account, password);
                        //���ô���
                        ftpWebRequest.Proxy = null;
                        //���÷���
                        ftpWebRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                        ftpWebRequest.UseBinary = true;

                        ftpWebRequest.KeepAlive = keepAlive;

                        if (bufferSize != _buffer.Length)
                        {
                            _buffer = new byte[bufferSize];
                        }
                        FtpWebResponse ftpWebResponse = await ftpWebRequest.GetResponseAsync() as FtpWebResponse;

                        zincAction?.Invoke(ftpWebResponse.ContentLength);
                    }
                    catch (Exception ex)
                    {
                        LogUtility.LogError(ex.Message);
                        zincAction?.Invoke(-1);
                    }
                }
            }
        }
    }
}
