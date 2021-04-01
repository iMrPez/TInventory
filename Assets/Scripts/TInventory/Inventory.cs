﻿using System;
using System.Collections.Generic;
using Inventory;
using Inventory.Item;
using TInventory.Container;
using TInventory.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TInventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;

        // TODO ADD SUMMARY
        [Header("Prefabs")] 
        [SerializeField] 
        private GameObject windowPrefab;
        
        /// <summary>
        /// Container prefab;
        /// </summary>
        [SerializeField]
        private GameObject containerPrefab;

        /// <summary>
        /// Slot prefab;
        /// </summary>
        [SerializeField]
        public GameObject slotPrefab;

        /// <summary>
        /// ContainerGroup Prefab
        /// </summary>
        [SerializeField] 
        public GameObject containerGroupPrefab;
        
        /// <summary>
        /// Item prefab;
        /// </summary>
        [SerializeField]
        public GameObject itemPrefab;

        public Canvas inventoryCanvas;
        
        [Header("Window")] 
        public Transform windowContainer;
        public float scrollDownAt;
        public float scrollUpAt;
        public float scrollSpeed;

        [Header("Container")] 
        public float padding;
        public float margin;
        public float slotSize;
        
        public ContainerData testContainerData; // TODO REMOVE! ONLY FOR TESTING
        
        private static GraphicRaycaster Raycaster;
        private static PointerEventData PointerEventData;
    
        
        [Header("Misc")]
        [SerializeField]
        private EventSystem eventSystem;

        public TInventory.Container.Container inventoryContainer;
        
        private void Awake()
        {
            instance = this;
            
            Raycaster = GetComponent<GraphicRaycaster>();
            
            PointerEventData = new PointerEventData(eventSystem);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Test();
            }
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

        // TODO ADD SUMMARY
        public static Container.Container CreateNewContainer()
        {
            var container = Instantiate(instance.containerPrefab).GetComponent<Container.Container>();

            return container;
        }

        // TODO REMOVE TESTING METHOD
        
        public void Test()
        {
            CreateNewWindow("Test Window", new Vector2(500, 500));
        }
        
        // TODO ADD SUMMARY
        public static Window.Window CreateNewWindow(string title, Vector2 windowSize)
        {
            var window = Instantiate(instance.windowPrefab, instance.windowContainer).GetComponent<Window.Window>();
            
            window.SetWindowTitle(title);
            window.SetWindowSize(windowSize.x, windowSize.y);

            return window;
        }
        

        /// <summary>
        /// Gets item at position. If an ignored item is supplied, the ignored item will be ignored.
        /// </summary>
        /// <param name="position">Position to check for item</param>
        /// <param name="ignoreItem"></param>
        /// <returns>Item found at supplied position.</returns>
        public static AItem GetItemAt(Vector3 position, GameObject ignoreItem = null)
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(position))
            {
                if (result.gameObject.CompareTag("Item"))
                {
                    if (result.gameObject != ignoreItem)
                    {
                        return result.gameObject.GetComponent<AItem>();
                    }
                }
            }
            
            return null;
        }
    
        // TODO add summary
        public static ContainerGroup GetContainerGroupAt(Vector3 position)
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(position))
            {
                if (result.gameObject.CompareTag("ContainerGroup"))
                {
                    return result.gameObject.GetComponent<ContainerGroup>();
                }
            }
            
            return null;
        }
        
        
        // TODO add summary
        public static AttachSlot.AttachSlot GetActionSlotAt(Vector3 position)
        {
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in GetRaycastResults(position))
            {
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
