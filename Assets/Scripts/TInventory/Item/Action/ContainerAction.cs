using TInventory.Container;
using UnityEngine;

namespace TInventory.Item.Action
{
    public class ContainerAction : IItemAction
    {

        private (Vector2 slot, ContainerGroup containerGroup) _location;
        
        public bool CanAct(Item heldItem = null, Item itemAtTouch = null, Container.Container openContainer = null)
        {
            if (!(itemAtTouch is ContainerItem containerItem)) return false;

            var heldContainerItem = heldItem as ContainerItem;
            
            if (!(heldContainerItem is null))
            {
                if (heldContainerItem.IsChildOf(itemAtTouch)) return false;
            }
            
            return containerItem.CanAddItem(heldItem, out _location);
        }

        public bool Act(Item heldItem = null, Item itemAtTouch = null, Container.Container openContainer = null)
        {
            if (!(itemAtTouch is ContainerItem containerItem)) return false;

            containerItem.AddItem(heldItem, _location.slot, _location.containerGroup);

            return true;
        }

        public Color GetActionColor()
        {
            return Color.blue;
        }
    }
}