using UnityEngine;

namespace TInventory.Item.Action
{
    public class AttachAction : IItemAction
    {

        public bool CanAct(Item heldItem = null, Item itemAtTouch = null, Container.Container openContainer = null)
        {
            var actionSlot = InventoryUtility.GetActionSlotAt(InputHandler.GetCursorPosition());

            return !(actionSlot is null || !actionSlot.CanAttach(heldItem));

        }

        public bool Act(Item heldItem = null, Item itemAtTouch = null, Container.Container openContainer = null)
        {
            var actionSlot = InventoryUtility.GetActionSlotAt(InputHandler.GetCursorPosition());

            if (actionSlot is null) return false;
            
            actionSlot.Attach(heldItem);

            return true;
        }

        public Color GetActionColor()
        {
            return Color.blue;
        }
    }
}
