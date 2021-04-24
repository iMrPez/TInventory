using System.Collections;
using TInventory.Container;
using TInventory.Item.Action;
using UnityEngine;

namespace TInventory.Item
{
    /// <summary>
    /// Class handles pick up, holding, and placing Items in Containers. 
    /// </summary>
    public class ItemMover : MonoBehaviour
    {

        public static ItemMover instance;
        
        /// <summary>
        /// Amount of time to hold down on an item before it is picked up.
        /// </summary>
        [Header("Settings")] [Tooltip("Time spent holding down to pick up item.")] [SerializeField]
        private float touchHoldTime;

        /// <summary>
        /// Color of the item when it's currently unplaceable.
        /// </summary>
        [Header("Colors")] [SerializeField]
        private Color unplaceableColor;
        
        /// <summary>
        /// The item currently being held
        /// </summary>
        private Item _heldItem;
        
        /// <summary>
        /// The last time a touch occured
        /// </summary>
        private float _lastTouchTime;
        
        /// <summary>
        /// The held item's position before it was picked up
        /// </summary>
        private Vector3 _startPos;

        /// <summary>
        /// The held item's containerGroup before it was picked up
        /// </summary>
        private ContainerGroup _startContainerGroup;
        
        /// <summary>
        /// The held item's rotation before it was picked up
        /// </summary>
        private bool _startRotation;
        
        /// <summary>
        /// Action to be done when the item is released
        /// </summary>
        private IItemAction _releasedAction;

        /// <summary>
        /// Clicked Item
        /// </summary>
        private Item _clickedItem;

        private Vector3 _clickedPosition = Vector3.zero;
        
        public static event ItemMovedDelegate ItemPlacedHandler;

        public static event ItemMovedDelegate ItemPickedUpHandler;


        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            
            if (InputHandler.GetRotateButtonDown())
            {
                RotateHeldItem();
            }

            // Check if the user is attempting to hold an item.
            if (IsAttemptingToHoldItem() && !IsHoldingItem())
            {
                HoldItemAtTouch();
            }
        }

        /// <summary>
        /// Checks if the user is attempting to hold an item
        /// </summary>
        private bool IsAttemptingToHoldItem()
        {
            if (InputHandler.GetPrimaryButtonDown())
            {

                var itemAtClick = _clickedItem;
                
                _clickedItem = InventoryUtility.GetItemAt(InputHandler.GetCursorPosition());
                
                if (_clickedItem is null) return false;

                // Check if double clicking item
                if (itemAtClick == _clickedItem && Time.time - _lastTouchTime < 0.3f)
                {
                    var action = itemAtClick.GetDoubleClickAction();
                    if(!(action is null) && action.CanAct()) action.Act();
                }
                
                _clickedPosition = Input.mousePosition;
                
                _lastTouchTime = Time.time;
            }

            if (InputHandler.GetPrimaryButton())
            {
                return (Time.time - _lastTouchTime > touchHoldTime || 
                        Vector3.Distance(InputHandler.GetCursorPosition(), _clickedPosition) > Inventory.Instance.slotSize / 2);
            }
            
            return false;
        }
        
        /// <summary>
        /// If there is an item at the touch's position, hold it.
        /// </summary>
        private void HoldItemAtTouch()
        {
            if (_clickedItem is null) return;
            
            _heldItem = _clickedItem;
            // Trigger events
            ItemPickedUpHandler?.Invoke(_heldItem);
        
            _startPos = _heldItem.SlotPosition;
            _startContainerGroup = _heldItem.ContainerGroup;
            _startRotation = _heldItem.IsRotated;

            _heldItem.ContainerGroup?.parentContainer.RemoveItem(_heldItem);

            _heldItem.AttachedSlot?.Detach();

            StartCoroutine(HoldItem());
            
        }
        
        /// <summary>
        /// Manages item well it's being held.
        /// </summary>
        private IEnumerator HoldItem()
        {
            // Get currently open container
            var containerAtTouch = InventoryUtility.GetContainerAt(InputHandler.GetCursorPosition());
            
            Item itemAtTouch = null;
            
            while (InputHandler.GetPrimaryButton())
            {
                if (IsHoldingItem())
                {
                    containerAtTouch = InventoryUtility.GetContainerAt(InputHandler.GetCursorPosition());

                    var window = InventoryUtility.GetWindowAtMousePosition();

                    window?.UpdateViewport();

                    itemAtTouch = InventoryUtility.GetItemAt(InputHandler.GetCursorPosition(), _heldItem.gameObject);

                    UpdateHeldItem(containerAtTouch, itemAtTouch);
                }
                yield return null;
            }
            
            HeldItemReleased(containerAtTouch, itemAtTouch);
        }
        
        /// <summary>
        /// Updates background color and position of held item.
        /// </summary>
        /// <param name="openContainer">Currently Open Container</param>
        private void UpdateHeldItem(TInventory.Container.Container openContainer, Item itemAtTouch)
        {
            _heldItem.transform.position = (Vector2) InputHandler.GetCursorPosition();

            _heldItem.transform.SetParent(Inventory.Instance.windowCanvas);
            _heldItem.transform.SetAsLastSibling();

            _releasedAction = null;
            
            bool colorSet = false;
            foreach (var itemAction in _heldItem.GetReleaseActions())
            {
                if (itemAction.CanAct(_heldItem, itemAtTouch, openContainer))
                {
                    
                    colorSet = true;
                    _heldItem.SetBackgroundColor(itemAction.GetActionColor());
                    _releasedAction = itemAction;
                }
            }

            if (!colorSet)
            {
                _heldItem.SetBackgroundColor(unplaceableColor);
            }
        }
        
        /// <summary>
        /// Attempts to place, stack, or reset item depending on where the item is being is at.
        /// </summary>
        /// <param name="openContainer">Currently Open Container</param>
        /// <param name="itemAtTouch"></param>
        private void HeldItemReleased(TInventory.Container.Container openContainer = null, Item itemAtTouch = null)
        {
            // Check if there is an action to do
            if (!(_releasedAction is null))
            {
                // Do the action and if it fails, return the held item to previous location
                if (!_releasedAction.Act(_heldItem, itemAtTouch, openContainer))
                {
                    ReturnHeldItem();
                }
                else
                {
                    ItemPlacedHandler?.Invoke(_heldItem);
                }
            }
            else
            {
                ReturnHeldItem();
            }

            ResetHold();
        }
        
        /// <summary>
        /// Check if there is an item currently being held.
        /// </summary>
        /// <returns>Returns true if an item is currently being held.</returns>
        public bool IsHoldingItem()
        {
            return !(_heldItem is null);
        }
        
        /// <summary>
        /// Rotate held item
        /// </summary>
        public void RotateHeldItem()
        {
            if(_heldItem != null)
                _heldItem.Rotate();
        }
        
        /// <summary>
        /// Returns held item to previous position and rotation.
        /// </summary>
        private void ReturnHeldItem()
        {
            // Rotate to original rotation before grabbing
            if (_startRotation != _heldItem.IsRotated) _heldItem.Rotate();

            if (!(_heldItem.AttachedSlot is null))
            {
                _heldItem.AttachedSlot.Attach(_heldItem);
            }
            else if (!(_heldItem.ContainerGroup is null))
            {
                _heldItem.ContainerGroup.parentContainer.PlaceItemAt(_startPos, _startContainerGroup, _heldItem);
            }
            
            ItemPlacedHandler?.Invoke(_heldItem);
        }

        /// <summary>
        /// Resets hold item fields.
        /// </summary>
        private void ResetHold()
        {
            _heldItem = null;
        }
        
    }
}