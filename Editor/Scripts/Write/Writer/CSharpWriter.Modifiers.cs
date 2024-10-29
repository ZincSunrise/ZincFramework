namespace ZincFramework.ScriptWriter
{
    public partial class CSharpWriter
    {
        public static class Modifiers
        {
            public static string[] Virtual { get; } = new string[] { "virtual" };
            public static string[] Abstract { get; } = new string[] { "abstract" };
            public static string[] Override { get; } = new string[] { "override" };
            public static string[] Static { get; } = new string[] { "static" };
            public static string[] Aysnc { get; } = new string[] { "async" };
            public static string[] ReadOnly { get; } = new string[] { "readonly" };
            public static string[] ReadOnlyStatic { get; } = new string[] { "readonly", "static" };
        }

        public static class Accessors
        {
            public const string Public = "public";
            public const string Private = "private";
            public const string Protected = "protected";
            public const string Internal = "internal";
        }
    }
}