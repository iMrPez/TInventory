using UnityEditor;
using UnityEngine;

namespace TInventory.Editor
{
    public class HelpEditor : EditorWindow
    {
        private static HelpEditor window;
        
        [MenuItem("Window/TInventory/Help")]
        public static void ShowWindow()
        {
            window = GetWindow<HelpEditor>("TInventory Help");
            window.minSize = new Vector2(500, 120);
            window.maxSize = new Vector2(500, 120);
        }
        
        private void OnGUI()
        {
            DrawTop();
            
            GUI.Label(new Rect(10, 70, position.width, 30), "Guides on getting started with TInventory will be on the Github.");
        }

        private void DrawTop()
        {
            var aTexture = Resources.Load<Texture>("TInventoryLogo");

            GUI.DrawTexture(new Rect(20, 10, 256, 50), aTexture, ScaleMode.ScaleToFit, true, 10.0F);

            var content = new GUIContent(Resources.Load<Texture>("github"));

            if (GUI.Button(new Rect(position.width - 80, 10, 64, 64), content))
            {
                Application.OpenURL("https://github.com/iMrPez/TInventory");
            }
        }
    }
}
