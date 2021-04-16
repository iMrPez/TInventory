using UnityEngine;

namespace TInventory.Container
{
    [CreateAssetMenu(fileName = "ContainerData", menuName = "Inventory/Container")]
    public class ContainerData : ScriptableObject
    {
        public string containerName;

        public Filter.Filter filter;

        private int[] _flattenedContainer = new int[1];

        public int[,] Container => ExpandArray(_flattenedContainer, Width, Height);

        public int Width = 1;
        public int Height = 1;

        /// <summary>
        /// Sets container
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(int[,] container)
        {
            _flattenedContainer = FlattenArray(container);

            Width = container.GetLength(0);
            Height = container.GetLength(1);
        }
        
        
        /// <summary>
        /// Flatten container to a saveable state
        /// </summary>
        /// <param name="arrayToConvert"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Expand container into a 2d matrix
        /// </summary>
        /// <param name="flattenedContainer">Flattened Container</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Container</returns>
        public int[,] ExpandArray(int[] flattenedContainer, int width, int height)
        {
            int[,] convertedArray = new int[width, height];

            int currentCount = 0;

            for (int currentWidth = 0; currentWidth < width; currentWidth++)
            for (int currentHeight = 0; currentHeight < height; currentHeight++)
            {
                if(currentCount >= flattenedContainer.Length) continue;
                
                convertedArray[currentWidth, currentHeight] = flattenedContainer[currentCount];
                currentCount++;
            }

            return convertedArray;
        }
    }

}
