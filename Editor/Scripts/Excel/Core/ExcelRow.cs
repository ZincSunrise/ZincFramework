using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;


namespace ZincFramework.Excel
{
    public delegate ExcelCell CreateCell(string value);

    public class ExcelRow
    {
        public List<ExcelCell> Cells { get; } = new List<ExcelCell>();

        private int _height;

        public ExcelRow()
        {

        }

        public ExcelRow(Row row, int colCount, SharedStringTable sharedStringTable = null)
        {
            foreach (var cell in row.Descendants<Cell>())
            {
                string cellValue = cell.GetCellValue(sharedStringTable);
                int cellIndex = ExcelUtility.GetCellIndex(cell.CellReference);

                if(cellIndex > Cells.Count)
                {
                    FillEmptyCell(cellIndex - Cells.Count);
                }

                Cells.Add(new ExcelCell(cellValue));
            }

            if(Cells.Count < colCount)
            {
                FillEmptyCell(colCount - Cells.Count);
            }
        }


        public string this[int cellIndex]
        {
            get => Cells[cellIndex].CellValue;
            set => Cells[cellIndex].CellValue = value;
        }

        public void SetHeight(int heiget)
        {
            _height = heiget;
        }

        public void FillEmptyCell(int fillCount) 
        {
            for(int i = 0; i < fillCount; i++)
            {
                Cells.Add(new ExcelCell(string.Empty));
            }
        }

        public override string ToString()
        {
            return string.Join(' ', Cells);
        }
    }
}
