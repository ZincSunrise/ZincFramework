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

                // �жϵ�Ԫ������
                string cellText = cell.CellValue.InnerText; // ԭʼֵ
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    // ����ǹ����ַ��������� SharedStringTable
                    if (int.TryParse(cellText, out int index))
                    {
                        return SharedStringTable.ElementAt(index).InnerText;
                    }
                }

                return cellText; // Ĭ�Ϸ���ԭʼֵ����ֵ���������ͣ�
            }

            /// <summary>
            /// ��������µĹ����ַ���
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