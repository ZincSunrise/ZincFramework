using UnityEngine;
using ZincFramework.Pools;

namespace ZincFramework.Audio.Internal
{
    internal class SoundSource : ISoundBase, IReuseable
    {
        public int Version { get; private set; }

        public ISoundBase SoundBase { get; private set; }

        public bool IsStopped { get; private set; }

        public AudioSource AudioSource => SoundBase?.AudioSource;

        public bool IsLoop => SoundBase != null && SoundBase.IsLoop;

        public bool IsPausing => SoundBase != null && SoundBase.IsPausing;

        public bool IsPlaying => SoundBase != null && SoundBase.IsPlaying;


        private static int _version = 0;

        public void Init(ISoundBase soundBase)
        {
            Version = _version;
            SoundBase = soundBase;
        }

        public void Play()
        {
            SoundBase?.Play();
        }

        public void Pause()
        {
            SoundBase?.Pause();
        }

        public void Refresh()
        {
            SoundBase?.Refresh();
        }

        public void SetVolume(float soundVolume)
        {
            SoundBase?.SetVolume(soundVolume);
        }

        public void Mute(bool isMute)
        {
            SoundBase?.Mute(isMute);
        }

        public void Stop() 
        {
            if (!IsStopped)
            {
                SoundBase?.Stop();
                IsStopped = true;
            }
        }

        public void OnRent()
        {
            IsStopped = false;
            Version = _version++;
        }

        public void OnReturn()
        {
            Version = -1;
            SoundBase = null;
        }

        void ISoundBase.Initialize(AudioSource audioSource)
        {
            throw new System.NotImplementedException();
        }
    }
}