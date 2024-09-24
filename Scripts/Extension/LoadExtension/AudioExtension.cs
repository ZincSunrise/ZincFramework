using UnityEngine;
using UnityEngine.Events;
using ZincFramework.Audio;
using ZincFramework.Load.Extension;


namespace ZincFramework
{
    namespace AudioExtension
    {
        public static class AudioExtension
        {
            public static void ChangeMusic(this MusicManager manager, string name, bool isFade = false, int offsetTime = 0)
            {
                if (isFade && !manager.IsHiding)
                {
                    MonoManager.Instance.StartCoroutine(manager.R_ChangeMusic(offsetTime, () => AddressablesManager.Instance.LoadAssetAsync<AudioClip>(name, manager.ChangeClip)));
                }
                else
                {
                    AddressablesManager.Instance.LoadAssetAsync<AudioClip>(name, manager.ChangeClip);
                }
            }
            public static void PlaySound(this SoundEffectManager manager, string name, bool isLoop = false, UnityAction<AudioSource> callback = null)
            {
                AddressablesManager.Instance.LoadAssetAsync<AudioClip>(name, (clip) => manager.SetSoundObject(clip, callback, isLoop));
            }
        }
    }
}

