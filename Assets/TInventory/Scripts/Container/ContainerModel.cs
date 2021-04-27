using System.Collections.Generic;

namespace TInventory.Container
{
    public struct ContainerModel
    {
        public string[] itemJson;

        public ContainerData containerData;

        public ContainerModel(string[] itemJson, ContainerData containerData)
        {
            this.itemJson = itemJson;
            this.containerData = containerData;
        }
    }
}