using DocumentFormat.OpenXml.Spreadsheet;
using UnityEngine;


namespace ZincFramework
{
    namespace Excel
    {
        public delegate string GetValueInCell(Cell cell);

        public delegate void SetValueInCell(Cell cell, string str);

        public class ExcelCell
        {
            public Cell Cell { get; }

            public event GetValueInCell GetCell;

            public event SetValueInCell SetCell;

            public ExcelCell(uint styleIndex, Cell cell, GetValueInCell getValue, SetValueInCell setValue) 
            {
                Cell = cell;
                SetCell = setValue;
                GetCell = getValue;
                Cell.StyleIndex = styleIndex;
            }

            public string GetValue() => GetCell?.Invoke(Cell);

            public void SetValue(string value) => SetCell?.Invoke(Cell, value);

            public static explicit operator string (ExcelCell excelCell)
            {
                return excelCell.GetValue();
            }
        }
    }
}