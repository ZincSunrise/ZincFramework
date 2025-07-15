using UnityEngine;


namespace ZincFramework.Audio
{
    /// <summary>
    /// 定义所有音效控制行为的统一接口。
    /// 所有可控音效（2D/3D/环境音等）都应实现该接口，用于外部统一控制播放、暂停、静音等。
    /// </summary>
    public interface ISoundBase
    {
        /// <summary>
        /// 绑定的 Unity AudioSource 实例。
        /// </summary>
        AudioSource AudioSource { get; }

        /// <summary>
        /// 是否为循环播放（对应 AudioSource.loop）。
        /// </summary>
        bool IsLoop { get; }

        /// <summary>
        /// 当前是否为暂停状态。
        /// </summary>
        bool IsPausing { get; }

        /// <summary>
        /// 当前是否正在播放。
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// 初始化音效资源，通常在对象池复用或创建时调用。
        /// </summary>
        /// <param name="audioSource">待绑定的 AudioSource 实例</param>
        void Initialize(AudioSource audioSource);

        /// <summary>
        /// 设置播放音量（范围 0~1）。
        /// </summary>
        /// <param name="soundVolume">目标音量</param>
        void SetVolume(float soundVolume);

        /// <summary>
        /// 播放音效。若处于暂停状态，将从暂停位置恢复播放；否则从头播放。
        /// </summary>
        void Play();

        /// <summary>
        /// 暂停音效播放。播放状态和播放位置将被保留，可调用 Play 恢复。
        /// </summary>
        void Pause();

        /// <summary>
        /// 从头开始重新播放音效。相当于 Stop 后立即 Play。
        /// </summary>
        void Refresh();

        /// <summary>
        /// 停止音效播放并重置播放进度。
        /// 同时会从逻辑控制区域（如音效循环模块）中移除该音效。
        /// </summary>
        void Stop();

        /// <summary>
        /// 设置是否静音（不影响当前音量设置，只是关闭输出）。
        /// </summary>
        /// <param name="isMute">是否静音</param>
        void Mute(bool isMute);
    }
}
