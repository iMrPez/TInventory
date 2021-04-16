using System.Collections.Generic;
using TInventory.ContextMenu.Action;
using TInventory.Item.Action;

namespace TInventory.Item
{
    /// <summary>
    /// Basic misc item that can be stacked.
    /// </summary>
    public class BasicItem : Item
    {
        
        public override List<IItemAction> GetReleaseActions()
        {
            
            return new List<IItemAction>()
            {
                new PlaceAction(),
                new StackAction(),
                new AttachAction()
            };
        }

        public override List<IOption> GetContextMenuActions()
        {
            return new List<IOption>()
            {
                new DeleteOption(this)
            };
        }
    }
}
