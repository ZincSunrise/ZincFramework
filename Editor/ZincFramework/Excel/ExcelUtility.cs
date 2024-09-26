using DocumentFormat.OpenXml.Spreadsheet;


namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelUtility
        {
            public static string GetReference(Row row, int colIndex)
            {
                char a = (char)(colIndex % 26 + 'A');
                int count = (colIndex + 1) / 26;
                string reference = new string(a, count) + row.RowIndex;

                return reference;
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


            public static void GetRowCellIndex(string cellRefence, out int rowIndex, out int cellIndex)
            {
                cellIndex = 0;
                rowIndex = 0;

                for (int i = 0; i < cellRefence.Length; i++)
                {
                    if (char.IsNumber(cellRefence[i]))
                    {
                        rowIndex = int.Parse(cellRefence[i..]);
                        break;
                    }
                    cellIndex += cellRefence[i] - 'A';
                }
            }
        }
    }
}