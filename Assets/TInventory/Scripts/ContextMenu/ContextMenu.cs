using System;
using System.Collections;
using System.Collections.Generic;
using TInventory.ContextMenu.Action;
using TInventory.Item;
using UnityEngine;

namespace TInventory.ContextMenu
{
    public class ContextMenu : MonoBehaviour
    {
        
        [SerializeField]
        private GameObject optionPrefab;
        
        private RectTransform _rectTransform;

        private List<ContextMenuOption> _options = new List<ContextMenuOption>();
        
        private float _optionHeight;
        
        public bool IsOpen => gameObject.activeSelf;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            InputHandler.KeyReleasedHandler += Clicked;

            _optionHeight = optionPrefab.GetComponent<RectTransform>().sizeDelta.y;
            
            Hide();
        }

        /// <summary>
        /// Updates the size of the menu to fit all of the options
        /// </summary>
        private void UpdateMenuSize()
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _options.Count * _optionHeight);
        }
        
        
        /// <summary>
        /// Shows menu if user right clicks an item or hides it if user clicks else where
        /// </summary>
        /// <param name="key"></param>
        private void Clicked(InputHandler.Key key)
        {
            if(key == InputHandler.Key.Primary) Hide();
            
            if(key != InputHandler.Key.Secondary || ItemMover.instance.IsHoldingItem()) return;

            var clickedItem = InventoryUtility.GetItemAt(InputHandler.GetCursorPosition());

            if (!(clickedItem is null))
            {
                ClearOptions();
                Show();
                ShowMenu(clickedItem);
            }
        }
        
        /// <summary>
        /// Shows context menu
        /// </summary>
        /// <param name="item">Item</param>
        private void ShowMenu(Item.Item item)
        {
            
            PopulateMenu(item.GetContextMenuActions());

            transform.position = InputHandler.GetCursorPosition();

            StartCoroutine(IsInactive(InputHandler.GetCursorPosition()));
        }

        
        /// <summary>
        /// Hides window if cursor is too far from menu
        /// </summary>
        /// <param name="clickedPosition"></param>
        /// <returns>null</returns>
        private IEnumerator IsInactive(Vector2 clickedPosition)
        {
            Vector2 position = transform.position +
                               new Vector3(_rectTransform.sizeDelta.x / 2, _rectTransform.sizeDelta.y / 2);
            
            while (IsOpen)
            {
                if (Vector2.Distance(position, InputHandler.GetCursorPosition()) > 
                    ((_options.Count - 1) * _optionHeight) + 100)
                {
                    Hide();
                    break;
                }
                
                yield return null;
            }
        }

        /// <summary>
        /// Populate menu with options
        /// </summary>
        /// <param name="actions"></param>
        private void PopulateMenu(List<IOption> actions)
        {
            foreach (var action in actions)
            {
                AddOption(action);
            }
            
            UpdateMenuSize();
        }

        /// <summary>
        /// Add option to menu
        /// </summary>
        /// <param name="option"></param>
        private void AddOption(IOption option)
        {
            var optionObj = Instantiate(optionPrefab, transform).GetComponent<ContextMenuOption>();
            
            if (option is null)
            {
                Debug.LogError("No ContextMenuOption found.");
                return;
            }
            
            _options.Add(optionObj);
            
            optionObj.InitializeOption(option);
        }

        /// <summary>
        /// Clears all options
        /// </summary>
        private void ClearOptions()
        {
            foreach (var option in _options)
            {
                Destroy(option.gameObject);
            }
            
            _options.Clear();
        }
        
        /// <summary>
        /// Show context menu
        /// </summary>
        public void Show()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }

        
        /// <summary>
        /// Hide context menu
        /// </summary>
        public void Hide()
        { 
            ClearOptions();
            gameObject.SetActive(false); 
        }
    }
}
