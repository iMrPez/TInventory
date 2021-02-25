using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TInventory
{
    public class Window : MonoBehaviour
    {

        /// <summary>
        /// Toggle for if the window can be moved or not.
        /// </summary>
        public bool isLocked = false;
        
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

        public GameObject TestContent;
        
        // TEST METHOD
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                var test = Instantiate(TestContent);
                
                AddContent(test);
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

        // TODO ADD SUMMARY
        public void AddContent(GameObject contentToAdd)
        {
            var contentRect = contentToAdd.GetComponent<RectTransform>();
            
            if (contentRect is null)
            {
                Debug.LogError("Can't add content without a RectTransform.");
                return;
            }

            // Set content's parent to the window
            contentToAdd.transform.SetParent(windowContent);
            
            IncreaseContentSize(contentRect.rect.height + layoutGroup.spacing);
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
