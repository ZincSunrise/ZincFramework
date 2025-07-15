using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZincFramework.Events;
using ZincFramework.Loop;



namespace ZincFramework.DialogueSystem
{
    /// <summary>
    /// 类似星铁的文字显示，较消耗性能
    /// </summary>
    public class MiddleTextShower : MonoBehaviour, ITextShower
    {
        public bool IsShowingText { get; private set; }

        public ZincAction OnTextEnd { get; set; }

        public ZincAction OnTextBegin { get; set; }

        public event ZincAction<int> OnTextShowing;

        private int _nowIndex;

        private WaitForSecondsRealtime _dialogueOffset;

        private Coroutine _showTextCoroutine;

        private TextSpilter _textSpliter;

        [SerializeField]
        private RectTransform _textContainer;

        [SerializeField]
        private float _maxLineLength;

        [SerializeField]
        private float _defaultOffset;

        private void Awake()
        {
            _textSpliter = new TextSpilter(CreateText(_textContainer, _maxLineLength), _maxLineLength);
            _dialogueOffset = new WaitForSecondsRealtime(_defaultOffset);
        }


        public void ShowTextAsync(string name, string text)
        {
            _textSpliter.SliceText(text);

            if (_defaultOffset == 0)
            {
                _textSpliter.Texts[0].text = text;
                return;
            }
            else
            {
                _dialogueOffset.waitTime = _defaultOffset;
            }

            ShowTextInternal();
        }

        private void ShowTextInternal()
        {
            _dialogueOffset.Reset();
            _showTextCoroutine = ZincLoopSystem.StartCoroutine(nameof(MiddleTextShower), ShowTextCoroutine());
        }

        private IEnumerator ShowTextCoroutine()
        {
            OnTextBegin?.Invoke();
            IsShowingText = true;
            string text;

            for(int i = 0; i < _textSpliter.Count; i++)
            {
                text = _textSpliter.Dialogues[i];
                for (_nowIndex = 0; _nowIndex < text.Length; _nowIndex++)
                {
                    _textSpliter.Texts[i].text = text[.._nowIndex];
                    yield return _dialogueOffset;
                }

                _textSpliter.SetText(i, text);
            }

            EndShowing();
        }

        public void CompleteImmidately()
        {
            ZincLoopSystem.StopCoroutine(nameof(MiddleTextShower));

            string text = null;
            for (int i = 0; i < _textSpliter.Count; i++)
            {
                _textSpliter.SetText(i, _textSpliter.Dialogues[i]);
            }

            _nowIndex = text?.Length ?? 0;
            EndShowing();
        }

        private void EndShowing()
        {
            _textSpliter.Dialogues.Clear();
            IsShowingText = false;
            _showTextCoroutine = null;
            OnTextEnd?.Invoke();
        }

        private Text[] CreateText(RectTransform textContainer, float maxLineLength)
        {
            GameObject prefab = Resources.Load<GameObject>("TextDialogue");
            float y = prefab.GetComponent<Text>().rectTransform.sizeDelta.y;

            int count = Mathf.CeilToInt(textContainer.sizeDelta.y / y);
            Text[] textDialogues = new Text[count];

            for (int i = 0; i < textDialogues.Length; i++)
            {
                GameObject gameObject = GameObject.Instantiate(prefab, textContainer.transform);

                textDialogues[i] = gameObject.GetComponent<Text>();

                RectTransform rectTransform = textDialogues[i].rectTransform;
                rectTransform.sizeDelta = new Vector2(maxLineLength, rectTransform.sizeDelta.y);
                rectTransform.anchorMax = rectTransform.anchorMin = rectTransform.pivot = new Vector2(0.5f, 1);
                rectTransform.anchoredPosition = new Vector2(0, -y * i);
                textDialogues[i].alignment = TextAnchor.MiddleLeft;
            }

            return textDialogues;
        }

        public void StartShowText()
        {
            throw new System.NotImplementedException();
        }

        public void EndShowText()
        {
            throw new System.NotImplementedException();
        }

        public void SkipText()
        {
            throw new System.NotImplementedException();
        }

        public void SetRoleSprite(string name)
        {
            throw new System.NotImplementedException();
        }

        public void SetRoleSprite(Sprite sprite)
        {
            throw new System.NotImplementedException();
        }
    }
}