using System;
using TInventory.ContextMenu.Action;
using TInventory.Item;
using UnityEngine;
using UnityEngine.UI;

namespace TInventory.ContextMenu
{
    [RequireComponent(typeof(Button))]
    public class ContextMenuOption : MonoBehaviour
    {
        private Text optionText;

        private Button optionButton;

        private void Awake()
        {
            optionText = GetComponentInChildren<Text>();
            optionButton = GetComponent<Button>();

            if (optionText is null) Debug.LogError("No text found in children.", this);
            if (optionButton is null) Debug.LogError("No button found in children.", this);
        }

        public void InitializeOption(IAction action)
        {
            //if(action == null) return; TODO REMOVE
            
            optionText.text = action.GetName();

            
            
            if (!action.CanAct())
            {
                optionButton.enabled = false;
            }
            else
            {
                optionButton.onClick.AddListener(action.Act);
            }
            
        }
    }
}
