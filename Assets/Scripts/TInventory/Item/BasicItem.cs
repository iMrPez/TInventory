using System;
using UnityEngine;

namespace Inventory.Item
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
        }
    }
}
