using System;
using ZincFramework.Audio.Internal;


namespace ZincFramework.Audio
{
    /// <summary>
    /// 作为值类型设计，支持多句柄指向同一底层资源。
    /// <para>该句柄负责控制播放相关操作，包含播放、暂停、停止等功能。</para>
    /// <para>
    /// 注意：调用 <see cref="Stop"/> 方法会立即停止音效播放并释放相关资源，
    /// 一旦调用该方法，该句柄即视为无效，后续对该句柄的任何操作都会抛出异常。
    /// </para>
    /// </summary>
    public readonly struct SoundHandle : IDisposable, IEquatable<SoundHandle>
    {
        private readonly SoundSource _soundSource;

        private readonly int _version;

        /// <summary>
        /// 是否为有效句柄（非空、未停止、版本匹配）。
        /// </summary>
        public bool IsValid => _soundSource != null && !_soundSource.IsStopped && _version == _soundSource.Version;

        /// <summary>
        /// 是否为循环播放音效。
        /// </summary>
        public bool IsLoop => _soundSource != null && _soundSource.IsLoop;

        /// <summary>
        /// 是否处于暂停状态。
        /// </summary>
        public bool IsPausing => _soundSource != null && _soundSource.IsPausing;

        /// <summary>
        /// 是否处于播放状态。
        /// </summary>
        public bool IsPlaying => _soundSource != null && _soundSource.IsPlaying;

        internal SoundHandle(SoundSource soundSource)
        {
            _soundSource = soundSource;
            _version = soundSource.Version;
        }

        /// <summary>
        /// 开始或继续播放音效。
        /// </summary>
        /// <exception cref="InvalidOperationException">当句柄无效时抛出。</exception>
        public void Play()
        {
            if (!IsValid)
                throw new InvalidOperationException("SoundHandle 已失效，不能再使用。");

            _soundSource.Play();
        }

        /// <summary>
        /// 暂停当前音效播放。
        /// </summary>
        /// <exception cref="InvalidOperationException">当句柄无效时抛出。</exception>
        public void Pause()
        {
            if (!IsValid)
                throw new InvalidOperationException("SoundHandle 已失效，不能再使用。");

            _soundSource.Pause();
        }

        /// <summary>
        /// 刷新当前音效，通常用于重播或重新加载。
        /// </summary>
        /// <exception cref="InvalidOperationException">当句柄无效时抛出。</exception>
        public void Refresh()
        {
            if (!IsValid)
                throw new InvalidOperationException("SoundHandle 已失效，不能再使用。");

            _soundSource.Refresh();
        }

        /// <summary>
        /// 设置音效播放音量，范围通常为 0~1。
        /// </summary>
        /// <param name="soundVolume">目标音量。</param>
        /// <exception cref="InvalidOperationException">当句柄无效时抛出。</exception>
        public void SetVolume(float soundVolume)
        {
            if (!IsValid)
                throw new InvalidOperationException("SoundHandle 已失效，不能再使用。");

            _soundSource.SetVolume(soundVolume);
        }

        /// <summary>
        /// 静音或取消静音当前音效。
        /// </summary>
        /// <param name="isMute">true 表示静音，false 表示取消静音。</param>
        /// <exception cref="InvalidOperationException">当句柄无效时抛出。</exception>
        public void Mute(bool isMute)
        {
            if (!IsValid)
                throw new InvalidOperationException("SoundHandle 已失效，不能再使用。");

            _soundSource.Mute(isMute);
        }

        /// <summary>
        /// 停止音效播放并释放底层资源，调用后所有指向该资源的旧句柄都会失效。
        /// </summary>
        /// <exception cref="InvalidOperationException">当句柄无效时抛出。</exception>
        public void Stop()
        {
            if (!IsValid)
                throw new InvalidOperationException($"SoundHandle 已失效，不能再使用。当前版本号:{_version} 资源版本号{_soundSource.Version}");

            _soundSource.Stop();
            Dispose();
        }

        /// <summary>
        /// 释放句柄关联的资源。当前为空方法，预留接口。
        /// </summary>
        public void Dispose()
        {
            // 目前无显式资源释放逻辑，预留扩展用。
        }

        /// <summary>
        /// 判断两个 SoundHandle 是否指向相同底层资源且版本一致。
        /// </summary>
        /// <param name="other">另一个 SoundHandle 实例。</param>
        /// <returns>true 表示两个句柄等价，false 表示不等价。</returns>
        public bool Equals(SoundHandle other)
        {
            return _soundSource == other._soundSource && _version == other._version;
        }
    }

}
