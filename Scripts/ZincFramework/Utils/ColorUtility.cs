using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ZincFramework
{
    public static class ColorUtility
    {
        /// <summary>
        /// 改变颜色的协程
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="callback"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static IEnumerator ChangeColor(Color origin, Color target, UnityAction<Color> callback, float speed)
        {
            while (origin.r < target.r)
            {
                origin = Color.Lerp(origin, target, Time.deltaTime * speed);
                callback.Invoke(origin);
                yield return null;
            }
        }

        /// <summary>
        /// 使得颜色隐藏的协程
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fadeSpeed"></param>
        /// <param name="type">1代表减去一个常数，2代表线性插值</param>
        /// <returns></returns>
        public static IEnumerator FadeColor(Color color, float fadeSpeed, UnityAction<Color> callback, int type = 1, int smoothTime = 0)
        {
            switch (type)
            {
                case 1:
                    while (color.a > 0)
                    {
                        color.a -= Time.deltaTime * fadeSpeed;
                        if (color.a < 0)
                        {
                            color.a = 0;
                        }
                        callback.Invoke(color);
                        yield return null;
                    }
                    break;
                case 2:
                    while (color.a > 0)
                    {
                        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * fadeSpeed);
                        if (color.a < 0.03f)
                        {
                            color.a = 0;
                        }
                        callback.Invoke(color);
                        yield return null;
                    }
                    break;
                case 3:
                    float speed = fadeSpeed;
                    while (color.a > 0)
                    {
                        color.a = Mathf.SmoothDamp(color.a, 0, ref speed, smoothTime);
                        if (color.a < 0.03f)
                        {
                            color.a = 0;
                        }
                        callback.Invoke(color);
                        yield return null;
                    }
                    break;
            }
        }

        /// <summary>
        /// 使得颜色显现的协程
        /// </summary>
        /// <param name="color"></param>
        /// <param name="showSpeed"></param>
        /// <param name="type">1代表减去一个常数，2代表线性插值</param>
        /// <returns></returns>

        public static IEnumerator ShowColor(Color color, float showSpeed, UnityAction<Color> callback, int smoothTime = 0, int type = 1)
        {
            switch (type)
            {
                case 1:
                    while (color.a < 1)
                    {
                        color.a += Time.deltaTime * showSpeed;
                        if (color.a > 1)
                        {
                            color.a = 1;
                        }
                        callback.Invoke(color);
                        yield return null;
                    }
                    break;
                case 2:
                    while (color.a < 1)
                    {
                        color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * showSpeed);
                        if (color.a > 0.98f)
                        {
                            color.a = 1;
                        }
                        callback.Invoke(color);
                        yield return null;
                    }
                    break;
                case 3:
                    float speed = showSpeed;
                    while (color.a < 1)
                    {
                        color.a = Mathf.SmoothDamp(color.a, 1, ref speed, smoothTime);
                        if (color.a < 0.03f)
                        {
                            color.a = 0;
                        }
                        callback.Invoke(color);
                        yield return null;
                    }
                    break;
            }
        }
    }
}

