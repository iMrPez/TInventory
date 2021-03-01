using System;
using System.Collections;
using System.Collections.Generic;
using TInventory.Container;
using UnityEngine;
using UnityEngine.UI;

namespace TInventory.Window
{
    public class Window : MonoBehaviour
    {

        /// <summary>
        /// Toggle for if the window can be moved or not.
        /// </summary>
        public bool isLocked = false;

        /// <summary>
        /// If checked, any containers set in the currently added containers will be shown
        /// </summary>
        [SerializeField]
        [Header("Settings")] 
        private bool startWithContainers;

        [SerializeField]
        private List<ContainerData> startContainers = new List<ContainerData>();
        
        /// <summary>
        /// List of currently show containers
        /// </summary>
        private List<Container.Container> containers = new List<Container.Container>();

        /// <summary>
        /// Container Rect Transform.
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// Container scroll viewport.
        /// </summary>
        [SerializeField]
        private ScrollRect scrollView;
        
        /// <summary>
        /// Container content, used as parent for slots and items.
        /// </summary>
        [SerializeField]
        private RectTransform windowContent;

        [SerializeField]
        private VerticalLayoutGroup layoutGroup;
        
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            
            windowContent.localPosition = Vector3.zero;
        }

        private void Start()
        {
            InitializeStartContainers();
        }

        /// <summary>
        /// TODO ADD SUMMARY
        /// </summary>
        private void InitializeStartContainers()
        {
            if (startWithContainers)
            {
                foreach (var containerData in startContainers)
                {

                    Debug.Log(containerData);
                    var container = Inventory.CreateNewContainer();
                    container.Initialize(containerData);

                    AddContent(container.GetComponent<RectTransform>());
                }
            }
        }

        /// <summary>
        /// Gets Containers RectTransform.
        /// </summary>
        /// <returns>Return Container's RectTransform.</returns>
        public RectTransform GetRect() => rectTransform;
        
        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        private void SetWindowSize(float width, float height)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        private void SetContentSize(float width, float height)
        {
            windowContent.sizeDelta = new Vector2(width, height);
        }
        
        // TODO make auto sizing.
        
        // TODO ADD SUMMARY
        private void IncreaseContentSize(float height)
        {
            var newSize = windowContent.sizeDelta + new Vector2(0, height);

            SetContentSize(newSize.x, newSize.y);
        }
        
        // TODO ADD SUMMARY
        private void DecreaseContentSize(float height)
        {
            var newSize = windowContent.sizeDelta - new Vector2(0, height);

            SetWindowSize(newSize.x, newSize.y);
        }

        public void HeaderClicked()
        {
            Debug.Log("Header Clicked!");
            StartCoroutine(DraggingHeader());
        }

        private IEnumerator DraggingHeader()
        {
            while (Input.GetMouseButton(0))
            {
                Debug.Log("Testin");
                yield return null;
            }
        }
        
        /// <summary>
        /// Moves the container's viewport if an item is being held near and edge of the viewport.
        /// </summary>
        public void UpdateViewport()
        {
            var value = NormalizeValue(
                scrollView.viewport.transform.position.y - Input.mousePosition.y,
                scrollView.viewport.rect.height);

            if (value > TInventory.Inventory.Instance.scrollDownAt)
            {
                scrollView.verticalNormalizedPosition =
                    Mathf.Clamp01(scrollView.verticalNormalizedPosition - TInventory.Inventory.Instance.scrollSpeed * Time.deltaTime);
            }
            else if (value < TInventory.Inventory.Instance.scrollUpAt)
            {
                scrollView.verticalNormalizedPosition =
                    Mathf.Clamp01(scrollView.verticalNormalizedPosition + TInventory.Inventory.Instance.scrollSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Adds content to window
        /// </summary>
        /// <param name="contentToAdd"></param>
        public void AddContent(RectTransform contentToAdd)
        {
            if (contentToAdd is null)
            {
                Debug.LogError("Can't add content without a RectTransform.");
                return;
            }

            // Set content's parent to the window
            contentToAdd.transform.SetParent(windowContent);
            
            IncreaseContentSize(contentToAdd.rect.height + layoutGroup.spacing);
        }
        
        /// <summary>
        /// Set if the window is locked in place or not.
        /// </summary>
        /// <param name="value">Locked</param>
        public void SetLock(bool value)
        {
            isLocked = value;
        }
        
        /// <summary>
        /// Normalizes input Value.
        /// </summary>
        /// <param name="value">Value to normalize</param>
        /// <param name="max">Max value</param>
        /// <returns>Returns normalized Value</returns>
        private float NormalizeValue(float value, float max)
        {
            return Mathf.Clamp01(value / max);
        }
    }
}
