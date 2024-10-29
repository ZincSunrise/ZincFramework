using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZincFramework.Events;



namespace ZincFramework
{
    namespace UI
    {
        public class ColorCircle : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
        {
            private enum EditType
            {
                None,
                Hue,
                Sature,
            }

            public ZincEvent<Color> OnColorChanged { get; } = new ZincEvent<Color>();

            [SerializeField]
            private Image _imageCircleHandle;

            [SerializeField]
            private Image _imageTriangleHandle;

            [SerializeField]
            private RawImage _imageCircle;

            [SerializeField]
            private Material _svMaterial;

            [SerializeField]
            private RectTransform _circleRect;

            [SerializeField]
            private RectTransform _triangleRect;

            private EditType curType = EditType.None;
            private float currentHue = 0;
            private float currentSat = 1;
            private float currentBright = 1;

            private const float RingLen = 0.27f;

            void Start()
            {
                SetColor(currentHue, currentSat, currentBright);
            }

            public void OnBeginDrag(PointerEventData eventData)
            {

            }
            public void OnEndDrag(PointerEventData eventData)
            {

            }

            public void OnDrag(PointerEventData eventData)
            {
                if (curType == EditType.Hue)
                {    
                    Vector3 clickPos = UIManager.Instance.ScreenToWorldPoint(eventData.position);
                    SetHueThumbPos(GetHueFromClickPos(clickPos));
                }
                else if (curType == EditType.Sature)
                {
                    Vector3 clickPos = UIManager.Instance.ScreenToWorldPoint(eventData.position);
                    GetSVFromClickPos(clickPos, out float s, out float v);
                    SetSatureThumbPos(s, v);
                }
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                Vector3 clickPos = UIManager.Instance.ScreenToWorldPoint(eventData.position);

                if (IsInHueRing(clickPos))
                {
                    // 点中了环形区域
                    curType = EditType.Hue;
                    SetHueThumbPos(GetHueFromClickPos(clickPos));
                }
                else
                {
                    // 如果不是色相，再判定饱和度
                    if (IsInSVTriangle(clickPos))
                    {
                        curType = EditType.Sature;
                        GetSVFromClickPos(clickPos, out float s, out float v);
                        SetSatureThumbPos(s, v);
                    }
                }
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                curType = EditType.None;
            }

            #region SVSet
            private bool IsInSVTriangle(Vector3 clickPos)
            {
                Vector3 localPos = _triangleRect.worldToLocalMatrix.MultiplyPoint(clickPos);
                // a,b,c triangle
                Vector2 t = new Vector2(localPos.x, localPos.y) * 2 / _triangleRect.rect.size;

                Vector3 p = new Vector3(t.x, t.y, 0);
                Vector3 a = new Vector3(-1, 1, 0);
                Vector3 b = new Vector3(1, 1, 0);
                Vector3 c = new Vector3(0, -0.7320508075688773f, 0); // 1-sqrt(3)

                Vector3 ab = (b - a).normalized;
                Vector3 bc = (c - b).normalized;
                Vector3 ca = (a - c).normalized;
                Vector3 ap = (p - a).normalized;
                Vector3 bp = (p - b).normalized;
                Vector3 cp = (p - c).normalized;

                Vector3 d1 = Vector3.Cross(ab, ap);
                Vector3 d2 = Vector3.Cross(bc, bp);
                Vector3 d3 = Vector3.Cross(ca, cp);

                return Vector3.Dot(d1, d2) > 0 && Vector3.Dot(d2, d3) > 0;
            }

            private void GetSVFromClickPos(Vector3 clickPos, out float s, out float v)
            {
                Vector3 localPos = _triangleRect.worldToLocalMatrix.MultiplyPoint(clickPos);
                Vector2 t = new Vector2(localPos.x, localPos.y) / _triangleRect.rect.size + new Vector2(0.5f, 0.5f);

                float sqrt3dv2 = 0.8660254037844386f;
                float oneminus = 1 - sqrt3dv2;

                v = (Mathf.Clamp(t.y, oneminus, 1) - oneminus) / sqrt3dv2;
                float temp = v / 2;
                s = Mathf.Clamp(t.x, 0.5f - temp, 0.5f + temp);
                s = v == 0 ? 0 : (s - 0.5f + temp) / v;

                currentSat = s;
                currentBright = v;

                // Debug.Log($"{currentSat}, {currentBright}");
            }

            private void SetSatureThumbPos(float s, float v)
            {
                float sqrt3dv2 = 0.8660254037844386f;
                float oneminus = 1 - sqrt3dv2;
                float y = v * sqrt3dv2 + oneminus; // [0-1]的坐标
                float width = v;
                float x = s * width + 0.5f - width / 2; // [0-1]的坐标

                Vector2 pos = new Vector2(x, y) * _triangleRect.rect.size - _triangleRect.rect.size / 2;

                // Debug.Log($"{x}, {y}, {pos.x}, {pos.y}");
                _imageTriangleHandle.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                UpdatePickColor();
            }
            #endregion

            #region HueSet
            private bool IsInHueRing(Vector3 clickPos)
            {
                Vector3 localPos = _circleRect.worldToLocalMatrix.MultiplyPoint(clickPos);
                // 判定是在色相的环里，还是饱和度的三角形里
                Vector2 dir = new Vector2(localPos.x, localPos.y) - _circleRect.rect.center;
                Vector2 d = new Vector2(dir.x * 2 / _circleRect.rect.width, dir.y * 2 / _circleRect.rect.height);
                float r = d.magnitude;
                return r >= (1 - RingLen) && r <= 1;
            }

            private float GetHueFromClickPos(Vector3 clickPos)
            {
                Vector3 localPos = _circleRect.worldToLocalMatrix.MultiplyPoint(clickPos);
                Vector2 dir = new Vector2(localPos.x, localPos.y) - _circleRect.rect.center;
                float theta = Mathf.Acos(Vector2.Dot(dir.normalized, Vector2.right));
                if (dir.y < 0)
                {
                    theta = 2 * Mathf.PI - theta;
                }
                currentHue = theta / 2 / Mathf.PI;

                return currentHue;
            }


            private void UpdatePickColor()
            {
                Color color = Color.HSVToRGB(currentHue, currentSat, currentBright);
                OnColorChanged?.Invoke(color);
            }

            private void SetHueThumbPos(float h)
            {
                float theta = currentHue * Mathf.PI * 2;
                float x = Mathf.Cos(theta);
                float y = Mathf.Sin(theta);
                Vector2 dir = new Vector2(x, y);
                dir = (1 - RingLen * 0.5f) * dir.normalized;
                Vector2 pos = new Vector2(dir.x * _circleRect.rect.width / 2, dir.y * _circleRect.rect.height / 2);

                _imageCircleHandle.transform.localPosition = new Vector3(pos.x, pos.y, 0);

                _svMaterial.SetFloat("_Hue", currentHue * 360);
                UpdatePickColor();
            }
            #endregion


            public void SetColor(Color color)
            {
                Color.RGBToHSV(color, out var h, out var s, out var v);
                SetColor(h, s, v);
            }

            public void SetColor(float h, float s, float v)
            {
                if(h == currentHue && s == currentSat && v == currentBright)
                {
                    return;
                }

                currentHue = h;
                currentSat = s;
                currentBright = v;
                // update ui position and color
                SetHueThumbPos(currentHue);
                SetSatureThumbPos(currentSat, currentBright);

                UpdatePickColor();
            }

            public Color GetColor()
            {
                return Color.HSVToRGB(currentHue, currentSat, currentBright);
            }
        }
    }
}