using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using UnityEngine;



namespace ZincFramework
{
    namespace Excel
    {
        public class SharedStringPool
        {
            public SharedStringTable SharedStringTable { get; }

            private readonly Dictionary<string, string> _sharedStringMap = new Dictionary<string, string>();


            public SharedStringPool(SharedStringTable sharedStringTable) 
            {
                SharedStringTable = sharedStringTable;
                int index = 0;

                foreach(var item in SharedStringTable.ChildElements)
                {
                    if(item is SharedStringItem stringItem)
                    {
                        _sharedStringMap.Add(stringItem.Text.Text, index.ToString());
                        index++;
                    }
                }
            }

            public string this[string index]
            {
                get
                {
                    foreach(var pair in _sharedStringMap)
                    {
                        if(pair.Value == index)
                        {
                            return pair.Key;
                        }
                    }

                    return string.Empty;
                }
            }

            public bool TryGetShared(string sharedString, out string index) => _sharedStringMap.TryGetValue(sharedString, out index);

            public void AddSharedString(string sharedString, out string index)
            {
                if (!_sharedStringMap.TryGetValue(sharedString, out index))
                {
                    int i = 0;
                    var newItem = SharedStringTable.AppendChild(new SharedStringItem(new Text(sharedString)));
                    foreach(var item in SharedStringTable.ChildElements)
                    {
                        if(newItem == item)
                        {
                            _sharedStringMap.Add(sharedString, i.ToString());
                            break;
                        }
                        i++;
                    }

                    SharedStringTable.Save();
                }
                else
                {
                    string value = null;
                    foreach (var item in _sharedStringMap.Keys)
                    {
                        if (item == sharedString)
                        {
                            value = item;
                        }
                    }

                    Debug.Log($"�������к�Ϊ{index}�Ĺ���string��������ظ���{sharedString},ԭstringΪ{value}");
                }
            }
        }
    }
}