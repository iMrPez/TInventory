using System.Collections.Generic;
using TInventory.Container;
using TInventory.ContextMenu.Action;
using TInventory.Item.Action;
using UnityEngine;

namespace TInventory.Item
{
    public class ContainerItem : Item
    {

        [SerializeField] private ContainerData _containerData;

        public ContainerData ContainerData => _containerData;

        public override List<IItemAction> GetReleaseActions()
        {
            return new List<IItemAction>()
            {
                new AttachAction(),
                new PlaceAction()
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
