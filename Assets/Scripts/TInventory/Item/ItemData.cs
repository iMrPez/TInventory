using UnityEngine;

namespace TInventory.Item
{
    [CreateAssetMenu(fileName = "BasicItemData", menuName = "Inventory/Item/Basic Item Data")]
    public class ItemData : ScriptableObject
    {
        /// <summary>
        /// Item id
        /// </summary>
        public int id;

        /// <summary>
        /// Items prefab type
        /// </summary>
        public ItemPrefab.ItemPrefabType itemPrefabType;
        
        /// <summary>
        /// Item name
        /// </summary>
        public string itemName;

        /// <summary>
        /// Items type
        /// </summary>
        public ItemType itemType;
        
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
