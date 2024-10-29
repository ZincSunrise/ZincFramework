using System.Collections.Generic;
using System.IO;
using System.Xml;


namespace ZincFramework
{
    namespace Network
    {
        public static class PoolCollector
        {
            private readonly static HashSet<string> _names = new HashSet<string>();

            public static void Collect(XmlNode messageNode)
            {
                _names.Add(messageNode.Attributes["namespace"].Value + '.' + messageNode.Attributes["name"].Value);
            }


            public static void InsertPool()
            {
                string path = Path.Combine(FrameworkPaths.FrameworkPath, "Network" , "Protocol" , "MessagePool.cs");
                List<string> poolStrs = new List<string>(File.ReadAllLines(path));

                int lineIndex = poolStrs.FindIndex((str) =>
                {
                    return str.Trim() == "static MessagePool()";
                });


                string registStr;
                foreach (string name in _names)
                {
                    registStr = new string('\t', 5) + $"RegistMessage<{name}, {name + "Handler"}>();";
                    if (!poolStrs.Contains(registStr))
                    {
                        poolStrs.Insert(lineIndex + 2, registStr);
                    }
                }

                File.WriteAllLines(path, poolStrs);
            }
        }
    }
}

