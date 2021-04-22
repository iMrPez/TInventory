using UnityEngine;

namespace TInventory.Item.Action
{
    public class ContainerAction : IItemAction
    {
        public bool CanAct(Item heldItem = null, Item itemAtTouch = null, Container.Container openContainer = null)
        {
            return false;
        }

        public bool Act(Item heldItem = null, Item itemAtTouch = null, Container.Container openContainer = null)
        {
            return false;
        }

        public Color GetActionColor()
        {
            return Color.white;
        }
    }
}