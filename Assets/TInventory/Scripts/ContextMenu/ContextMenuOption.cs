using TInventory.ContextMenu.Action;
using UnityEngine;
using UnityEngine.UI;

namespace TInventory.ContextMenu
{
    [RequireComponent(typeof(Button))]
    public class ContextMenuOption : MonoBehaviour
    {
        private Text _optionText;

        private Button _optionButton;

        private void Awake()
        {
            _optionText = GetComponentInChildren<Text>();
            _optionButton = GetComponent<Button>();

            if (_optionText is null) Debug.LogError("No text found in children.", this);
            if (_optionButton is null) Debug.LogError("No button found in children.", this);
        }

        /// <summary>
        /// Initialize Option
        /// </summary>
        /// <param name="option">Option Action</param>
        public void InitializeOption(IOption option)
        {

            _optionText.text = option.GetName();

            
            if (!option.CanAct())
            {
                _optionButton.enabled = false;
            }
            else
            {
                _optionButton.onClick.AddListener(option.Act);
            }
            
        }
    }
}
