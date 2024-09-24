using System.Net.Sockets;
using UnityEngine;


namespace ZincFramework
{
    public class FrameworkData : ScriptableObject
    {
        [Header("-----------文件读取存储相关-----------")]
        [Header("保存文件的拓展名")]
        public string extension;

        [Header("-----------资源读取相关-----------")]
        [Header("是否在加载资源的时候是debug模式")]
        public bool isDebug = false;

        [Header("-----------音乐相关-----------")]
        [Header("音乐模式")]
        public E_Sound_Mode soundMode = E_Sound_Mode.TwoD;
        [Header("音乐淡出时间")]
        public float fadePersent = 0.2f;
        [Header("音乐音效单位最大数量,必须是2的幂次方")]
        public int maxSoundCount = 32;
        [Header("3D模式下,音乐物体的播放完后的消失延迟")]
        public float disappearOffset = 0.2f;

        [Header("-----------对象池相关-----------")]
        [Header("是否开启游戏对象池排列功能")]
        public bool isOpenLayout = false;
        [Header("游戏对象池创建对象池的默认最大容量")]
        public int maxPoolCount = 16;


        [Header("-----------输入系统相关-----------")]
        [Header("是否启用了新输入系统")]
        public bool isInputSystem = false;

        [Header("-----------计时相关-----------")]
        [Header("计时管理器每隔多少秒检测一次")]
        public float intervalCheckTime = 0.05f;

        [Header("-----------自动保存相关-----------")]
        [Header("自动保存间隔时间")]
        public int saveOffset = 600;

        [Header("-----------网络相关-----------")]
        [Header("本地IP地址")]
        public string localAddress = "127.0.0.1";
        [Header("本地IP地址的端口")]
        public short localPort = 11451;

        [Header("远端服务器IP地址")]
        public string remoteAddress = "127.0.0.1";
        [Header("远端服务器IP地址的端口")]
        public short remotePort = 19198;

        [Header("间隔多少秒向服务器发送心跳消息")]
        public int sendHeartMassageOffset = 10;

        
        [Header("本地链接协议")]
        public ProtocolType protocolType = ProtocolType.Tcp;


        public void Deconstruct(out string extension) =>
            extension = this.extension;

        public void Deconstruct(out E_Sound_Mode soundMode, out float fadePersent, out int maxSoundCount, out float disappearOffset) =>
            (soundMode, fadePersent, maxSoundCount, disappearOffset) = (this.soundMode, this.fadePersent, this.maxSoundCount, this.disappearOffset);


        public void Deconstruct(out int sendHeartMassageOffset, out ProtocolType protocolType) =>
            (sendHeartMassageOffset, protocolType) = (this.sendHeartMassageOffset, this.protocolType);
    }
}

