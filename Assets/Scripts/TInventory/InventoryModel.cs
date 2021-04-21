using System.Collections.Generic;

namespace TInventory
{
    public struct InventoryModel
    {
        public List<string> containerJson;

        public InventoryModel(List<string> containerJson)
        {
            this.containerJson = containerJson;
        }
    }
}