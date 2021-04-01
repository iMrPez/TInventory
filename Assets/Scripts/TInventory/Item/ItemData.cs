using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(fileName = "BasicItemData", menuName = "Inventory/Item/Basic Item Data")]
    public class ItemData : ScriptableObject
    {
        /// <summary>
        /// Item id
        /// </summary>
        public int id;
        
        /// <summary>
        /// Item name
        /// </summary>
        public string itemName;
        
        /// <summary>
        /// Item description
        /// </summary>
        public string description;
        
        /// <summary>
        /// Item's image
        /// </summary>
        public Sprite image;

        /// <summary>
        /// Max size the image can be stretched to.
        /// </summary>
        public Vector2 maxImageSize;
        
        /// <summary>
        /// Item's size * slot size
        /// </summary>
        public Vector2 size;
        
        /// <summary>
        /// Item's maximum count, used for stacking or loadable items.
        /// </summary>
        public int maxCount;
    }
}
