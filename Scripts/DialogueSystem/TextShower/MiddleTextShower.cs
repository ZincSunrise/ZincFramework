using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ZincFramework.Events;



namespace ZincFramework.DialogueSystem
{
    /// <summary>
    /// 类似星铁的文字显示，较消耗性能
    /// </summary>
    public class MiddleTextShower : ITextShower
    {
        public bool IsShowingText { get; private set; }

        public event ZincAction OnTextEnd;

        private readonly WaitForSecondsRealtime _dialogueOffset;

        private readonly float _defaultOffset;

        private int _nowIndex;

        private Coroutine _showTextCoroutine;

        private readonly TextSpilter _textSpliter;

        public MiddleTextShower(RectTransform textContainer, float defaultOffset, float maxLineLength, ZincAction onTextEnd)
        {
            _textSpliter = new TextSpilter(CreateText(textContainer, maxLineLength), maxLineLength);
            _dialogueOffset = new WaitForSecondsRealtime(defaultOffset);

            _defaultOffset = defaultOffset;
            OnTextEnd += onTextEnd;
        }

        public void ShowTextAsync(string text, ZincAction OnShowComplete = null, float showOffset = -1)
        {
            _textSpliter.SliceText(text);

            if (showOffset == 0)
            {
                _textSpliter.Texts[0].text = text;
                return;
            }
            else if (showOffset != -1)
            {
                _dialogueOffset.waitTime = showOffset;
            }
            else
            {
                _dialogueOffset.waitTime = _defaultOffset;
            }

            ShowTextInternal(OnShowComplete);
        }

        private void ShowTextInternal(ZincAction OnShowComplete)
        {
            _dialogueOffset.Reset();
            _showTextCoroutine = MonoManager.Instance.StartCoroutine(ShowTextCoroutine(OnShowComplete));
        }

        private IEnumerator ShowTextCoroutine(ZincAction OnShowComplete)
        {
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

            EndShowing(OnShowComplete);
        }

        public void CompleteImmidately(ZincAction OnShowComplete = null)
        {
            MonoManager.Instance.StopCoroutine(_showTextCoroutine);

            string text = null;
            for (int i = 0; i < _textSpliter.Count; i++)
            {
                _textSpliter.SetText(i, _textSpliter.Dialogues[i]);
            }

            _nowIndex = text?.Length ?? 0;
            EndShowing(OnShowComplete);
        }

        private void EndShowing(ZincAction OnShowComplete = null)
        {
            _textSpliter.Dialogues.Clear();
            IsShowingText = false;
            _showTextCoroutine = null;
            OnTextEnd?.Invoke();
            OnShowComplete?.Invoke();
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
    }
}