using System;
using UnityEngine;
using ZincFramework.Audio.Internal;
using ZincFramework.Audio.Loop;
using ZincFramework.Events;
using ZincFramework.LoadServices;
using ZincFramework.Loop;
using ZincFramework.Pools;
using ZincFramework.Pools.GameObjects;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.Audio
{
    public class SoundEffectManager : BaseSafeSingleton<SoundEffectManager>, IDisposable
    {
        public bool IsPauseAll { get; private set; }

        public bool IsMuteAll { get; private set; }

        public E_Sound_Mode SoundMode => FrameworkConsole.Instance.SharedData.soundMode;

        private int MaxSoundCount => FrameworkConsole.Instance.SharedData.maxSoundCount;

        public float Volume
        {
            get => _soundEffectVolume;
            set
            {
                _soundEffectVolume = value;
                ApplyAllVolume();
            }
        }


        private float _soundEffectVolume = 0.5f;

        private readonly GameObject _soundRoot;

        private readonly SoundLoopModule _soundLoopModule;
        /// <summary>
        /// 3D声音池
        /// </summary>
        private readonly CyclicPool _threeDSoundPool;

        private readonly DataPool<SoundSource> _sourcePool;

        private SoundEffectManager()
        {
            _soundRoot = new GameObject("SoundRoot");
            GameObject.DontDestroyOnLoad(_soundRoot);

            _threeDSoundPool = new CyclicPool(Resources.Load<GameObject>("Audio/SoundObject"), MaxSoundCount, _soundRoot);
            _sourcePool = new DataPool<SoundSource>(() => new SoundSource());

            _soundLoopModule = new SoundLoopModule();
            ZincLoopSystem.AddModule(_soundLoopModule);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == UnityEditor.PlayModeStateChange.EnteredEditMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    _soundLoopModule.Clear();
                }
            };
#endif
        }

        /// <summary>
        /// 播放音效,建议提前加载音效资源
        /// </summary>
        /// <param name="name">音效在资源系统中的索引键</param>
        /// <param name="loopCount">循环次数，如果填入-1那么就是无限循环</param>
        /// <param name="callback">音效完成后的回调函数</param>
        /// <returns></returns>
        public async ZincTask<SoundHandle> PlaySoundAsync(string name, int loopCount, ZincAction callback = null)
        {
            AudioClip audioClip = await AssetLoadManager.LoadAssetAsync<AudioClip>(name);
            return SetSoundInternal(audioClip, loopCount, callback);
        }

        /// <summary>
        /// 播放音效，必须提前加载后再播放
        /// </summary>
        /// <param name="name">音效在资源系统中的索引键</param>
        /// <param name="loopCount">循环次数，如果填入-1那么就是无限循环</param>
        /// <param name="callback">音效播放完后的回调函数</param>
        public SoundHandle PlaySound(string name, int loopCount, ZincAction callback = null)
        {
            AudioClip audioClip = AssetLoadManager.LoadAsset<AudioClip>(name);
            return SetSoundInternal(audioClip, loopCount, callback);
        }

        #region 播放内部函数
        private SoundHandle SetSoundInternal(AudioClip audioClip, int loopCount, ZincAction onSoundEnd)
        {
            if (loopCount == 0)
            {
                Debug.LogWarning("不可以传入0次循环次数");
                return default;
            }


            ThreeDSound threeDSound = _threeDSoundPool.RentValue() as ThreeDSound;
            if (threeDSound.AudioSource == null)
            {
                threeDSound.Initialize(threeDSound.GetComponent<AudioSource>());
            }

            threeDSound.AudioSource.clip = audioClip;
            threeDSound.Mute(IsMuteAll);
            threeDSound.SetVolume(_soundEffectVolume);

            SoundSource soundSource = _sourcePool.RentValue();
            soundSource.Init(threeDSound);

            if (loopCount == 1)
            {
                _soundLoopModule.RegisterSound(soundSource, onSoundEnd);
            }
            else
            {
                _soundLoopModule.RegisterLoopSound(soundSource, loopCount, onSoundEnd);
            }

            if (IsPauseAll)
            {
                threeDSound.Pause();
            }
            else
            {
                threeDSound.Play();
            }

            return new SoundHandle(soundSource);
        }

        internal void ReturnSound(SoundSource soundSource)
        {
            _threeDSoundPool.ReturnValue(soundSource.SoundBase as ThreeDSound);
            _sourcePool.ReturnValue(soundSource);
        }
        #endregion

        public void MuteAllSound(bool isMute)
        {
            IsMuteAll = isMute;
            var soundSpan = _soundLoopModule.SoundSpan;
            for (int i = 0; i < soundSpan.Length; i++)
            {
                soundSpan[i].SoundSource.Mute(isMute);
            }

            var loopSoundSpan = _soundLoopModule.LoopSoundSpan;
            for (int i = 0; i < loopSoundSpan.Length; i++)
            {
                loopSoundSpan[i].SoundSource.Mute(isMute);
            }
        }

        public void ContiuneAllSound()
        {
            IsPauseAll = false;

            var soundSpan = _soundLoopModule.SoundSpan;
            for (int i = 0; i < soundSpan.Length; i++)
            {
                soundSpan[i].SoundSource.Play();
            }

            var loopSoundSpan = _soundLoopModule.LoopSoundSpan;
            for (int i = 0; i < loopSoundSpan.Length; i++)
            {
                loopSoundSpan[i].SoundSource.Play();
            }
        }

        public void PauseAllSound()
        {
            IsPauseAll = true;

            var soundSpan = _soundLoopModule.SoundSpan;
            for (int i = 0; i < soundSpan.Length; i++)
            {
                soundSpan[i].SoundSource.Pause();
            }

            var loopSoundSpan = _soundLoopModule.LoopSoundSpan;
            for (int i = 0; i < loopSoundSpan.Length; i++)
            {
                loopSoundSpan[i].SoundSource.Pause();
            }
        }

        public void ClearAllSound()
        {
            var soundSpan = _soundLoopModule.SoundSpan;
            var loopSoundSpan = _soundLoopModule.LoopSoundSpan;

            for (int i = 0; i < loopSoundSpan.Length; i++)
            {
                _sourcePool.ReturnValue(loopSoundSpan[i].SoundSource);
            }

            for (int i = 0; i < soundSpan.Length; i++)
            {
                _sourcePool.ReturnValue(soundSpan[i].SoundSource);
            }

            _soundLoopModule.Clear();
            _threeDSoundPool.ReturnAll();
        }

        private void ApplyAllVolume()
        {
            var soundSpan = _soundLoopModule.SoundSpan;
            for (int i = 0; i < soundSpan.Length; i++)
            {
                soundSpan[i].SoundSource.SetVolume(_soundEffectVolume);
            }

            var loopSoundSpan = _soundLoopModule.LoopSoundSpan;
            for (int i = 0; i < loopSoundSpan.Length; i++)
            {
                loopSoundSpan[i].SoundSource.SetVolume(_soundEffectVolume);
            }
        }

        public void Dispose()
        {
            _soundLoopModule.Clear();
            _threeDSoundPool.Dispose();
            _sourcePool.Dispose();
            ZincLoopSystem.RemoveModule(_soundLoopModule);

            GameObject.Destroy(_soundRoot);
            _instance = new Lazy<SoundEffectManager>(() => new SoundEffectManager());
        }
    }
}

