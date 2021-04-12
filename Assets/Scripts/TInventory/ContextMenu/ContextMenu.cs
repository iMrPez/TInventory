using System;
using System.Collections.Generic;
using TInventory.ContextMenu.Action;
using TInventory.Item;
using UnityEngine;

namespace TInventory.ContextMenu
{
    // TODO add summaries 
    public class ContextMenu : MonoBehaviour
    {
        private float optionHeight;
        
        [SerializeField]
        private GameObject optionPrefab;
        
        [SerializeField] [HideInInspector]
        private RectTransform rectTransform;

        private List<ContextMenuOption> options = new List<ContextMenuOption>();
        
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            InputHandler.KeyReleasedHandler += Clicked;

            optionHeight = optionPrefab.GetComponent<RectTransform>().sizeDelta.y;
            
            Hide();
        }


        private void UpdateMenuSize()
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, options.Count * optionHeight);
        }
        
        private void Clicked(InputHandler.Key key)
        {
            
            if(key == InputHandler.Key.Primary) Hide();
            
            if(key != InputHandler.Key.Secondary || ItemMover.instance.IsHoldingItem()) return;

            var clickedItem = Inventory.GetItemAt(InputHandler.GetCursorPosition());

            if (!(clickedItem is null))
            {
                Show();
                ShowMenu(clickedItem);
            }
        }
        
        
        private void ShowMenu(AItem item)
        {

            PopulateMenu(item.contextMenuActions);

            transform.position = InputHandler.GetCursorPosition();
        }

        private void PopulateMenu(List<IAction> actions)
        {
            foreach (var action in actions)
            {
                AddOption(action);
            }
            
            UpdateMenuSize();
        }

        private void AddOption(IAction action)
        {
            var option = Instantiate(optionPrefab, transform).GetComponent<ContextMenuOption>();
            
            if (option is null)
            {
                Debug.LogError("No ContextMenuOption found.");
                return;
            }
            
            options.Add(option);
            
            option.InitializeOption(action);
        }

        private void ClearOptions()
        {
            foreach (var option in options)
            {
                Destroy(option.gameObject);
            }
            
            options.Clear();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        { 
            ClearOptions();
            gameObject.SetActive(false); 
        }
    }
}
