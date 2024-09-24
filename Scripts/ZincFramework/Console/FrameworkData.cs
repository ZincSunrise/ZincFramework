using System.Net.Sockets;
using UnityEngine;


namespace ZincFramework
{
    public class FrameworkData : ScriptableObject
    {
        [Header("-----------�ļ���ȡ�洢���-----------")]
        [Header("�����ļ�����չ��")]
        public string extension;

        [Header("-----------��Դ��ȡ���-----------")]
        [Header("�Ƿ��ڼ�����Դ��ʱ����debugģʽ")]
        public bool isDebug = false;

        [Header("-----------�������-----------")]
        [Header("����ģʽ")]
        public E_Sound_Mode soundMode = E_Sound_Mode.TwoD;
        [Header("���ֵ���ʱ��")]
        public float fadePersent = 0.2f;
        [Header("������Ч��λ�������,������2���ݴη�")]
        public int maxSoundCount = 32;
        [Header("3Dģʽ��,��������Ĳ���������ʧ�ӳ�")]
        public float disappearOffset = 0.2f;

        [Header("-----------��������-----------")]
        [Header("�Ƿ�����Ϸ��������й���")]
        public bool isOpenLayout = false;
        [Header("��Ϸ����ش�������ص�Ĭ���������")]
        public int maxPoolCount = 16;


        [Header("-----------����ϵͳ���-----------")]
        [Header("�Ƿ�������������ϵͳ")]
        public bool isInputSystem = false;

        [Header("-----------��ʱ���-----------")]
        [Header("��ʱ������ÿ����������һ��")]
        public float intervalCheckTime = 0.05f;

        [Header("-----------�Զ��������-----------")]
        [Header("�Զ�������ʱ��")]
        public int saveOffset = 600;

        [Header("-----------�������-----------")]
        [Header("����IP��ַ")]
        public string localAddress = "127.0.0.1";
        [Header("����IP��ַ�Ķ˿�")]
        public short localPort = 11451;

        [Header("Զ�˷�����IP��ַ")]
        public string remoteAddress = "127.0.0.1";
        [Header("Զ�˷�����IP��ַ�Ķ˿�")]
        public short remotePort = 19198;

        [Header("��������������������������Ϣ")]
        public int sendHeartMassageOffset = 10;

        
        [Header("��������Э��")]
        public ProtocolType protocolType = ProtocolType.Tcp;


        public void Deconstruct(out string extension) =>
            extension = this.extension;

        public void Deconstruct(out E_Sound_Mode soundMode, out float fadePersent, out int maxSoundCount, out float disappearOffset) =>
            (soundMode, fadePersent, maxSoundCount, disappearOffset) = (this.soundMode, this.fadePersent, this.maxSoundCount, this.disappearOffset);


        public void Deconstruct(out int sendHeartMassageOffset, out ProtocolType protocolType) =>
            (sendHeartMassageOffset, protocolType) = (this.sendHeartMassageOffset, this.protocolType);
    }
}

