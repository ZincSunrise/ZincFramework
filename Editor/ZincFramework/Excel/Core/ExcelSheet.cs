using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;




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

            public uint StyleIndex {  get; private set; }

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
                    if(rowIndex > RowCount)
                    {
                        RowCount = rowIndex;
                    }
                    if(excelRow.ColCount > ColCount)
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

            private ExcelCell CreateCell(string cellValue)
            {
                bool isNumber = long.TryParse(cellValue, out _) && (float.TryParse(cellValue, out _) || double.TryParse(cellValue, out _));
                Cell cell = new Cell();

                if (isNumber)
                {
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(cellValue);
                }
                else
                {
                    cell.DataType = CellValues.SharedString;
                    if (!SharedStringPool.TryGetShared(cellValue, out var index))
                    {
                        SharedStringPool.AddSharedString(cellValue, out index);
                    }

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

                    foreach(var col in columns.Descendants<Column>())
                    {
                        if(col.Max > column.Max)
                        {
                            columns.InsertBefore(column, col);
                            break;
                        }
                    }

                    if(column.Parent != columns)
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
                if (cell.CellValue == null)
                {
                    return string.Empty;
                }

                return cell.DataType != null && cell.DataType == CellValues.SharedString ? SharedStringPool[cell.CellValue.InnerText] : cell.CellValue.InnerText;
            }

            private void SetCellValue(Cell cell, string value)
            {
                if(cell.DataType != null && cell.DataType == CellValues.SharedString)
                {
                    if (!SharedStringPool.TryGetShared(value, out var index))
                    {
                        SharedStringPool.AddSharedString(value, out index);
                    }

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
        }
    }
}