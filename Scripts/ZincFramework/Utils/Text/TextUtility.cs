using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using ZincFramework.DataPool;
using ZincFramework.Events;


namespace ZincFramework
{
    public static partial class TextUtility
    {
        private static readonly StringBuilder _result = new StringBuilder(30);

        private static readonly StringBuilder _dialogueText = new StringBuilder(30);


        //提取出字符串开头的数字部分
        public static int SubHeadNumber(string str)
        {
            int index = 0;
            if (!char.IsNumber(str[index]) && str[index++] != '-')
            {
                return int.MinValue;
            }

            while (char.IsNumber(str[index]))
            {
                index++;
            }

            ReadOnlySpan<char> chars = str;
            return int.TryParse(chars[..index], out var result) ? result : int.MaxValue;
        }

        public static Coroutine ShowTextStep(string showText, float offset, ZincAction<string> callback, ZincAction endCallback = null)
        {
            return MonoManager.Instance.StartCoroutine(ShowTextStep(showText, offset, callback, endCallback));

            IEnumerator ShowTextStep(string showText, float offset, ZincAction<string> callback, ZincAction endCallback)
            {
                ReuseableWait reuseableWait = DataPoolManager.RentInfo(() => new ReuseableWait(offset));

                for (int i = 0; i < showText.Length; i++)
                {
                    callback?.Invoke(showText[..i]);
                    yield return reuseableWait.WaitForSeconds;
                }

                callback?.Invoke(showText);
                DataPoolManager.ReturnInfo<ReuseableWait>(reuseableWait);
                endCallback?.Invoke();
            }
        }


        public static IEnumerator SliceTextStepRealTime(string showText, float offset, ZincAction<string> callback, ZincAction endCallback = null)
        {
            ReuseableRealWait reuseableRealWait = DataPoolManager.RentInfo(() => new ReuseableRealWait(offset));

            for (int i = 0; i < showText.Length; i++)
            {
                callback?.Invoke(showText[..i]);
                yield return reuseableRealWait.WaitForSecondsRealtime;
            }

            DataPoolManager.ReturnInfo<ReuseableRealWait>(reuseableRealWait);
            endCallback?.Invoke();
        }

        public static string UpperFirstChar(string value)
        {
            if (_result.Length != 0)
            {
                _result.Clear();
            }

            _result.Append(value);
            _result[0] = char.ToUpper(_result[0]);

            return _result.ToString();
        }

        public static string LowerFirstChar(string value)
        {
            if (_result.Length != 0)
            {
                _result.Clear();
            }

            _result.Append(value);
            _result[0] = char.ToLower(_result[0]);

            return _result.ToString();
        }

        public static string[] Split(string str, char sparator, bool isLocalize = true)
        {
            if (isLocalize)
            {
                str = LocalizeSymbols(str, sparator);
            }
            return str.Split(sparator);
        }

        public static string[] Split(string str, string sparator, bool isLocalize = true)
        {
            for (int i = 0; i < sparator.Length; i++)
            {
                if (isLocalize)
                {
                    str = LocalizeSymbols(str, sparator[i]);
                }
            }
            return str.Split(sparator);
        }


        /// <summary>
        /// 直接将字符串转换为int数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static int[] SplitToIntArray(string str, char character)
        {
            string[] strs = Split(str, character);

            if (strs.Length == 0)
            {
                return Array.Empty<int>();
            }

            return Array.ConvertAll(strs, (str) => int.Parse(str));
        }

        /// <summary>
        /// 给int值补零的方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length">输出多少位，没超过就会在前面补0，如果超过该长度则输出原来的value</param>
        /// <returns></returns>
        public static string IntToStringWithBehindZero(int value, int length)
        {
            return value.ToString($"D{length}");
        }

        /// <summary>
        /// 不四舍五入保留float的小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalNumber">保留几位小数的长度</param>
        /// <returns></returns>
        public static string KeepDecimalPlace(float value, int decimalNumber)
        {
            return value.ToString($"F{decimalNumber}");
        }

        public static string BigNumberToString(long number, string units1 = "万", string units2 = "亿")
        {
            _result.Clear();
            Span<char> chars = stackalloc char[16];

            if (number > 1e8)
            {
                (number / (long)1e8).TryFormat(chars, out int written);
                _result.Append(chars[..written]);
                _result.Append(units2);

                if (number / 1e5 != 0)
                {
                    (number / 10000 % 10000).TryFormat(chars, out written);
                    _result.Append(chars[..written]);
                    _result.Append(units1);
                }
            }
            else if (number > 1e5)
            {
                (number / (long)1e5).TryFormat(chars, out int written);
                _result.Append(chars[..written]);
                _result.Append(units1);
            }

            return _result.ToString();
        }

        public static string IntToTime(int seconds, string secondUnit = "秒", string minuteUnit = "分", string hourUnit = "时")
        {
            _result.Clear();
            float second = seconds % 60;
            float minute = seconds / 60 % 60;
            float hour = seconds / 3600;
            Span<char> chars = stackalloc char[4];

            int written;
            if (hour > 0)
            {
                hour.TryFormat(chars, out written);
                _result.Append(chars[..written]);
                _result.Append(hourUnit);
            }
;
            if (minute > 0 || hour > 0)
            {
                minute.TryFormat(chars, out written);
                _result.Append(chars[..written]);
                _result.Append(minuteUnit);
            }

            second.TryFormat(chars, out written);
            _result.Append(chars[..written]);
            _result.Append(secondUnit);

            return _result.ToString();
        }

        public static string LocalizeSymbols(string str, char symbols)
        {
            if(_result.Length != 0)
            {
                _result.Clear();
            }

            _result.Append(str);
            switch (symbols)
            {
                case ' ':
                case '\n':
                case '%':
                case '&':
                case '#':
                case '|':
                case '$':
                    break;
                case ',':
                    _result.Replace('，', ',');
                    break;
                case ';':
                    _result.Replace('；', ';');
                    break;
                case ':':
                    _result.Replace('：', ':');
                    break;
                case '?':
                    _result.Replace('？', '?');
                    break;
                case '.':
                    _result.Replace('。', '.');
                    break;
            }

            return _result.ToString();
        }
    }
}

