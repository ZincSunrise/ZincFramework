using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    namespace Excel
    {
        public delegate ExcelCell CreateCell(string value);

        public class ExcelRow
        {
            public Row Row { get; }

            public int ColCount { get; private set; }


            public event CreateCell CreateCell;


            private readonly Dictionary<int, ExcelCell> _excelCells = new Dictionary<int, ExcelCell>();
     
            public ExcelRow(uint styleIndex, Row row, CreateCell createCell, GetValueInCell getAction, SetValueInCell setAction) 
            {
                CreateCell = createCell;
                Row = row;

                foreach (var cell in row.Descendants<Cell>())
                {
                    int cellIndex = ExcelUtility.GetCellIndex(cell.CellReference);
                    _excelCells.Add(cellIndex, new ExcelCell(styleIndex, cell, getAction, setAction));
                    ColCount = Mathf.Max(ColCount, cellIndex + 1);
                }
            }


            public string this[int cellIndex]
            {
                get
                {
                    TryGetValue(cellIndex, out var str);
                    return str;
                }
                set
                {
                    if (!_excelCells.TryGetValue(cellIndex, out var excelCell))
                    {
                        excelCell = MakeCell(cellIndex, value);
                    }

                    excelCell.SetValue(value);
                    if (cellIndex > ColCount)
                    {
                        ColCount = cellIndex;
                    }
                }
            }

            public void SetHeight(int heiget)
            {
                Row.CustomHeight = true;
                Row.Height = heiget;
            }

            public bool TryGetValue(int cellIndex, out string value)
            {
                _excelCells.TryGetValue(cellIndex, out ExcelCell cell);

                value = cell?.GetValue() ?? string.Empty;
                return cell != null;
            }

            public void AddCell(int cellIndex, string cellValue)
            {
                if (!_excelCells.ContainsKey(cellIndex))
                {
                    MakeCell(cellIndex, cellValue);
                }
            }

            public void RemoveCell(int cellIndex) 
            {
                _excelCells.Remove(cellIndex, out var value);
                value.Cell.Remove();
            }

            private ExcelCell MakeCell(int cellIndex, string cellValue)
            {
                ExcelCell excelCell = CreateCell?.Invoke(cellValue);
                excelCell.Cell.CellReference = ExcelUtility.GetReference(Row, cellIndex);

                foreach (var cell in Row.Descendants<Cell>())
                {
                    int index = ExcelUtility.GetCellIndex(cell.CellReference);
                    if (index > cellIndex)
                    {
                        Row.InsertBefore(excelCell.Cell, cell);
                        break;
                    }
                }

                if (excelCell.Cell.Parent != Row)
                {
                    Row.AppendChild(excelCell.Cell);
                }

                _excelCells.Add(cellIndex, excelCell);
                return excelCell;
            }
        }
    }
}
