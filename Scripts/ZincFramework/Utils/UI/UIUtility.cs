using UnityEngine;


namespace ZincFramework
{
    namespace UI
    {
        public static class UIUtility
        {
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


