using System.Collections;
using UnityEngine;

namespace TInventory.Window
{
    public class WindowMover : MonoBehaviour
    {
        
        void Update()
        {
            TryMoveWindow();
        }

        private void TryMoveWindow()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var windowHeaderObject = Inventory.GetWindowHeaderAtMousePosition();

                var window = windowHeaderObject?.transform.parent.GetComponent<Window>();

                if (window is null || window.IsLocked()) return;

                StartCoroutine(MoveWindowToMouse(window.transform, window.transform.position - Input.mousePosition));
            }
        }


        /// <summary>
        /// Moves window to mouse position
        /// </summary>
        /// <param name="window">Window clicked on</param>
        /// <param name="mouseOffset">Mouse's offset from the window's</param>
        private IEnumerator MoveWindowToMouse(Transform window, Vector3 mouseOffset)
        {
            while (Input.GetMouseButton(0))
            {
                window.transform.SetAsLastSibling();
                window.position = Input.mousePosition + mouseOffset;
                yield return null;
            }
        }
    }
}
