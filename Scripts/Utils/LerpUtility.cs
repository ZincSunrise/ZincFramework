using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using ZincFramework.Threading.Tasks;



namespace ZincFramework
{
    public static class LerpUtility
    {
        /// <summary>
        /// 改变颜色的协程
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="callback"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public static async ZincTask UpdateColor(Color origin, Color target, float updateTime, UnityAction<Color> callback, UnityAction<Color> endCallback = null)
        {
            Color startColor;
            for (float i = 0; i < 1 * updateTime; i += Time.deltaTime)
            {
                startColor = Color.Lerp(origin, target, i / updateTime);
                callback?.Invoke(startColor);
                await ZincTask.Yield();
            }

            startColor = target;    
            callback?.Invoke(startColor);
            endCallback?.Invoke(startColor);
        }

        public static async ZincTask LerpValue(float originValue, float targetValue, float lerpTime, bool isRealTime, UnityAction<float> callback, UnityAction endCallback = null, CancellationToken cancellationToken = default)
        {
            float nowValue;
            for (float i = 0; i < 1 * lerpTime; i += isRealTime ? Time.unscaledDeltaTime : Time.deltaTime)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                nowValue = Mathf.Lerp(originValue, targetValue, i / lerpTime);
                callback?.Invoke(nowValue);
                await ZincTask.Yield();
            }

            nowValue = targetValue;
            callback?.Invoke(nowValue);
            endCallback?.Invoke();
        }

        public static async ZincTask ShowColorAsync(Color color, float showTime, UnityAction<Color> callback, CancellationToken cancellationToken)
        {
            Color nowColor = color;
            float elapsedTime = 0f;

            while (elapsedTime < showTime && !cancellationToken.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                nowColor.a = Mathf.Lerp(color.a, 1, elapsedTime / showTime);
                callback?.Invoke(nowColor);
                await ZincTask.Yield(); // 允许控制权返回到主线程
            }

            nowColor.a = 1;
            callback?.Invoke(nowColor);
        }

        public static async ZincTask FadeColorAsync(Color color, float showTime, UnityAction<Color> callback, CancellationToken cancellationToken)
        {
            Color nowColor = color;
            float elapsedTime = 0f;

            while (elapsedTime < showTime && !cancellationToken.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                nowColor.a = Mathf.Lerp(color.a, 0, elapsedTime / showTime);
                callback?.Invoke(nowColor);
                await ZincTask.Yield();
            }

            nowColor.a = 0;
            callback?.Invoke(nowColor);
        }

        /// <summary>
        /// 默认以Update作为速率
        /// </summary>
        /// <param name="colorA"></param>
        /// <param name="colorB"></param>
        /// <param name="lerpTime"></param>
        /// <param name="callback"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async ZincTask LerpColorAsync(Color colorA, Color colorB, float lerpTime, UnityAction<Color> callback, CancellationToken cancellationToken)
        {
            float elapsedTime = 0f;
            Color nowColor;

            while (elapsedTime < lerpTime && !cancellationToken.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                nowColor = Color.Lerp(colorA, colorB, elapsedTime / lerpTime);
                callback?.Invoke(nowColor);
                await ZincTask.Yield();
            }

            nowColor = colorB;
            callback?.Invoke(colorB);
        }
    }
}

