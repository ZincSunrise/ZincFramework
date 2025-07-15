using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ZincFramework.Excel
{
    public class ExcelSheet
    {
        public List<ExcelRow> ExcelRows { get; set; }

        public SheetData SheetData { get; }

        public string TableName { get; private set; }

        public int RowCount { get; private set; }

        public int ColCount { get; private set; }

        #region 读取方法
        public ExcelSheet(Sheet sheet, Worksheet worksheet, SharedStringTable sharedStringTable)
        {
            TableName = sheet.Name;
            ExcelRows = new List<ExcelRow>();
            SheetData = worksheet.GetFirstChild<SheetData>();

            var rows = worksheet.Descendants<Row>();

            //首先获取最大行数和最大列数
            Row lastRow = null;
            foreach (var row in rows)
            {
                if (!row.IsEmpty(sharedStringTable))
                {
                    lastRow = row;
                    Cell cell = row.Descendants<Cell>().Last(x => !x.IsEmpty(sharedStringTable));
                    int cellIndex = ExcelUtility.GetCellIndex(cell.CellReference);
                    ColCount = Mathf.Max(cellIndex + 1, ColCount);
                }
            }

            RowCount = (int)(uint)lastRow?.RowIndex;

            int nowRowIndex = 0;
            foreach (var row in rows)
            {
                //每行的Index是1，2，3这类
                int rowIndex = (int)(uint)row.RowIndex - 1;

                if (nowRowIndex < rowIndex)
                {
                    FillEmptyRow(rowIndex - nowRowIndex, ColCount);
                }

                if (row.IsEmpty(sharedStringTable))
                {
                    Debug.LogWarning($"行{row.RowIndex?.Value}为空");
                    continue;
                }

                ExcelRow excelRow = new ExcelRow(row, ColCount, sharedStringTable);
                ExcelRows.Add(excelRow);
                nowRowIndex++;
            }
        }

        public string this[int rowIndex, int colIndex]
        {
            get => ExcelRows[rowIndex].Cells[colIndex].CellValue;
            set => ExcelRows[rowIndex].Cells[colIndex].CellValue = value;
        }

        public ExcelRow this[int rowIndex] => ExcelRows[rowIndex];
        #endregion

        #region 写入方法
        /// <summary>
        /// 创建一个行的方法
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public ExcelRow CreateRow(int rowIndex)
        {
            return new ExcelRow(null, RowCount);
        }

        public void SetRowHeight(int rowIndex, int height)
        {
            uint index = (uint)(rowIndex + 1);
        }

        public void SetColumnWidth(int index, int width)
        {

        }

        private void FillEmptyRow(int fillCount, int colCount)
        {
            for (int i = 0; i < fillCount; i++) 
            {
                ExcelRow excelRow = new ExcelRow();
                excelRow.FillEmptyCell(colCount);
                ExcelRows.Add(excelRow);
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Join('\n', ExcelRows);
        }
    }
}