using UnityEngine;

namespace TInventory.Item
{
    public interface IItemAction
    {
        bool CanAct(AItem heldItem = null, AItem itemAtTouch = null, TInventory.Container.Container openContainer = null);

        bool Act(AItem heldItem = null, AItem itemAtTouch = null, TInventory.Container.Container openContainer = null);

        Color GetActionColor();
    }
}
