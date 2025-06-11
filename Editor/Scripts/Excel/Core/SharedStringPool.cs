using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;



namespace ZincFramework
{
    namespace Excel
    {
        public class SharedStringPool
        {
            public SharedStringTable SharedStringTable { get; }

            private readonly Dictionary<string, int> _sharedIndexMap = new Dictionary<string, int>();


            public SharedStringPool(SharedStringTable sharedStringTable)
            {
                SharedStringTable = sharedStringTable;
                int index = 0;

                foreach (var item in SharedStringTable.ChildElements)
                {
                    if (item is SharedStringItem sharedItems)
                    {
                        _sharedIndexMap.TryAdd(sharedItems.InnerText, index);
                    }

                    index++;
                }
            }

            public string GetCellValue(Cell cell)
            {
                if (cell == null || cell.CellValue == null) return string.Empty;

                // 判断单元格类型
                string cellText = cell.CellValue.InnerText; // 原始值
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    // 如果是共享字符串，查找 SharedStringTable
                    if (int.TryParse(cellText, out int index))
                    {
                        return SharedStringTable.ElementAt(index).InnerText;
                    }
                }

                return cellText; // 默认返回原始值（数值或其他类型）
            }

            /// <summary>
            /// 尝试添加新的共享字符串
            /// </summary>
            /// <param name="sharedString"></param>
            /// <param name="index"></param>
            public bool AddOrGet(string sharedString, out int index)
            {
                if (!_sharedIndexMap.TryGetValue(sharedString, out index))
                {
                    var newItem = SharedStringTable.AppendChild(new SharedStringItem(new Text(sharedString)));
                    index = SharedStringTable.TakeWhile(item => item != newItem).Count();

                    SharedStringTable.Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}