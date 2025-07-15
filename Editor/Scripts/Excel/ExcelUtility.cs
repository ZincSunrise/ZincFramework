using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;


namespace ZincFramework.Excel
{
    public static class ExcelUtility
    {
        private readonly static StringBuilder _stringBuilder = new StringBuilder();

        public static string GetReference(Row row, int colIndex)
        {
            _stringBuilder.Clear();

            for (int i = colIndex; i > 0; i /= 26)
            {
                i--;
                _stringBuilder.Insert(0, (char)(i % 26 + 'A'));
            }

            _stringBuilder.Append((uint)row.RowIndex);
            return _stringBuilder.ToString();
        }

        public static int GetCellIndex(string cellRefence)
        {
            int number = 0;

            for (int i = 0; i < cellRefence.Length; i++)
            {
                if (char.IsNumber(cellRefence[i]))
                {
                    break;
                }

                number += cellRefence[i] - 'A';
            }

            return number;
        }
    }
}