using UnityEngine;
using ZincFramework.Pools.GameObjects;


namespace ZincFramework.Audio.Internal
{
    internal class ThreeDSound : ReuseableObject, ISoundBase
    {
        public AudioSource AudioSource => _audioSource;

        public bool IsPausing {  get; private set; }

        public bool IsLoop => _audioSource != null && AudioSource.loop;

        public bool IsPlaying => _audioSource != null && AudioSource.isPlaying;


        [SerializeField]
        private AudioSource _audioSource;

        public void Initialize(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void Play()
        {
            IsPausing = false;
            AudioSource.Play();
        }

        public void Pause()
        {
            IsPausing = true;
            AudioSource.Pause();
        }

        public void Refresh()
        {
            IsPausing = false;
            AudioSource.Stop();
            AudioSource.Play();
        }

        public void SetVolume(float soundVolume)
        {
            AudioSource.volume = soundVolume;
        }

        public void Mute(bool isMute)
        {
            AudioSource.mute = isMute;
        }

        public void Stop()
        {
            AudioSource.Stop();
            IsPausing = false;
        }

        public override void OnRent()
        {

        }

        public override void OnReturn()
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }

            _audioSource.clip = null;
            IsPausing = false;
        }
    }
}

