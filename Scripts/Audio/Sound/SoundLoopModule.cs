using System;
using UnityEngine;
using UnityEngine.Profiling;
using ZincFramework.Audio.Internal;
using ZincFramework.Events;
using ZincFramework.Loop;
using ZincFramework.Loop.Internal;
using ZincFramework.Pools.GameObjects;


namespace ZincFramework.Audio.Loop
{
    internal class SoundLoopModule : ILoopModule<SoundLoopModule.SoundItem>, ILoopModule<SoundLoopModule.LoopSoundItem>
    {
        public ReadOnlySpan<SoundItem> SoundSpan => _soundArray.AsSpan();

        public ReadOnlySpan<LoopSoundItem> LoopSoundSpan => _loopSoundArray == null ? new ReadOnlySpan<LoopSoundItem>() : _loopSoundArray.AsSpan();

        private readonly struct MusicLoop { }

        #region 内部音乐类
        /// <summary>
        /// 一次性音效
        /// </summary>
        internal readonly struct SoundItem : ILoopItem, IEquatable<SoundItem>
        {
            public SoundSource SoundSource { get; }

            private readonly ZincAction _onSoundEnd;

            public SoundItem(SoundSource soundSource, ZincAction onSoundEnd)
            {
                SoundSource = soundSource;
                _onSoundEnd = onSoundEnd;
            }

            public bool Tick()
            {
                if (SoundSource.IsStopped || !SoundSource.IsPlaying && !SoundSource.IsPausing)
                {
                    _onSoundEnd?.Invoke();

                    //回收音频对象
                    SoundEffectManager.Instance.ReturnSound(SoundSource);
                    return false;
                }

                return true;
            }

            public bool Equals(SoundItem other)
            {
                return SoundSource?.AudioSource == other.SoundSource?.AudioSource;
            }

            public override bool Equals(object obj)
            {
                return obj is SoundItem sound && sound.Equals(this);
            }

            public override int GetHashCode()
            {
                return SoundSource.AudioSource.GetHashCode();
            }

            public static bool operator ==(SoundItem a, SoundItem b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(SoundItem a, SoundItem b)
            {
                return !a.Equals(b);
            }
        }

        /// <summary>
        /// 循环播放音效
        /// </summary>
        internal struct LoopSoundItem : ILoopItem, IEquatable<LoopSoundItem>
        {
            public SoundSource SoundSource { get; }

            public int LoopCount { get; private set; }

            private readonly ZincAction _onSoundEnd;

            public LoopSoundItem(SoundSource soundSource, int loopCount, ZincAction onSoundEnd)
            {
                SoundSource = soundSource;
                LoopCount = loopCount;
                _onSoundEnd = onSoundEnd;
            }

            public bool Tick()
            {
                if (SoundSource.IsStopped)
                {
                    _onSoundEnd?.Invoke();

                    //回收音频对象
                    SoundEffectManager.Instance.ReturnSound(SoundSource);
                    return false;
                }

                if (!SoundSource.IsPlaying && !SoundSource.IsPausing)
                {
                    _onSoundEnd?.Invoke();

                    if (LoopCount < 0)
                    {
                        return true;
                    }
                    else if(LoopCount > 0)
                    {
                        LoopCount--;

                        Debug.Log(LoopCount);

                        if (LoopCount == 0)
                        {
                            SoundEffectManager.Instance.ReturnSound(SoundSource);
                            return false;
                        }

                        SoundSource.Play();
                        return true;
                    }
                    else
                    {
                        //回收音频对象
                        SoundEffectManager.Instance.ReturnSound(SoundSource);
                        return false;
                    }
                }

                return true;
            }

            #region 重写等值方法
            public bool Equals(LoopSoundItem other)
            {
                return SoundSource?.AudioSource == other.SoundSource?.AudioSource;
            }

            public override bool Equals(object obj)
            {
                return obj is LoopSoundItem sound && sound.Equals(this);
            }

            public override int GetHashCode()
            {
                return SoundSource.AudioSource.GetHashCode();
            }

            public static bool operator ==(LoopSoundItem a, LoopSoundItem b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(LoopSoundItem a, LoopSoundItem b)
            {
                return !a.Equals(b);
            }
            #endregion
        }
        #endregion

        public E_LoopType LoopType => E_LoopType.FixedUpdate;

        public Type FlagType => typeof(MusicLoop);


        private readonly LoopValueAutoArray<SoundItem> _soundArray = new LoopValueAutoArray<SoundItem>();

        private LoopValueAutoArray<LoopSoundItem> _loopSoundArray;

        public void Clear()
        {
            _soundArray.Clear();
            _loopSoundArray?.Clear();
        }

        /// <summary>
        /// 设置为-1则代表永久Loop
        /// </summary>
        /// <param name="soundSource"></param>
        /// <param name="loopCount"></param>
        public void RegisterLoopSound(SoundSource soundSource, int loopCount, ZincAction onSoundEnd)
        {
            _loopSoundArray ??= new LoopValueAutoArray<LoopSoundItem>();

            LoopSoundItem loopSoundItem = new LoopSoundItem(soundSource, loopCount, onSoundEnd);
            _loopSoundArray.Register(loopSoundItem);
        }

        public void RegisterSound(SoundSource soundSource, ZincAction onSoundEnd)
        {
            SoundItem soundItem = new SoundItem(soundSource, onSoundEnd);
            _soundArray.Register(soundItem);
        }

        public void Tick()
        {
            _soundArray.Tick();
            _loopSoundArray?.Tick();
        }

        #region 重写的接口方法
        public void Register(SoundItem loopItem)
        {
            _soundArray.Register(loopItem);
        }

        public void Unregister(SoundItem loopItem)
        {
            _soundArray.Unregister(loopItem);
        }

        public void Register(LoopSoundItem loopItem)
        {
            _loopSoundArray.Register(loopItem);
        }

        public void Unregister(LoopSoundItem loopItem)
        {
            _loopSoundArray.Unregister(loopItem);
        }

        void ILoopModule.Register(ILoopItem loopItem)
        {
            if(loopItem is LoopSoundItem loopSoundItem)
            {
                Register(loopSoundItem);
            }
            else if(loopItem is SoundItem soundItem)
            {
                Register(soundItem);
            }

            throw new NotSupportedException("你不可以传入不支持的SoundItem");
        }

        void ILoopModule.Unregister(ILoopItem loopItem)
        {
            if (loopItem is LoopSoundItem loopSoundItem)
            {
                Unregister(loopSoundItem);
            }
            else if (loopItem is SoundItem soundItem)
            {
                Unregister(soundItem);
            }

            throw new NotSupportedException("你不可以传入不支持的SoundItem");
        }
        #endregion
    }
}
