using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelSheet
        {
            public Worksheet Worksheet { get; }

            public SheetData SheetData { get; }

            public SharedStringPool SharedStringPool { get; }

            public int RowCount { get; private set; }

            public int ColCount { get; private set; }

            public uint StyleIndex { get; private set; }

            public string TableName { get; }


            private readonly Dictionary<uint, ExcelRow> _rowMap = new Dictionary<uint, ExcelRow>();

            public ExcelSheet(Sheet sheet, Worksheet worksheet, uint styleIndex, SharedStringPool sharedStringPool)
            {
                TableName = sheet.Name;
                SharedStringPool = sharedStringPool;
                Worksheet = worksheet;
                SheetData = worksheet.GetFirstChild<SheetData>();
                StyleIndex = styleIndex;

                foreach (var row in Worksheet.Descendants<Row>())
                {
                    ExcelRow excelRow = new ExcelRow(StyleIndex, row, CreateCell, GetCellValue, SetCellValue);
                    _rowMap.Add(row.RowIndex, excelRow);

                    RowCount = (int)Mathf.Max(RowCount, row.RowIndex.Value);
                    ColCount = Mathf.Max(ColCount, excelRow.ColCount);
                }
            }

            public string this[string cellReference]
            {
                get
                {
                    ExcelUtility.GetRowCellIndex(cellReference, out var rowIndex, out var cellIndex);

                    if (!_rowMap.TryGetValue((uint)rowIndex, out var excelRow))
                    {
                        return string.Empty;
                    }

                    return excelRow[cellIndex];
                }
                set
                {
                    ExcelUtility.GetRowCellIndex(cellReference, out var rowIndex, out var cellIndex);

                    if (!_rowMap.TryGetValue((uint)rowIndex, out var excelRow))
                    {
                        excelRow = CreateRow(rowIndex);
                        _rowMap.Add((uint)rowIndex, excelRow);
                    }

                    excelRow[cellIndex] = value;
                    if (rowIndex > RowCount)
                    {
                        RowCount = rowIndex;
                    }
                    if (excelRow.ColCount > ColCount)
                    {
                        ColCount = excelRow.ColCount;
                    }
                }
            }

            public string this[int rowIndex, int colIndex]
            {
                get
                {
                    if (!_rowMap.TryGetValue((uint)(rowIndex + 1), out var excelRow))
                    {
                        return string.Empty;
                    }

                    return excelRow[colIndex];
                }
                set
                {
                    rowIndex++;
                    if (!_rowMap.TryGetValue((uint)rowIndex, out var excelRow))
                    {
                        excelRow = CreateRow(rowIndex);
                        _rowMap.Add((uint)rowIndex, excelRow);
                    }

                    excelRow[colIndex] = value;

                    if (rowIndex > RowCount)
                    {
                        RowCount = rowIndex;
                    }
                    if (colIndex > ColCount)
                    {
                        ColCount = colIndex;
                    }
                }
            }

            public ExcelRow this[int rowIndex] => _rowMap[(uint)rowIndex + 1];

            public bool TryGetRow(int rowIndex, out ExcelRow excelRow) => _rowMap.TryGetValue((uint)(rowIndex + 1), out excelRow);

            /// <summary>
            /// 创建一个行的方法
            /// </summary>
            /// <param name="rowIndex"></param>
            /// <returns></returns>
            public ExcelRow CreateRow(int rowIndex)
            {
                Row row = new Row() { RowIndex = (uint)rowIndex };

                foreach (var excelRow in SheetData.Descendants<Row>())
                {
                    if (excelRow.RowIndex > row.RowIndex)
                    {
                        SheetData.InsertBefore(row, excelRow);
                        break;
                    }
                }

                if (row.Parent != SheetData)
                {
                    SheetData.AppendChild(row);
                }


                if (rowIndex > RowCount)
                {
                    RowCount = rowIndex;
                }

                return new ExcelRow(StyleIndex, row, CreateCell, GetCellValue, SetCellValue);
            }

            /// <summary>
            /// 创建一个格子的方法
            /// </summary>
            /// <param name="cellValue"></param>
            /// <returns></returns>
            private ExcelCell CreateCell(string cellValue)
            {
                Cell cell = new Cell();

                if (cellValue.All(x => char.IsNumber(x)))
                {
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue);
                }
                else
                {
                    cell.DataType = CellValues.SharedString;
                    SharedStringPool.AddOrGet(cellValue, out var index);
                    cell.CellValue = new CellValue(index);
                }


                return new ExcelCell(StyleIndex, cell, GetCellValue, SetCellValue);
            }

            public void SetColumnWidth(int columnIndex, int width)
            {
                columnIndex++;
                Columns columns = Worksheet.Descendants<Columns>()?.FirstOrDefault() ?? new Columns();
                if (columns.Parent != Worksheet)
                {
                    Worksheet.InsertAt(columns, 0);
                }

                Column column = columns.Descendants<Column>().FirstOrDefault(col => col.Max == columnIndex);
                if (column == null)
                {
                    column = new Column()
                    {
                        Max = (uint)columnIndex,
                        Min = (uint)columnIndex,
                        CustomWidth = true,
                        Width = width
                    };

                    foreach (var col in columns.Descendants<Column>())
                    {
                        if (col.Max > column.Max)
                        {
                            columns.InsertBefore(column, col);
                            break;
                        }
                    }

                    if (column.Parent != columns)
                    {
                        columns.AppendChild(column);
                    }
                }
                else
                {
                    column.Width = width;
                }

                Worksheet.Save();
            }

            public void SetRowHeight(int rowIndex, int height)
            {
                uint index = (uint)(rowIndex + 1);
                if (!_rowMap.TryGetValue(index, out var excelRow))
                {
                    excelRow = CreateRow(rowIndex);
                    _rowMap.Add(index, excelRow);
                }

                excelRow.SetHeight(height);
            }

            private string GetCellValue(Cell cell)
            {
                return SharedStringPool.GetCellValue(cell);
            }


            private void SetCellValue(Cell cell, string value)
            {
                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                {
                    SharedStringPool.AddOrGet(value, out var index);
                    cell.CellValue = new CellValue(index);
                }
                else
                {
                    cell.CellValue = new CellValue(value);
                }
            }

            public void SaveSheet()
            {
                Worksheet.Save();
            }

            /// <summary>
            /// 得到一整列的方法
            /// </summary>
            /// <param name="colIndex"></param>
            /// <returns></returns>
            public string[] GetColomn(int colIndex)
            {
                string[] strings = new string[RowCount];
                for (int i = 0; i < RowCount; i++)
                {
                    strings[i] = this[i][colIndex];
                }

                return strings;
            }
        }
    }
}