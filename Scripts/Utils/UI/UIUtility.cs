using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace ZincFramework
{
    namespace UI
    {
        public static class UIUtility
        {
            public static IEnumerator FadeUI(Graphic graphicUI, int fadeTime)
            {
                for (float i = 0; i < 1 * fadeTime; i += Time.deltaTime) 
                {
                    float a = Mathf.Lerp(0, 1, i / fadeTime);
                    Color c = graphicUI.color;
                    graphicUI.color = new Color(c.r, c.g, c.b, a);
                    yield return null;
                }
            }

            public static Vector2 WorldPointToScreen(Camera camera, RectTransform root, Transform point, float xOffset = 0, float yOffset = 0)
            {
                Vector2 viewPoint = camera.WorldToViewportPoint(point.position);
                float x = (viewPoint.x - 0.5f) * root.rect.size.x;
                float y = (viewPoint.y - 0.5f) * root.rect.size.y;
                return new Vector2(x + xOffset, y + yOffset);
            }

            public static Vector2 WorldPointToMainCamera(RectTransform root, Transform point, float xOffset = 0, float yOffset = 0)
            {
                return WorldPointToScreen(Camera.main, root, point, xOffset, yOffset);
            }

            public static Vector2 MainCameraPointToUIRect(RectTransform root, Vector2 point)
            {
                Vector2 persent = Camera.main.ScreenToViewportPoint(point);
                persent = new Vector2(persent.x - 0.5f, persent.y - 0.5f);

                Vector2 localPos = root.rect.size * persent;
                return localPos;
            }
        }
    }
}


