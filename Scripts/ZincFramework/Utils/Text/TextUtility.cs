using System;
using System.Text;


namespace ZincFramework
{
    public static partial class TextUtility
    {
        private static readonly StringBuilder _result = new StringBuilder(30);

        public static string UpperFirstString(string value)
        {
            if(_result.Length != 0)
            {
                _result.Clear();
            }

            _result.Append(value);
            _result[0] = char.ToUpper(_result[0]);

            return _result.ToString();
        }

        public static string LowerFirstString(string value)
        {
            if (_result.Length != 0)
            {
                _result.Clear();
            }

            _result.Append(value);
            char.ToLower(_result[0]);

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
        /// ֱ�ӽ��ַ���ת��Ϊint����
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
        /// ��intֵ����ķ���
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length">�������λ��û�����ͻ���ǰ�油0����������ó��������ԭ����value</param>
        /// <returns></returns>
        public static string IntToStringWithBehindZero(int value, int length)
        {
            return value.ToString($"D{length}");
        }

        /// <summary>
        /// ���������뱣��float��С��
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalNumber">������λС���ĳ���</param>
        /// <returns></returns>
        public static string KeepDecimalPlace(float value, int decimalNumber)
        {
            return value.ToString($"F{decimalNumber}");
        }

        public static string BigNumberToString(long number, string units1 = "��", string units2 = "��")
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

        public static string IntToTime(int seconds, string secondUnit = "��", string minuteUnit = "��", string hourUnit = "ʱ")
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
                    _result.Replace('��', ',');
                    break;
                case ';':
                    _result.Replace('��', ';');
                    break;
                case ':':
                    _result.Replace('��', ':');
                    break;
                case '?':
                    _result.Replace('��', '?');
                    break;
                case '.':
                    _result.Replace('��', '.');
                    break;
            }

            return _result.ToString();
        }
    }
}
