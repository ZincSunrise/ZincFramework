using System.Collections.Generic;
using UnityEngine;
using ZincFramework.LoadServices;
using ZincFramework.Threading.Tasks;



namespace ZincFramework.Audio
{
    public enum E_Sound_Mode
    {
        TwoD,
    }

    /// <summary>
    /// 背景音乐管理
    /// </summary>
    public class MusicManager : BaseAutoMonoSingleton<MusicManager>
    {
        public bool IsHaveMusic => _musicSource.clip != null;

        public bool IsPlaying => _musicSource.isPlaying;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                if (_musicSource != null)
                {
                    _musicSource.volume = _volume;
                }
            }
        }

        private float _volume = 0.5f;

        private AudioSource _musicSource;

        private readonly Queue<AudioClip> _musicQueue = new Queue<AudioClip>();

        private void Awake()
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.clip = null;
        }

        public async ZincTask EqueueMusicAsync(string name, float fadeTime = 0)
        {
            AudioClip audioClip = await AssetLoadManager.LoadAssetAsync<AudioClip>(name);
            if (_musicQueue.Count > 0)
            {
                _musicQueue.Enqueue(audioClip);
            }
            else
            {
                ChangeMusic(audioClip);
            }
        }

        public void ChangeMusic(AudioClip audioClip)
        {
            _musicSource.clip = audioClip;
            _musicSource.Play();
        }
    }
}

