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
        
        private Vector2 _currentPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        
        public void Initialize(int id, Vector2 position, Vector2 size, Container container)
        {
            this.id = id;
            this.position = position;
            _currentPosition = position;
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
            if (_currentPosition.x < x)
            {
                size.x += 1;
                _currentPosition.x += 1;
            }

            if (_currentPosition.y < y)
            {
                size.y += 1;
                _currentPosition.y += 1;
            }
        }
    }
}