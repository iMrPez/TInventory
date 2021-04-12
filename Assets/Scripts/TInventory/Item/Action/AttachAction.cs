using UnityEngine;

namespace TInventory.Item.Action
{
    public class AttachAction : IItemAction
    {

        public bool CanAct(AItem heldItem = null, AItem itemAtTouch = null, Container.Container openContainer = null)
        {
            var actionSlot = Inventory.GetActionSlotAt(InputHandler.GetCursorPosition());

            return !(actionSlot is null || !actionSlot.CanAttach(heldItem));

        }

        public bool Act(AItem heldItem = null, AItem itemAtTouch = null, Container.Container openContainer = null)
        {
            var actionSlot = Inventory.GetActionSlotAt(InputHandler.GetCursorPosition());

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
