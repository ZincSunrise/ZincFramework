using System.Collections;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using UnityEditor.SceneManagement;


namespace ZincFramework
{

    [InitializeOnLoad()]
    public class AutoSave
    {
        private static EditorCoroutine autoSaveCoroutiue;
        private static int _saveOffset = 600;
        static AutoSave()
        {
            _saveOffset = Resources.Load<FrameworkData>("Framework/FrameworkData").saveOffset;
            SaveScenes();

            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    EditorCoroutineUtility.StopCoroutine(autoSaveCoroutiue);
                }
                else if (state == PlayModeStateChange.EnteredEditMode)
                {
                    autoSaveCoroutiue = EditorCoroutineUtility.StartCoroutineOwnerless(R_SaveScenes());
                }
            };
        }

        private static void SaveScenes()
        {
            autoSaveCoroutiue = EditorCoroutineUtility.StartCoroutineOwnerless(R_SaveScenes());
        }

        private static IEnumerator R_SaveScenes()
        {
            while (true)
            {
                yield return new EditorWaitForSeconds(_saveOffset);
                Debug.Log("自动保存了");

                EditorSceneManager.SaveOpenScenes();
                _saveOffset = Resources.Load<FrameworkData>("Framework/FrameworkData").saveOffset;
            }

        }
    }
}

