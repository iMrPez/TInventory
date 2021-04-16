using UnityEngine;

namespace TInventory.Item.Action
{
    public class PlaceAction : IItemAction
    {

        public bool CanAct(Item heldItem = null, Item itemAtTouch = null, TInventory.Container.Container openContainer = null)
        {
            var containerAtTouch = InventoryUtility.GetContainerAt(Input.mousePosition);

            if (containerAtTouch is null) return false;
            
            var slotGroup = containerAtTouch.GetSlotFromPosition(Input.mousePosition);
            
            return !(heldItem is null) && containerAtTouch.CanPlaceItemAt(slotGroup.Slot, slotGroup.ContainerGroup,
                heldItem);
        }

        public bool Act(Item heldItem = null, Item itemAtTouch = null, TInventory.Container.Container openContainer = null)
        {
            var containerAtTouch = InventoryUtility.GetContainerAt(Input.mousePosition);
            
            if (containerAtTouch is null || heldItem is null) return false;
            
            var slotGroup = containerAtTouch.GetSlotFromPosition(Input.mousePosition);
            
            containerAtTouch.PlaceItemAt(slotGroup.Slot, slotGroup.ContainerGroup, heldItem);

            heldItem.ResetColor();
            
            return true;
        }

        public Color GetActionColor()
        {
            return Color.green;
        }
    }
}
