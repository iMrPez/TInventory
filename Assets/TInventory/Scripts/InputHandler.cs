using UnityEngine;

namespace TInventory
{
    public delegate void KeyDelegate(InputHandler.Key key);
    
    public class InputHandler : MonoBehaviour
    {
        public static event KeyDelegate KeyPressedHandler;
        public static event KeyDelegate KeyReleasedHandler;
        
        public enum Key
        {
            Primary = 0,
            Secondary = 1,
            Rotate = 2
        }

        private void Update()
        {
            TriggerPrimaryEvents();
            TriggerSecondaryEvents();
            TriggerRotateEvents();
        }

        
        private void TriggerPrimaryEvents()
        {
            if (GetPrimaryButtonDown())
            {
                KeyPressedHandler?.Invoke(Key.Primary);
            }

            if (GetPrimaryButtonUp())
            {
                KeyReleasedHandler?.Invoke(Key.Primary);
            }
        }
        private void TriggerSecondaryEvents()
        {
            if (GetSecondaryButtonDown())
            {
                KeyPressedHandler?.Invoke(Key.Secondary);
            }

            if (GetSecondaryButtonUp())
            {
                KeyReleasedHandler?.Invoke(Key.Secondary);
            }
        }
        private void TriggerRotateEvents()
        {
            if (GetRotateButtonDown())
            {
                KeyPressedHandler?.Invoke(Key.Rotate);
            }

            if (GetRotateButtonUp())
            {
                KeyReleasedHandler?.Invoke(Key.Rotate);
            }
        }
        
        public static Vector3 GetCursorPosition()
        {
            return Input.mousePosition;
        }
        
        // Primary Button
        public static bool GetPrimaryButtonDown()
        {
            return Input.GetMouseButtonDown(0);
        }
        public static bool GetPrimaryButtonUp()
        {
            return Input.GetMouseButtonUp(0);
        }
        public static bool GetPrimaryButton()
        {
            return Input.GetMouseButton(0);
        }
        
        // Secondary Button
        public static bool GetSecondaryButtonDown()
        {
            return Input.GetMouseButtonDown(1);
        }
        public static bool GetSecondaryButtonUp()
        {
            return Input.GetMouseButtonUp(1);
        }
        public static bool GetSecondaryButton()
        {
            return Input.GetMouseButton(1);
        }

        
        // Rotate Button
        public static bool GetRotateButtonDown()
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        public static bool GetRotateButtonUp()
        {
            return Input.GetKeyUp(KeyCode.R);
        }
        public static bool GetRotateButton()
        {
            return Input.GetKey(KeyCode.R);
        }

        public static bool GetModifierButton()
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
    }
}
