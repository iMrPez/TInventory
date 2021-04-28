using UnityEngine;

namespace TInventory.Container
{
    [CreateAssetMenu(fileName = "ContainerData", menuName = "TInventory/Container")]
    public class ContainerData : ScriptableObject
    {
        public string containerName;

        public Filter.Filter filter;

        public int[] Container;

        public int GetContainerGroupAt(int x, int y)
        {
            return Container[x + y * Width];
        }

        public int Width = 1;
        public int Height = 1;
        
    }
}
