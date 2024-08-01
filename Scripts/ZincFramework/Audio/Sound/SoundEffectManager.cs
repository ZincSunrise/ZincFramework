using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZincFramework.Load;




namespace ZincFramework
{
    namespace Audio
    {
        public class SoundEffectManager : BaseSafeSingleton<SoundEffectManager>
        {
            public float PauseTime { get; private set; }

            public bool IsPause { get; private set; }


            private float _soundEffectVolume = 0.5f;

            private int MaxSoundCount => FrameworkData.Shared.maxSoundCount;

            private float DisappearOffset => FrameworkData.Shared.disappearOffset;

            private E_Sound_Mode _soundMode => FrameworkData.Shared.soundMode;

            private string _loadSoundName = "soundeffect";


            private readonly List<ISoundBase> _loopBases = new List<ISoundBase>();
            private readonly List<ISoundBase> _soundBases = new List<ISoundBase>();


            private SoundEffectManager()
            {
                
            }

            /// <summary>
            /// 用来播放音效的方法
            /// 如果不使用AssetBundle加载，请使用拓展方法
            /// </summary>
            /// <param name="name">音效名字</param>
            /// <param name="isLoop">是否循环</param>
            /// <param name="isAsync">是否异步</param>
            /// <param name="callback">回调函数</param>
            public void PlaySound(string name, bool isLoop = false, bool isAsync = false, UnityAction<AudioSource> callback = null)
            {
                AssetBundleManager.Instance.LoadAssetAsync<AudioClip>(_loadSoundName, name, (clip) =>
                {
                    SetSoundObject(clip, callback, isLoop);
                }, isAsync);
            }

            internal void SetSoundObject(AudioClip clip, UnityAction<AudioSource> callback, bool isLoop = false)
            {
                ISoundBase soundBase = ISoundBase.GetSound(_soundMode);
                soundBase.Initialize(isLoop, _soundEffectVolume, DisappearOffset, clip);

                if (isLoop)
                {
                    _loopBases.Add(soundBase);
                }
                else
                {
                    _soundBases.Add(soundBase);
                }
                callback?.Invoke(soundBase.AudioSource);
            }

            internal void RemoveSound(ISoundBase soundBase)
            {
                if (soundBase.IsLoop)
                {
                    _loopBases.Remove(soundBase);
                }
                else
                {
                    _soundBases.Remove(soundBase);
                }
                soundBase.Pause();
                soundBase.Refresh();
            }

            public void ChangeSoundEffectVolume(float volume)
            {
                _soundEffectVolume = volume;

                for (int i = 0; i < _loopBases.Count; i++)
                {
                    _loopBases[i].ChangeVolume(volume);
                }

                for (int i = 0; i < _soundBases.Count; i++)
                {
                    _soundBases[i].ChangeVolume(volume);
                }
            }

            public void PauseAllSounds()
            {
                IsPause = true;
                PauseTime = Time.time;

                for (int i = 0; i < _loopBases.Count; i++)
                {
                    _loopBases[i].Pause();
                }
                for (int i = 0; i < _soundBases.Count; i++)
                {
                    _soundBases[i].Pause();
                }
            }

            public void PlayAllSounds()
            {
                for (int i = 0; i < _loopBases.Count; i++)
                {
                    _loopBases[i].Play();
                }
                for (int i = 0; i < _soundBases.Count; i++)
                {
                    _soundBases[i].Play();
                }
                IsPause = false;
            }

            public void RemoveAllSounds()
            {
                for (int i = 0; i < _loopBases.Count; i++)
                {
                    _loopBases[i].Pause();
                    _loopBases[i].Refresh();
                }
                for (int i = 0; i < _soundBases.Count; i++)
                {
                    _loopBases[i].Pause();
                    _loopBases[i].Refresh();
                }

                _soundBases.Clear();
                _loopBases.Clear();
            }
        }
    }
}

