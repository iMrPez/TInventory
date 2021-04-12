using UnityEngine;

namespace TInventory.Item.Action
{
    public interface IItemAction
    {

        public bool CanAct(AItem heldItem = null, AItem itemAtTouch = null, TInventory.Container.Container openContainer = null);

        public bool Act(AItem heldItem = null, AItem itemAtTouch = null, TInventory.Container.Container openContainer = null);

        public Color GetActionColor();
    }
}
