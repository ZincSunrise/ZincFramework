using System.IO;
using UnityEngine.UIElements;
using ZincFramework.Events;



namespace ZincFramework.DialogueSystem.GraphView
{
    public class TreeAssetBar : VisualElement
    {
        public int AssetIndex { get; set; }

        public event ZincAction<MouseDownEvent> OnMouseDown;

        public TreeAssetBar(string labelName, int assetIndex)
        {
            this.name = "AssetButton";

            this.Add(new Label(Path.GetFileNameWithoutExtension(labelName)));
            this.RegisterCallback<MouseDownEvent>(LoadAsset);
            AssetIndex = assetIndex;
        }

        private void LoadAsset(MouseDownEvent mouseDownEvent)
        {
            OnMouseDown?.Invoke(mouseDownEvent);
        }
    }
}
