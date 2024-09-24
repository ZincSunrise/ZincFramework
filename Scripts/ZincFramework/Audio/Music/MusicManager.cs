using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZincFramework.Load;



namespace ZincFramework
{
    public enum E_Sound_Mode
    {
        TwoD,
        ThreeD,
    }

    /// <summary>
    /// ��Ч����ű������Ը��ĳ����߼�
    /// </summary>
    public class MusicManager : BaseAutoMonoSingleton<MusicManager>
    {
        public bool IsHaveMusic => _musicSource.clip != null;

        public bool IsPlaying => _musicSource.isPlaying;

        public bool IsHiding { get; private set; }

        private float FadePersent => FrameworkConsole.Instance.SharedData.fadePersent;

        private float _musicVolume = 0.5f;
        private string _loadName = "Music";

        private AudioSource _musicSource;
        private Queue<AudioClip> _musicQueue = new Queue<AudioClip>();


        private void Awake()
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.clip = null;
        }

        /// <summary>
        /// �����ı����ֵķ���
        /// �����ʹ��AssetBundle���أ���ʹ����չ����
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="isFade">�Ƿ񵭳�</param>
        /// <param name="offsetTime">�����������µ����µ�����</param>
        public void ChangeMusic(string name, bool isFade, int offsetTime = 0)
        {
            if (isFade && !IsHiding)
            {
                StartCoroutine(R_ChangeMusic(offsetTime, () =>
                {
                    AssetBundleManager.Instance.LoadAssetAsync<AudioClip>(_loadName, name, ChangeClip);
                }));
            }
            else
            {
                AssetBundleManager.Instance.LoadAssetAsync<AudioClip>(_loadName, name, ChangeClip);
            }
        }

        public IEnumerator R_ChangeMusic(int offsetTime, UnityAction callback)
        {
            yield return StartCoroutine(FadeMusic((audio) => { audio.Pause(); }));

            if (offsetTime > 0)
            {
                yield return new WaitForSeconds(offsetTime);
            }

            callback?.Invoke();
        }

        public void ChangeMusicVolume(float volume)
        {
            if (_musicSource != null)
            {
                _musicVolume = volume;
                _musicSource.volume = volume;
            }
        }

        public void ChangeClip(AudioClip cilp)
        {
            if (cilp != null)
            {
                _musicSource.clip = cilp;
                _musicSource.volume = _musicVolume;
                _musicSource.Play();
            }
        }

        public void PlayMusic(bool isFade)
        {
            if (isFade && !IsHiding)
            {
                _musicSource.Play();
                MonoManager.Instance.StartCoroutine(ShowMusic());
            }
            else if (isFade)
            {
                _musicSource.volume = _musicVolume;
                _musicSource.Play();
            }
        }

        public void PauseMusic(bool isFade)
        {
            if (isFade && !IsHiding)
            {
                StartCoroutine(FadeMusic((audio) => audio.Pause()));
            }
            else if (isFade)
            {
                _musicSource.Pause();
            }
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public void MuteMusic(bool isMute)
        {
            if (_musicSource != null)
            {
                _musicSource.mute = isMute;
            }
        }

        private IEnumerator FadeMusic(UnityAction<AudioSource> callback)
        {
            IsHiding = true;
            float wholeVolume = _musicSource.volume;
            while (_musicSource.volume > 0)
            {
                _musicSource.volume -= wholeVolume * FadePersent * Time.deltaTime;
                yield return null;
            }
            callback.Invoke(_musicSource);
            IsHiding = false;
        }

        private IEnumerator ShowMusic()
        {
            IsHiding = true;
            float wholeVolume = _musicSource.volume;
            if (wholeVolume == 0)
            {
                wholeVolume = 0.2f;
            }
            while (_musicSource.volume < _musicVolume)
            {
                _musicSource.volume += wholeVolume * FadePersent * Time.deltaTime;
                yield return null;
            }
            IsHiding = false;
        }
    }
}

