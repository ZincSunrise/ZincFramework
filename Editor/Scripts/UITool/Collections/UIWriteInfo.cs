namespace ZincFramework.UI.Collections
{
    public readonly struct UIWriteInfo
    {
        public string Name { get;  }

        public string Path { get; }

        public string Type { get; }

        public UIWriteInfo(string name, string path, string type)
        {
            Name = name;
            Path = path;
            Type = type;
        }
    }
}