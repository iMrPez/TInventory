using System.Collections;
using Inventory.Item;
using TInventory.Container;
using UnityEngine;

namespace TInventory
{
    /// <summary>
    /// Class handles pick up, holding, and placing Items in Containers. 
    /// </summary>
    public class ItemHolder : MonoBehaviour
    {

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
        /// Item's default color. TODO replace with rarity specific colors.
        /// </summary>
        [SerializeField]
        private Color defaultColor;
        
        /// <summary>
        /// The item currently being held.
        /// </summary>
        private AItem heldItem;
        
        /// <summary>
        /// The last time a touch occured.
        /// </summary>
        private float lastTouchTime;
        
        /// <summary>
        /// The held item's position before it was picked up.
        /// </summary>
        private Vector3 startPos;

        /// <summary>
        /// TODO MAKE SUMMARY
        /// </summary>
        private ContainerGroup startContainerGroup;
        
        /// <summary>
        /// The held item's rotation before it was picked up.
        /// </summary>
        private bool startRotation;
        
        /// <summary>
        /// Action to be done when the item is released.
        /// </summary>
        private IItemAction releasedAction;

        /// <summary>
        /// Clicked Item
        /// </summary>
        private AItem clickedItem;

        private Vector3 clickedPosition = Vector3.zero;
        
        private void Update()
        {

            // TODO RETURN IF INVENTORY IS NOT OPEN
            
            // TODO REMOVE - for testings
            if (Input.GetKeyDown(KeyCode.R))
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
            if (Input.GetMouseButtonDown(0))
            {
                clickedItem = TInventory.Inventory.GetItemAt(Input.mousePosition);
                
                if (clickedItem is null) return false;
                
                clickedPosition = Input.mousePosition;
                
                lastTouchTime = Time.time;
            }

            if (Input.GetMouseButton(0))
            {
                return (Time.time - lastTouchTime > touchHoldTime || 
                        Vector3.Distance(Input.mousePosition, clickedPosition) > TInventory.Inventory.Instance.slotSize / 2);
            }
            
            return false;
        }
        
        /// <summary>
        /// If there is an item at the touch's position, hold it.
        /// </summary>
        private void HoldItemAtTouch()
        {
            if (!(clickedItem is null))
            {
                heldItem = clickedItem;
                startPos = clickedItem.slotPosition;
                startContainerGroup = clickedItem.containerGroup;
                startRotation = clickedItem.IsRotated();
                clickedItem.containerGroup.parentContainer.items.Remove(clickedItem);
                
                StartCoroutine(HoldItem());
            }
        }
        
        /// <summary>
        /// Manages item well it's being held.
        /// </summary>
        private IEnumerator HoldItem()
        {
            // Get currently open container
            var containerAtTouch = TInventory.Inventory.GetContainer(Input.mousePosition);
            
            AItem itemAtTouch = null;
            
            while (Input.GetMouseButton(0))
            {
                if (IsHoldingItem())
                {
                    containerAtTouch = TInventory.Inventory.GetContainer(Input.mousePosition);

                    Debug.Log(containerAtTouch);
                    
                    TInventory.Inventory.GetWindowAtMousePosition().UpdateViewport();
                    
                    itemAtTouch = TInventory.Inventory.GetItemAt(Input.mousePosition, heldItem.gameObject);

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
        private void UpdateHeldItem(global::TInventory.Container.Container openContainer, AItem itemAtTouch)
        {
            heldItem.transform.position = (Vector2) Input.mousePosition;

            heldItem.transform.SetParent(TInventory.Inventory.Instance.transform);
            heldItem.transform.SetAsLastSibling();

            releasedAction = null;
            
            bool colorSet = false;
            foreach (var itemAction in heldItem.itemReleaseActions)
            {
                if (itemAction.CanAct(heldItem, itemAtTouch, openContainer))
                {
                    
                    colorSet = true;
                    heldItem.SetBackgroundColor(itemAction.GetActionColor());
                    releasedAction = itemAction;
                }
            }

            if (!colorSet)
            {
                heldItem.SetBackgroundColor(unplaceableColor);
            }
        }
        
        /// <summary>
        /// Attempts to place, stack, or reset item depending on where the item is being is at.
        /// </summary>
        /// <param name="openContainer">Currently Open Container</param>
        /// <param name="itemAtTouch"></param>
        private void HeldItemReleased(global::TInventory.Container.Container openContainer = null, AItem itemAtTouch = null)
        {
            // Check if there is an action to do
            if (!(releasedAction is null))
            {
                // Do the action and if it fails, return the held item to previous location
                if (!releasedAction.Act(heldItem, itemAtTouch, openContainer))
                {
                    ReturnHeldItem();
                }
            }
            else
            {
                ReturnHeldItem();
            }
            
            // Reset background color
            heldItem.SetBackgroundColor(defaultColor);

            ResetHold();
        }
        
        /// <summary>
        /// Check if there is an item currently being held.
        /// </summary>
        /// <returns>Returns true if an item is currently being held.</returns>
        public bool IsHoldingItem()
        {
            return !(heldItem is null);
        }
        
        /// <summary>
        /// Rotate held item
        /// </summary>
        public void RotateHeldItem()
        {
            if(heldItem != null)
                heldItem.Rotate();
        }
        
        /// <summary>
        /// Returns held item to previous position and rotation.
        /// </summary>
        private void ReturnHeldItem()
        {
            heldItem.containerGroup.parentContainer.PlaceItemAt(startPos, startContainerGroup, heldItem);
            if (startRotation != heldItem.IsRotated()) heldItem.Rotate();
        }

        /// <summary>
        /// Resets hold item fields.
        /// </summary>
        private void ResetHold()
        {
            heldItem = null;
        }

        
    }
}