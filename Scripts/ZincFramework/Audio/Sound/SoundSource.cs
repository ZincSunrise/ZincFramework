using System.Collections;
using UnityEngine;
using ZincFramework.DataPool;



namespace ZincFramework
{
    namespace Audio
    {
        internal class SoundSource : ISoundBase, IResumable
        {
            public bool IsLoop => _audioSource.loop;
            public AudioSource AudioSource => _audioSource;

            private AudioSource _audioSource;
            private Coroutine _waitEnd;
            private float _bornTime;
            private float _lifeTime;
            private WaitForSecondsRealtime _sleepTime;


            public void Initialize(bool isLoop, float volume, float disappearTime, AudioClip audioClip)
            {
                if (_waitEnd != null)
                {
                    MonoManager.Instance.StopCoroutine(_waitEnd);
                }

                _audioSource = ComponentPoolManager.Instance.RentComponent<AudioSource>();

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
                _waitEnd = MonoManager.Instance.StartCoroutine(WaitMusicEnd(_sleepTime));
            }

            public void Pause()
            {
                _audioSource.Pause();
                MonoManager.Instance.StopCoroutine(_waitEnd);
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
                    _waitEnd = MonoManager.Instance.StartCoroutine(WaitMusicEnd(new WaitForSecondsRealtime(lastTime)));
                }
            }

            private IEnumerator WaitMusicEnd(WaitForSecondsRealtime wait)
            {
                yield return wait;
                if (_audioSource.loop)
                {
                    yield break;
                }

                yield return 1;
                SoundEffectManager.Instance.RemoveSound(this);
            }

            public void Refresh()
            {
                _audioSource.clip = null;
                ComponentPoolManager.Instance.ReturnComponent(_audioSource);
                DataPoolManager.ReturnInfo(this);
                _lifeTime = -1;
                _bornTime = -1;
                MonoManager.Instance.StopCoroutine(_waitEnd);
            }

            public void ChangeVolume(float soundVolume)
            {
                _audioSource.volume = soundVolume;
            }


            public void OnReturn()
            {
                _audioSource = null;
                _sleepTime = null;
                _waitEnd = null;
            }

            public void OnRent()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

