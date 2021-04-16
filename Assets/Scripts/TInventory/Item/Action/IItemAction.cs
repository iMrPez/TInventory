using UnityEngine;

namespace TInventory.Item.Action
{
    public interface IItemAction
    {

        public bool CanAct(Item heldItem = null, Item itemAtTouch = null, TInventory.Container.Container openContainer = null);

        public bool Act(Item heldItem = null, Item itemAtTouch = null, TInventory.Container.Container openContainer = null);

        public Color GetActionColor();
    }
}
