using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using System.Threading.Tasks;
using ZincFramework.LoadServices.Addressable;


namespace ZincFramework.Audio
{
    public static class AddressableAudioExtension
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
        public static void PlaySoundAsync(this SoundEffectManager manager, string name, bool isLoop = false, UnityAction<AudioSource> callback = null)
        {
            AddressablesManager.Instance.LoadAssetAsync<AudioClip>(name, (clip) => manager.SetSoundObject(clip, callback, isLoop));
        }

        public static async void PlaySoundRepeatAsync(this SoundEffectManager manager, string name, int repeatCount, CancellationToken cancellationToken, UnityAction<AudioSource> callback = null)
        {
            int nowCount = 0;
            while (nowCount++ < repeatCount)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var clip = await AddressablesManager.Instance.LoadAssetAsync<AudioClip>(name);
                manager.SetSoundObject(clip, callback, false);
                await Task.Delay(Mathf.CeilToInt(1000 * clip.length), cancellationToken);
            }
        }
    }
}

