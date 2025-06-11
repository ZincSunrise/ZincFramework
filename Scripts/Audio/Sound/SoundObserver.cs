using UnityEngine;
using ZincFramework.DataPools;
using ZincFramework.MonoModel;


namespace ZincFramework.Audio
{
    public class SoundObserver : ISoundBase, IMonoObserver, IReuseable
    {
        private readonly AudioSource _audioSource;

        public AudioSource AudioSource => _audioSource;

        public bool IsLoop => _audioSource.loop;

        public SoundObserver()
        {

        }

        public SoundObserver(AudioSource audioSource) 
        {
            _audioSource = audioSource;
        }

        public void NotifyObserver()
        {
            if (!_audioSource.isPlaying)
            {
                SoundEffectManager.Instance.RemoveSound(this);
                MonoManager.Instance.RemoveFixedUpdateObserver(this);
            }
        }


        public void ChangeVolume(float soundVolume) => _audioSource.volume = soundVolume;

        public void Play() => _audioSource?.Play();

        public void Pause() => _audioSource?.Pause();

        public void Refresh()
        {
            _audioSource.Stop();
        }

        public void Initialize(bool isLoop, float volume, float disappearTime, AudioClip audioClip)
        {
            _audioSource.loop = isLoop;
            _audioSource.volume = volume;
            _audioSource.clip = audioClip;
        }

        public void OnRegist()
        {

        }

        public void OnRemove()
        {

        }

        public void OnReturn()
        {
            _audioSource.Stop();
        }

        public void OnRent()
        {

        }
    }
}