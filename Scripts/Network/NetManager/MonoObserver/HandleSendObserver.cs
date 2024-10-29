using System;
using ZincFramework.MonoModel;



namespace ZincFramework.Network
{
    public sealed partial class NetworkManager
    {
        private class HandleSendObserver : IMonoObserver
        {
            public async void NotifyObserver()
            {
                int nowIndex = 0;
                int totalLength = 0;
                foreach (var item in Instance._sendQueue)
                {
                    totalLength += item.Length;
                }

                byte[] sendBuffer = new byte[totalLength];

                while (Instance._sendQueue.Count > 0)
                {
                    if (Instance._sendQueue.TryDequeue(out Memory<byte> item))
                    {
                        item.CopyTo(sendBuffer.AsMemory(nowIndex));
                        nowIndex += item.Length;
                    }
                    else
                    {
                        LogUtility.LogError("���Ӵ��������Ƿ����߳�����");
                    }
                }

                if (nowIndex > 0)
                {
                    if (!await Instance._networkModule.SendAsync(sendBuffer, 0, nowIndex))
                    {
                        LogUtility.LogError("����ʧ�ܣ����ڶ�������������");
                        Instance.Disconnect(true);
                    }
                }
            }

            public void OnRegist()
            {

            }

            public void OnRemove()
            {

            }
        }
    }
}