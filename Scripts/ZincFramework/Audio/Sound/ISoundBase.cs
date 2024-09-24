using System.Collections.Generic;
using UnityEngine;
using ZincFramework.DataPool;
using ZincFramework.GameObjectPool;

namespace ZincFramework
{
    namespace Audio
    {
        internal interface ISoundBase
        {
            public static Dictionary<string, WaitForSecondsRealtime> allWaits = new Dictionary<string, WaitForSecondsRealtime>();

            public AudioSource AudioSource { get; }

            public bool IsLoop { get; }

            void ChangeVolume(float soundVolume);

            void Play();

            void Pause();

            void Refresh();

            void Initialize(bool isLoop, float volume, float disappearTime, AudioClip audioClip);

            public static ISoundBase GetSound(E_Sound_Mode soundMode)
            {
                if(soundMode == E_Sound_Mode.TwoD)
                {
                    GameObject obj = GameObjectPoolManager.Instance.RentGameObject("Audio/SoundObject");
                    return obj.GetComponent<SoundObject>();
                }
                else if(soundMode == E_Sound_Mode.ThreeD)
                {
                    return DataPoolManager.RentInfo<SoundSource>();
                }

                return null;
            }
        }
    }
}

