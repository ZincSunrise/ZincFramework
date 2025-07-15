using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;


namespace ZincFramework.Excel
{
    public static class ExcelExtension
    {
        public static bool IsEmpty(this Row row, SharedStringTable sharedStringTable = null)
        {
            return row == null || row.Descendants<Cell>().All(x => x.IsEmpty(sharedStringTable));
        }

        public static bool IsEmpty(this Cell cell, SharedStringTable sharedStringTable = null)
        {
            if(cell == null)
            {
                return true;
            }
            if(cell.DataType == null)
            {
                return string.IsNullOrWhiteSpace(cell.CellValue?.Text);
            }

            if(cell.DataType == CellValues.InlineString)
            {
                return string.IsNullOrWhiteSpace(cell.InlineString?.Text?.Text);
            }
            else if(cell.DataType == CellValues.SharedString)
            {
                if (sharedStringTable == null || cell.CellValue == null)
                {
                    return true;
                }

                if(!int.TryParse(cell.CellValue?.Text, out var index) || index < 0 || index >= sharedStringTable.Count())
                {
                    return true;
                }

                SharedStringItem item = sharedStringTable.ElementAt(index) as SharedStringItem;
                string sharedText = item.Text?.Text;

                return string.IsNullOrWhiteSpace(sharedText); 
            }

            return string.IsNullOrWhiteSpace(cell.CellValue?.Text);
        }

        public static string GetCellValue(this Cell cell, SharedStringTable sharedStringTable = null)
        {
            if (cell.IsEmpty(sharedStringTable))
            {
                return string.Empty;
            }

            if(cell.DataType == null)
            {
                return cell.CellValue?.Text ?? string.Empty;
            }

            if(cell.DataType == CellValues.SharedString)
            {
                SharedStringItem item = sharedStringTable.ElementAt(int.Parse(cell.CellValue?.Text)) as SharedStringItem;
                string sharedText = item.Text?.Text;

                return sharedText;
            }
            else if (cell.DataType == CellValues.InlineString)
            {
                return cell.InlineString?.Text?.Text ?? string.Empty;
            }

            return cell.CellValue?.Text ?? string.Empty;
        }
    }
}