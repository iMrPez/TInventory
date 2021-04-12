using TInventory.ContextMenu.Action;
using TInventory.Item.Action;

namespace TInventory.Item
{
    /// <summary>
    /// Basic misc item that can be stacked.
    /// </summary>
    public class BasicItem : AItem
    {
        private void Start()
        {
            AddItemReleaseActions();
        }

        /// <summary>
        /// Adds inventory item release actions, these are used when an item is picked up and released.
        /// </summary>
        private void AddItemReleaseActions()
        {
            // Add inventory release actions
            itemReleaseActions.Add(new PlaceAction());
            itemReleaseActions.Add(new StackAction());
            itemReleaseActions.Add(new AttachAction());
            
            // Add Context Menu Options
            contextMenuActions.Add(new DeleteAction(this));
        }
    }
}
