using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TInventory.Editor
{
    
    public class StartWizardEditor : EditorWindow
    {
        
        [MenuItem("Window/TInventory/Startup Wizard")]
        public static void ShowWindow()
        {
            var window = GetWindow<StartWizardEditor>("TInventory Startup Wizard");
            window.minSize = new Vector2(280, 120);
            window.maxSize = new Vector2(280, 120);
        }
        
        private void OnGUI()
        {
            var aTexture = Resources.Load<Texture>("TInventoryLogo");
            
            GUI.DrawTexture(new Rect(10, 10, 256, 50), aTexture, ScaleMode.ScaleToFit, true, 10.0F);

            if (GUI.Button(new Rect(10, 70, 256, 40),"Initialize TInventory"))
            {
                
                var inventory = FindOrCreateInventory();
                
                FindOrCreateCanvas(inventory);
                
                FindOrCreateEventSystem();
            }
        }

        private static Inventory FindOrCreateInventory()
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            
            if (inventory is null)
            {
                var prefab = Resources.Load<GameObject>("[TInventory]");

                inventory = Instantiate(prefab).GetComponent<Inventory>();

                inventory.gameObject.name = "[TInventory]";
            }

            return inventory;
        }

        private static void FindOrCreateEventSystem()
        {
            if (FindObjectOfType<EventSystem>() is null)
            {
                var newEventSystem = new GameObject("EventSystem");
                newEventSystem.AddComponent<EventSystem>();
                newEventSystem.AddComponent<StandaloneInputModule>();
            }
        }

        private static void FindOrCreateCanvas(Inventory inventory)
        {
            var canvas = FindObjectOfType<Canvas>();

            if (canvas is null)
            {
                var newCanvasObj = new GameObject("Canvas");
                newCanvasObj.AddComponent<Canvas>();

                var newCanvas = newCanvasObj.GetComponent<Canvas>();
                newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                newCanvasObj.AddComponent<CanvasScaler>();
                newCanvasObj.AddComponent<GraphicRaycaster>();

                inventory.windowCanvas = newCanvasObj.transform;
            }
            else
            {
                inventory.windowCanvas = canvas.transform;
            }
        }
    }
}
