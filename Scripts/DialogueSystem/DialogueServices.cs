namespace ZincFramework.DialogueSystem
{
    public static class DialogueServices
    {
        public static TextTreeRunner TextTreeRunner { get; private set; }

        public static void Initialize(TextTreeRunner textTreeRunner)
        {
            TextTreeRunner = textTreeRunner;
        }

        public static void SetTextTree(TextTree textTree)
        {
            TextTreeRunner.SetTextTree(textTree);
        }
    }
}