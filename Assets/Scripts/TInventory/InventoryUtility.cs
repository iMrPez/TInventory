using System.Collections.Generic;
using TInventory.Container;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TInventory
{
    public class InventoryUtility : MonoBehaviour
    {
        private static EventSystem EventSystem;
        private static GraphicRaycaster Raycaster;
        private static PointerEventData PointerEventData;
        
        private void Awake()
        {

            EventSystem = FindObjectOfType<EventSystem>();
            Raycaster = FindObjectOfType<GraphicRaycaster>();
            
            PointerEventData = new PointerEventData(EventSystem);
        }
        
        /// <summary>
        /// Gets the hit objects from raycast
        /// </summary>
        /// <param name="position">Position to raycast at</param>
        /// <returns>List of object hit from raycast</returns>
        private static List<RaycastResult> GetRaycastResults(Vector3 position)
        {
            //Set the Pointer Event Position to that of the mouse position
            PointerEventData.position = position;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            Raycaster.Raycast(PointerEventData, results);

            return results;
        }
        
        /// <summary>
        /// Gets item at position. If an ignored item is supplied, the ignored item will be ignored.
        /// </summary>
        /// <param name="position">Position to check for item</param>
        /// <param name="ignoreItem"></param>
        /// <returns>Item found at supplied position.</returns>
        public static Item.Item GetItemAt(Vector3 position, GameObject ignoreItem = null)
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(position))
            {
                if (result.gameObject.CompareTag("Window")) break;
                
                if (result.gameObject.CompareTag("Item"))
                {
                    if (result.gameObject != ignoreItem)
                    {
                        return result.gameObject.GetComponent<Item.Item>();
                    }
                }
            }
            
            return null;
        }
    
        /// <summary>
        /// Gets container group at supplied position
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>ContainerGroup</returns>
        public static ContainerGroup GetContainerGroupAt(Vector3 position)
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(position))
            {
                if (result.gameObject.CompareTag("Window")) break;
                
                if (result.gameObject.CompareTag("ContainerGroup"))
                {
                    return result.gameObject.GetComponent<ContainerGroup>();
                }
            }
            
            return null;
        }
        
        
        /// <summary>
        /// Gets action slot at supplied position
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>Attach Slot</returns>
        public static AttachSlot.AttachSlot GetActionSlotAt(Vector3 position)
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(position))
            {
                if (result.gameObject.CompareTag("Window")) break;
                
                if (result.gameObject.CompareTag("AttachSlot"))
                {
                    return result.gameObject.GetComponent<AttachSlot.AttachSlot>();
                }
            }
            
            return null;
        }
        
        
        /// <summary>
        /// Gets the container at position.
        /// </summary>
        /// <returns>Returns Container at position.</returns>
        public static TInventory.Container.Container GetContainerAt(Vector2 position)
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(position))
            {
                if (result.gameObject.CompareTag("Window")) break;
                
                if (result.gameObject.CompareTag("Container"))
                {
                    return result.gameObject.GetComponent<global::TInventory.Container.Container>();
                }
            }
            return null;
        }
        
        /// <summary>
        /// Gets the container at touch position.
        /// </summary>
        /// <returns>Returns Container at touch position.</returns>
        public static Window.Window GetWindowAtMousePosition()
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(Input.mousePosition))
            {
                if (result.gameObject.CompareTag("Window"))
                {
                    return result.gameObject.GetComponent<Window.Window>();
                }
            }
            return null;
        }
        
        /// <summary>
        /// Gets the header for the window at the mouse position.
        /// </summary>
        /// <returns>Window Header GameObject</returns>
        public static GameObject GetWindowHeaderAtMousePosition()
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(Input.mousePosition))
            {
                if (result.gameObject.CompareTag("WindowHeader"))
                {
                    return result.gameObject;
                }
            }
            return null;
        }
    }
}
