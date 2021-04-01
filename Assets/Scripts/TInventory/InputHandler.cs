using UnityEngine;

namespace TInventory
{
    public static class InputHandler
    {
        public static Vector3 GetCursorPosition()
        {
            return Input.mousePosition;
        }
        
        public static bool GetPrimaryButtonDown()
        {
            return Input.GetMouseButtonDown(0);
        }
        
        public static bool GetPrimaryButton()
        {
            return Input.GetMouseButton(0);
        }
        
        public static bool GetSecondaryButtonDown()
        {
            return Input.GetMouseButtonDown(1);
        }
        
        public static bool GetSecondaryButton()
        {
            return Input.GetMouseButton(1);
        }

        public static bool RotateButtonDown()
        {
            return Input.GetKeyDown(KeyCode.R);
        }
    }
}
