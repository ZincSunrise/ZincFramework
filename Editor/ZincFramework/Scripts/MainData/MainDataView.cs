using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;



namespace ZincFramework
{
    namespace UI
    {
        namespace Main
        {
            public class MainDataView : VisualElement
            {
                public IMGUIContainer Container { get; set; }

                public new class UxmlFactory : UxmlFactory<MainDataView> { }

                private Editor _frameworkEditor;
                public MainDataView() 
                {
                    _frameworkEditor = Editor.CreateEditor(FrameworkConsole.Instance.SharedData);
                    Container = new IMGUIContainer(_frameworkEditor.OnInspectorGUI);
                    Container.style.flexGrow = 1;
                    this.Add(Container);
                }
            }
        }
    }
}