using UnityEngine;

namespace TInventory.Container
{
    public class ContainerGroup : MonoBehaviour
    {
        public int id;
        public Vector2 position;
        public Vector2 size;
        public Container parentContainer;
        
        public RectTransform rectTransform;
        
        private Vector2 currentPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Init(int id, Vector2 position, Vector2 size, TInventory.Container.Container container)
        {
            this.id = id;
            this.position = position;
            currentPosition = position;
            this.size = size;
            parentContainer = container;
        }

        /// <summary>
        /// Expands the size of the container group if the added position is offset from the start position. This method is only used when creating the container.
        /// </summary>
        /// <param name="x">x Position</param>
        /// <param name="y">y Position</param>
        public void AddPosition(int x, int y)
        {
            // TODO FIX THIS BULLSHIT
            if (currentPosition.x < x)
            {
                Debug.Log($"Input: {x} | Current: {currentPosition.x} Increasing {id} X");
                size.x += 1;
                currentPosition.x += 1;
            }

            if (currentPosition.y < y)
            {
                Debug.Log($"Input: {y} | Current: {currentPosition.y} Increasing {id} Y");
                size.y += 1;
                currentPosition.y += 1;
            }
        }
    }
}