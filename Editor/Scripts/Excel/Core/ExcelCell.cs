using DocumentFormat.OpenXml.Spreadsheet;


namespace ZincFramework.Excel
{
    public class ExcelCell
    {
        public string CellValue { get; set; }

        public ExcelCell(string cellValue)
        {
            CellValue = cellValue;
        }

        public static explicit operator string(ExcelCell excelCell)
        {
            return excelCell.CellValue;
        }

        public Cell ConvertToOpenXmlCell()
        {
            Cell cell = new Cell();

            if(int.TryParse(CellValue, out _))
            {
                cell.DataType = CellValues.Number;
                cell.CellValue = new CellValue(CellValue);
            }
            else
            {
                cell.DataType = CellValues.InlineString;
                cell.InlineString = new InlineString(CellValue);
            }

            return cell;
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(CellValue) ? "Null" : CellValue;
        }
    }
}