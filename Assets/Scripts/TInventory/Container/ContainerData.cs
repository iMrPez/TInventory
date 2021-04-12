using UnityEngine;

namespace TInventory.Container
{
    [CreateAssetMenu(fileName = "ContainerData", menuName = "Inventory/Container")]
    public class ContainerData : ScriptableObject
    {
        public string containerName;

        public Filter.Filter filter;

        private int[] flattenedContainer = new int[1];

        public int[,] Container => ExpandArray(flattenedContainer, Width, Height);

        // TODO EDIT
        public int Width = 1;
        public int Height = 1;

        public void SetContainer(int[,] container)
        {
            
            flattenedContainer = FlattenArray(container);

            Width = container.GetLength(0);
            Height = container.GetLength(1);
        }
        
        public int[] FlattenArray(int[,] arrayToConvert)
        {
            int[] convertedArray = new int[arrayToConvert.Length];

            int currentPosition = 0;

            foreach (var i in arrayToConvert)
            {
                convertedArray[currentPosition] = i;
                currentPosition++;
            }

            return convertedArray;
        }
        
        public int[,] ExpandArray(int[] arrayToConvert, int width, int height)
        {
            int[,] convertedArray = new int[width, height];

            int currentCount = 0;

            for (int currentWidth = 0; currentWidth < width; currentWidth++)
            for (int currentHeight = 0; currentHeight < height; currentHeight++)
            {
                if(currentCount >= arrayToConvert.Length) continue;
                
                convertedArray[currentWidth, currentHeight] = arrayToConvert[currentCount];
                currentCount++;
            }

            return convertedArray;
        }
    }

}
