namespace TInventory.Container
{
    public struct ContainerDataModel
    {
        public string containerName;
        public Filter.Filter filter;
        public int[] flattenedContainer;
        public int width;
        public int height;

        public ContainerDataModel(ContainerData containerData)
        {
            containerName = containerData.containerName;
            filter = containerData.filter;
            flattenedContainer = containerData._flattenedContainer;
            width = containerData.Width;
            height = containerData.Height;
        }
    }
}