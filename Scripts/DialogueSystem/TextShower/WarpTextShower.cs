using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ZincFramework.Events;


namespace ZincFramework.DialogueSystem
{
    /// <summary>
    /// 普通galgame的文字显示
    /// </summary>
    public class WarpTextShower : ITextShower
    {
        public bool IsShowingText { get; private set; }

        public event ZincAction OnTextEnd;

        private readonly Text _textDialogue;

        private readonly WaitForSecondsRealtime _dialogueOffset;

        private readonly float _defaultOffset;

        private int _nowIndex;

        private Coroutine _showTextCoroutine;

        private string _nowShowingText;


        public WarpTextShower(RectTransform rectTransform, float defaultOffset, ZincAction onTextEnd)
        {
            GameObject prefab = Resources.Load<GameObject>("TextDialogue");
            GameObject gameObject = GameObject.Instantiate(prefab, rectTransform);

            _textDialogue = gameObject.GetComponent<Text>();
            _textDialogue.rectTransform.pivot = _textDialogue.rectTransform.anchorMax = _textDialogue.rectTransform.anchorMin = Vector2.one / 2;
            _textDialogue.rectTransform.sizeDelta = rectTransform.sizeDelta;
            _textDialogue.rectTransform.anchoredPosition = Vector2.zero;
            _textDialogue.alignment = TextAnchor.UpperLeft;

            _defaultOffset = defaultOffset;
            _dialogueOffset = new WaitForSecondsRealtime(defaultOffset);
            OnTextEnd += onTextEnd;
        }

        public void ShowTextAsync(string text, ZincAction OnShowComplete = null, float showOffset = -1)
        {
            if (showOffset == 0)
            {
                _textDialogue.text = text;
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

            ShowTextInternal(text, OnShowComplete);
        }

        private void ShowTextInternal(string text, ZincAction OnShowComplete)
        {
            _nowShowingText = text;
            _dialogueOffset.Reset();
            _showTextCoroutine = MonoManager.Instance.StartCoroutine(ShowTextCoroutine(OnShowComplete));
        }


        private IEnumerator ShowTextCoroutine(ZincAction OnShowComplete)
        {
            IsShowingText = true;

            for (_nowIndex = 0; _nowIndex < _nowShowingText.Length; _nowIndex++)
            {
                _textDialogue.text = _nowShowingText[.._nowIndex];
                yield return _dialogueOffset;
            }

            _textDialogue.text = _nowShowingText;
            IsShowingText = false;

            OnTextEnd?.Invoke();
            OnShowComplete?.Invoke();
        }


        public void CompleteImmidately(ZincAction OnShowComplete = null)
        {
            IsShowingText = false;

            _textDialogue.text = _nowShowingText;
            MonoManager.Instance.StopCoroutine(_showTextCoroutine);

            _nowShowingText = string.Empty;


            _nowIndex = _nowShowingText.Length - 1;
            _showTextCoroutine = null;

            OnTextEnd?.Invoke();
            OnShowComplete?.Invoke();
        }
    }
}