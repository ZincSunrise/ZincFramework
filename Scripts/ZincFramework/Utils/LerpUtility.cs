using System.Collections;
using UnityEngine;
using UnityEngine.Events;



namespace ZincFramework
{
    public static class LerpUtility
    {
        /// <summary>
        /// 可用于移动的协程
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="updateTime"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator UpdateVector2(Vector2 origin, Vector2 target, float updateTime, UnityAction<Vector2> callback, UnityAction<Vector2> endCallback = null)
        {
            Vector2 start;
            for (float i = 0; i < 1 * updateTime; i += Time.deltaTime)
            {
                start = Vector2.Lerp(origin, target, i / updateTime);
                callback?.Invoke(start);
                yield return null;
            }

            start = target;
            endCallback?.Invoke(start);
        }

        /// <summary>
        /// 改变颜色的协程
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="callback"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public static IEnumerator UpdateColor(Color origin, Color target, float updateTime, UnityAction<Color> callback, UnityAction<Color> endCallback = null)
        {
            Color startColor;
            for (float i = 0; i < 1 * updateTime; i += Time.deltaTime)
            {
                startColor = Color.Lerp(origin, target, i / updateTime);
                callback?.Invoke(startColor);
                yield return null;
            }

            startColor = target;    
            callback?.Invoke(startColor);
            endCallback?.Invoke(startColor);
        }

        public static IEnumerator LerpValue(float originValue, float targetValue, float lerpTime, UnityAction<float> callback, UnityAction endCallback = null)
        {
            float nowValue;
            for (float i = 0; i < 1 * lerpTime; i += Time.deltaTime)
            {
                nowValue = Mathf.Lerp(originValue, targetValue, i / lerpTime);
                callback?.Invoke(nowValue);
                yield return null;
            }

            nowValue = targetValue;
            callback?.Invoke(nowValue);
            endCallback?.Invoke();
        }

        /// <summary>
        /// 使得颜色隐藏的协程
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fadeTime">隐藏所需的时间</param>
        /// <returns></returns>
        public static IEnumerator FadeColor(Color color, float fadeTime, UnityAction<Color> callback)
        {
            Color nowColor = color;
            for (float i = 0; i < 1 * fadeTime; i += Time.deltaTime)
            {             
                nowColor.a = Mathf.Lerp(color.a, 0, i / fadeTime);
                callback?.Invoke(nowColor);
                yield return null;
            }

            nowColor.a = 0;
            callback?.Invoke(nowColor);
        }

        /// <summary>
        /// 使得颜色显现的协程
        /// </summary>
        /// <param name="color"></param>
        /// <param name="showTime">显现所需的时间</param>
        /// <returns></returns>

        public static IEnumerator ShowColor(Color color, float showTime, UnityAction<Color> callback)
        {
            Color nowColor = color;
            for (float i = 0; i < 1 * showTime; i += Time.deltaTime)
            {
                nowColor.a = Mathf.Lerp(color.a, 1, i / showTime);
                callback?.Invoke(nowColor);
                yield return null;
            }

            nowColor.a = 1;
            callback?.Invoke(nowColor);
        }
    }
}

