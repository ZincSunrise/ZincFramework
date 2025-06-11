namespace ZincFramework.DialogueSystem
{
    public class TextTreeRunner : BaseSafeSingleton<TextTreeRunner>
    {
        public TextTree MainTextTree { get; private set; }

        private TextTreeRunner()
        {

        }

        public void SetTextTree(TextTree textTree) => MainTextTree = textTree;
    }
}