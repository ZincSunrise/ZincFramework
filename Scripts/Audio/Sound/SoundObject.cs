using UnityEngine;
using System.Collections;
using ZincFramework.Pool.GameObjects;


namespace ZincFramework.Audio
{
    internal class SoundObject : ReuseableObject, ISoundBase
    {
        public AudioSource AudioSource => _audioSource;
        public bool IsLoop => _audioSource.loop;

        private Coroutine _waitEnd;

        private AudioSource _audioSource;

        private float _bornTime;
        private float _lifeTime;
        private WaitForSecondsRealtime _sleepTime;

        public void Initialize(bool isLoop, float volume, float disappearTime, AudioClip audioClip)
        {
            if (!TryGetComponent(out _audioSource))
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (!ISoundBase.allWaits.TryGetValue(audioClip.name, out _sleepTime))
            {
                _sleepTime = new WaitForSecondsRealtime(audioClip.length + disappearTime);
                ISoundBase.allWaits.Add(audioClip.name, _sleepTime);
            }

            _sleepTime.Reset();

            _bornTime = Time.time;
            _lifeTime = _sleepTime.waitTime;

            _audioSource.loop = isLoop;
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.Play();

            _waitEnd = StartCoroutine(WaitMusicEnd(_sleepTime));
        }


        public void Pause()
        {
            Debug.Log("µ˜”√¡ÀÕ£÷π");
            _audioSource.Pause();
            StopCoroutine(_waitEnd);
        }

        public void Play()
        {
            float lastTime = _lifeTime - (SoundEffectManager.Instance.PauseTime - _bornTime);

            if (lastTime < 0.05f)
            {
                lastTime = 0.1f;
            }

            _audioSource.Play();

            if (!_audioSource.loop)
            {
                _waitEnd = StartCoroutine(WaitMusicEnd(new WaitForSecondsRealtime(lastTime)));
            }
        }

        private IEnumerator WaitMusicEnd(WaitForSecondsRealtime wait)
        {
            yield return wait;
            if (_audioSource.loop)
            {
                yield break;
            }
            SoundEffectManager.Instance.RemoveSound(this);
        }

        public void ChangeVolume(float soundVolume)
        {
            _audioSource.volume = soundVolume;
        }

        public override void OnRent()
        {

        }

        public override void OnReturn()
        {
            Refresh();
        }

        public void Refresh()
        {
            _audioSource.clip = null;
            _lifeTime = -1;
            _bornTime = -1;

            if (_waitEnd != null)
            {
                StopCoroutine(_waitEnd);
            }
        }
    }
}

