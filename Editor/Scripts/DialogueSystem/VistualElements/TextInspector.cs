using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Events;
using ZincFramework.TreeService;
using ZincFramework.TreeService.GraphView;



namespace ZincFramework.DialogueSystem.GraphView
{
    public class TextInspector : NodeInspector<TextTree>
    {
        public event ZincAction<TextTree> OnSelectedButton;

        public TextTree NowTextTree { get; set; }

        public new class UxmlFactory : UxmlFactory<TextInspector> { } 


        private string[] _textTreePath;


        private Editor _nodeEditor;


        private bool _isShowFile = true;


        private readonly ScrollView _scrollView = new ScrollView();


        private TreeAssetBar _selectedBar;

        public TextInspector()
        {
            _isShowFile = EditorPrefs.GetBool(nameof(_isShowFile), _isShowFile);
            Repaint();
        }

        public void OnSelectedEdge(Edge edge)
        {
            if(edge.output.node is ChoiceNodeView)
            {
                OnSelectNode((edge.input.node as TextNodeView).DisplayNode);
            }
        }

        public void Repaint()
        {
            var assetCollection = AssetDatabase.FindAssets("t:texttree");
            _textTreePath = Array.ConvertAll(assetCollection, (str) => AssetDatabase.GUIDToAssetPath(str));
            Init();
        }

        public void ClickShowFile()
        {
            if (!_isShowFile)
            {
                ShowAllFile();
                EditorPrefs.SetBool(nameof(_isShowFile), _isShowFile);
            }
        }

        public void ClickShowNode()
        {
            if (_isShowFile)
            {
                ShowSelectedNode();
                EditorPrefs.SetBool(nameof(_isShowFile), _isShowFile);
            }
        }

        private void Init()
        {
            if (_isShowFile)
            {
                ShowAllFile();
            }
            else
            {
                ShowSelectedNode();
            }
        }

        private void ShowAllFile()
        {
            Clear();

            _isShowFile = true;
            Add(_scrollView);
            _scrollView.contentContainer.Clear();

            for (int i = 0; i < _textTreePath.Length; i++)
            {
                TreeAssetBar treeAssetBar = new TreeAssetBar(_textTreePath[i], i);
                treeAssetBar.OnMouseDown += SelectedBarOnMouseDown;
                _scrollView.contentContainer.Add(treeAssetBar);
            }
        }

        private void ShowSelectedNode()
        {
            _isShowFile = false;
            Clear();
        }

        private void SelectedBarOnMouseDown(MouseDownEvent mouseEvent)
        {
            if(mouseEvent.currentTarget is TreeAssetBar treeAssetBar)
            {
                SetSelectedBar(treeAssetBar);
                TextTree textTree = AssetDatabase.LoadAssetAtPath<TextTree>(_textTreePath[_selectedBar.AssetIndex]);
                NowTextTree = textTree;
                OnSelectedButton?.Invoke(textTree);
            }
        }

        public void FindSelection(TextTree textTree)
        {
            NowTextTree = textTree;

            if (_isShowFile)
            {
                var collection = this.Query<TreeAssetBar>();
                string path = AssetDatabase.GetAssetPath(textTree);
                SetSelectedBar(collection.AtIndex(Array.IndexOf(_textTreePath, path)));
            }     
        }

        public void SetSelectedBar(TreeAssetBar treeAssetBar)
        {
            _selectedBar?.RemoveFromClassList("clicked");
            _selectedBar = treeAssetBar;
            treeAssetBar.AddToClassList("clicked");
        }

        public void OnSelectNode(BaseNode baseNode)
        {
            if (_isShowFile) 
            {
                return;
            }

            Clear();
            GameObject.DestroyImmediate(_nodeEditor);
            _nodeEditor = Editor.CreateEditor(baseNode);

            IMGUIContainer iMGUIContainer = new IMGUIContainer(() =>
            {
                if (_nodeEditor.target)
                {
                    _nodeEditor.OnInspectorGUI();
                }
            });

            this.Add(iMGUIContainer);
        }
    }
}