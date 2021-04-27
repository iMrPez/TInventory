using UnityEngine;

namespace TInventory.Item.Action
{
    public class StackAction : IItemAction
    {
        
        public bool CanAct(Item heldItem = null, Item itemAtTouch = null, TInventory.Container.Container openContainer = null)
        {
            if (heldItem is null || itemAtTouch is null) return false;
            
            // Check if item is not already at max count.
            if (itemAtTouch.GetCount() >= itemAtTouch.Data.maxCount)
                return false;

            // Check if the two items are of the same type
            return itemAtTouch.Data.id == heldItem.Data.id;
        }

        public bool Act(Item heldItem = null, Item itemAtTouch = null, TInventory.Container.Container openContainer = null)
        {
            if ((itemAtTouch is null) || (heldItem is null)) return false;
            
            var remainder = GetRemainder(itemAtTouch.GetCount(), heldItem.GetCount(), itemAtTouch.Data.maxCount);

            if (remainder > 0)
            {
                heldItem.SetCount(remainder);
                itemAtTouch.SetCount(); // Setting count to max
            }
            else
            {
                itemAtTouch.SetCount(itemAtTouch.GetCount() + heldItem.GetCount());
                heldItem.Destroy();
            }

            
            
            return remainder <= 0;
        }

        public Color GetActionColor()
        {
            return Color.yellow;;
        }

        public int GetRemainder(int value, int valueToAdd, int max)
        {
            return value + valueToAdd - max;
        }
    }
}
