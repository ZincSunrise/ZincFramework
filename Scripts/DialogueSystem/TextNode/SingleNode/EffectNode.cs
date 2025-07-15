namespace ZincFramework.DialogueSystem
{
    public class EffectNode : SingleTextNode
    {
#if UNITY_EDITOR
        public override string InputHtmlColor => "#F2F230";

        public override string OutputHtmlColor => "#61F2C2";
#endif
    }
}