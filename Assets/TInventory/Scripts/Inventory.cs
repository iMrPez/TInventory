using System.Collections.Generic;
using TInventory.Container;
using UnityEngine;


namespace TInventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }
        
        [Header("Prefabs")] 
        [SerializeField] private GameObject windowPrefab;
        [SerializeField] private GameObject containerPrefab;
        [SerializeField] private GameObject slotPrefab;
        public GameObject SlotPrefab => slotPrefab;

        [SerializeField] private GameObject containerGroupPrefab;
        public GameObject ContainerGroupPrefab => containerGroupPrefab;

        [Header("Window")] 
        public Transform windowCanvas;
        public float scrollDownAt;
        public float scrollUpAt;
        public float scrollSpeed;

        [Header("Container")] 
        public float padding;
        public float margin;
        public float slotSize;

        [SerializeField] private List<ContainerData> _containerDatas = new List<ContainerData>();

        public Window.Window InventoryWindow;
        
        private void Awake()
        {
            if(Instance != null) Debug.LogError("More then one instance of Inventory!", Instance);
            Instance = this;
        }

        
        /// <summary>
        /// Gets the id associated with supplied containerData. This is only used for saving and loading
        /// </summary>
        /// <param name="containerData">ContainerData</param>
        /// <returns>ContainerData Id</returns>
        public int GetContainerDataId(ContainerData containerData)
        {
            for (int i = 0; i < _containerDatas.Count; i++)
            {
                if (_containerDatas[i] == containerData) return i;
            }

            return -1;
        }

        
        /// <summary>
        /// Gets containerData from supplied id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>ContainerData</returns>
        public ContainerData GetContainerDataFromId(int id)
        {
            return _containerDatas[id];
        }
        
        /// <summary>
        /// Creates New Container
        /// </summary>
        /// <returns>Container</returns>
        public static Container.Container CreateNewContainer()
        {
            var container = Instantiate(Instance.containerPrefab).GetComponent<Container.Container>();

            return container;
        }

        /// <summary>
        /// Create new window
        /// </summary>
        /// <param name="title">Title of window</param>
        /// <param name="windowSize">Size of window</param>
        /// <returns>Window</returns>
        public static Window.Window CreateNewWindow(string title, Vector2 windowSize = default)
        {
            var window = Instantiate(Instance.windowPrefab, Instance.windowCanvas).GetComponent<Window.Window>();
            
            window.SetWindowTitle(title);
            window.SetWindowSize(windowSize.x, windowSize.y);

            return window;
        }
    }
}
