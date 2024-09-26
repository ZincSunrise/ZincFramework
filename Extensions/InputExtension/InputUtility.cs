using System.IO;
using UnityEngine.InputSystem;

namespace ZincFramework 
{ 
    namespace InputListener
    {
        public class InputUtility
        {
            public static InputActionAsset LoadInputAsset(string path, string name)
            {
                string jsonStr = File.ReadAllText(Path.Combine(path, name + ".json"));
                return InputActionAsset.FromJson(jsonStr);
            }

            public static void SaveInputAsset(InputActionAsset changedAsset, string path)
            {
                File.WriteAllText(Path.Combine(path, changedAsset.name + ".json"), changedAsset.ToJson());
            }
            public static void SaveInputAsset(PlayerInput playerInput, string path)
            {
                File.WriteAllText(Path.Combine(path, playerInput.actions.name + ".json"), playerInput.actions.ToJson());
            }
        }
    }
}


