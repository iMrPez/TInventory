namespace TInventory.Container
{
    public struct ContainerDataModel
    {
        public string containerName;
        public Filter.Filter filter;
        public int[] container;
        public int width;
        public int height;

        public ContainerDataModel(ContainerData containerData)
        {
            containerName = containerData.containerName;
            filter = containerData.filter;
            container = containerData.Container;
            width = containerData.Width;
            height = containerData.Height;
        }
    }
}