using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



namespace ZincFramework.DialogueSystem
{
    public class TextSpilter
    {
        public List<string> Dialogues { get; } = new List<string>();

        public Text[] Texts { get; }

        public int Count => _count;

        private Text TextHelper => Texts[0];

        private readonly TextGenerator _textGenerator;

        private readonly float _maxLineLength;
        private int _count;

        public TextSpilter(Text[] texts, float maxLineLength)
        {
            Texts = texts;
            _maxLineLength = maxLineLength;
            _textGenerator = new TextGenerator();
        }

        public void SliceText(string str)
        {
            TextHelper.rectTransform.sizeDelta = new Vector2(_maxLineLength, TextHelper.rectTransform.sizeDelta.y);
            TextHelper.text = str;

            var setting = TextHelper.GetGenerationSettings(TextHelper.rectTransform.rect.size);
            setting.horizontalOverflow = HorizontalWrapMode.Wrap;
            setting.verticalOverflow = VerticalWrapMode.Overflow;
            
            _textGenerator.Populate(TextHelper.text, setting);
            _count = _textGenerator.lines.Count;
            float y = TextHelper.rectTransform.sizeDelta.y;


            for (int i = 0; i < _textGenerator.lineCount; i++)
            {
                UILineInfo lineInfo = _textGenerator.lines[i];
                
                int startIndex = lineInfo.startCharIdx;
                int endIndex = i == _textGenerator.lineCount - 1 ? str.Length : _textGenerator.lines[i + 1].startCharIdx;
                string line = str[startIndex..endIndex];

                Dialogues.Add(line);
                TextHelper.text = line;
                Texts[i].rectTransform.sizeDelta = new Vector2(TextHelper.preferredWidth, y);
            }

            TextHelper.text = string.Empty;
        }

        public void SetText(int index, string text)
        {
            Texts[index].text = text;
        }
    }
}