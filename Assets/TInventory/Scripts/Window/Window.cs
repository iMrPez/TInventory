using System;
using System.Collections.Generic;
using TInventory.Container;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TInventory.Window
{

    public delegate void Notify();
    
    [RequireComponent(typeof(RectTransform))]
    public class Window : MonoBehaviour
    {

        public event Notify WindowClosedHandler; 
        public event Notify WindowShownHandler; 
        
        /// <summary>
        /// Toggle for if the window can be moved or not.
        /// </summary>
        private bool _isLocked;
        
        /// <summary>
        /// Will hide window instead of fully deleting and saving the window.
        /// </summary>
        [Header("Settings")] 
        public bool hideWindowOnClose;
        
        [SerializeField] private bool isInventoryWindow;

        /// <summary>
        /// If checked, any containers set in the currently added containers will be shown
        /// </summary>
        [SerializeField] private bool _startWithContainers;

        /// <summary>
        /// List of containers that the window should open with
        /// </summary>
        [SerializeField] private List<StartContainer> _startContainers = new List<StartContainer>();
        
        private RectTransform _rectTransform;

        /// <summary>
        /// Container scroll viewport.
        /// </summary>
        [SerializeField] private ScrollRect _scrollView;
        
        /// <summary>
        /// Container content, used as parent for slots and items.
        /// </summary>
        [SerializeField] private RectTransform _windowContent;
        
        private VerticalLayoutGroup _layoutGroup;
        
        [SerializeField] private TextMeshProUGUI _titleText;

        public readonly List<Container.Container> Containers = new List<Container.Container>();

        
        public Vector2 WindowSize => _rectTransform.sizeDelta;
        
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _layoutGroup = _windowContent.GetComponent<VerticalLayoutGroup>();
            
            if(_scrollView is null) Debug.LogError("Scroll View is not set!", this);
            if(_windowContent is null) Debug.LogError("Window content is not set!", this);
            if(_layoutGroup is null) Debug.LogError("Layout group is not placed on content!", _windowContent);
            
            _windowContent.localPosition = Vector3.zero;
        }

        private void Start()
        {
            InitializeStartContainers();
            
            if (isInventoryWindow)
            {
                Inventory.Instance.InventoryWindow = this;
            }
        }

        /// <summary>
        /// Initializes the list of start containers if the option is checked
        /// </summary>
        private void InitializeStartContainers()
        {
            if (_startWithContainers)
            {
                foreach (var startContainer in _startContainers)
                {
                    var container = Inventory.CreateNewContainer();

                    container.containerId = startContainer.id;
                    
                    container.InitializeContainer(startContainer.containerData);

                    AddContainer(container);
                }
            }
        }

        /// <summary>
        /// Gets Containers RectTransform.
        /// </summary>
        /// <returns>Return Container's RectTransform.</returns>
        public RectTransform GetRect() => _rectTransform;

        /// <summary>
        /// Sets window title
        /// </summary>
        /// <param name="text"></param>
        public void SetWindowTitle(string text)
        {
            _titleText.text = text;
        }
        
        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public void SetWindowSize(float width, float height)
        {
            _rectTransform.sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// Sets content size
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        private void SetContentSize(float width, float height)
        {
            _windowContent.sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// Check if the window is locked in place
        /// </summary>
        /// <returns></returns>
        public bool IsLocked() => _isLocked;

        /// <summary>
        /// Increase the size of the content
        /// </summary>
        /// <param name="height">Height</param>
        /// <param name="width">Width</param>
        private void IncreaseContentSize(float height, float width)
        {
            var newSize = _windowContent.sizeDelta + new Vector2(width, height);

            SetContentSize(newSize.x, newSize.y);
        }

        /// <summary>
        /// Decrease the size of the content
        /// </summary>
        /// <param name="height">Height</param>
        /// <param name="width">Width</param>
        private void DecreaseContentSize(float height, float width)
        {
            var newSize = _windowContent.sizeDelta - new Vector2(width, height);

            SetWindowSize(newSize.x, newSize.y);
        }

        /// <summary>
        /// Moves the container's viewport if an item is being held near and edge of the viewport.
        /// </summary>
        public void UpdateViewport()
        {
            var value = NormalizeValue(
                _scrollView.viewport.transform.position.y - Input.mousePosition.y,
                _scrollView.viewport.rect.height);

            if (value > Inventory.Instance.scrollDownAt)
            {
                _scrollView.verticalNormalizedPosition =
                    Mathf.Clamp01(_scrollView.verticalNormalizedPosition - Inventory.Instance.scrollSpeed * Time.deltaTime);
            }
            else if (value < Inventory.Instance.scrollUpAt)
            {
                _scrollView.verticalNormalizedPosition =
                    Mathf.Clamp01(_scrollView.verticalNormalizedPosition + Inventory.Instance.scrollSpeed * Time.deltaTime);
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
            contentToAdd.transform.SetParent(_windowContent);
            
            IncreaseContentSize(contentToAdd.rect.height + _layoutGroup.spacing, 0);
        }

        /// <summary>
        /// Add Container to window
        /// </summary>
        /// <param name="container">Container</param>
        public void AddContainer(Container.Container container)
        {
            if (container is null)
            {
                Debug.LogError("Can't add content without a RectTransform.");
                return;
            }

            // Set content's parent to the window
            container.transform.SetParent(_windowContent);
            
            Containers.Add(container);
            
            IncreaseContentSize(container.RectTransform.rect.height + _layoutGroup.spacing, 0);
        }
        
        /// <summary>
        /// Set if the window is locked in place or not.
        /// </summary>
        /// <param name="value">Locked</param>
        public void SetLock(bool value)
        {
            _isLocked = value;
        }

        
        /// <summary>
        /// Hides Window
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows Window
        /// </summary>
        public void Show()
        {
            WindowShownHandler?.Invoke();
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Closes Window
        /// </summary>
        public void Close()
        {
            WindowClosedHandler?.Invoke();
            
            if (!hideWindowOnClose)
            {
                Destroy(gameObject);
            }
            else
            {
                Hide();
            }
            
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

    [Serializable]
    public struct StartContainer
    {
        public int id;
        public ContainerData containerData;
        
    }
}
