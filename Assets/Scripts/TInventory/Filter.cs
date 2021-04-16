using TInventory.Item;
using UnityEngine;

namespace TInventory.Filter
{
    [CreateAssetMenu(fileName = "New Filter", menuName = "Inventory/Filter/Filter")]
    public class Filter : ScriptableObject
    {
        public ItemCategory allowedCategory;

        public bool IsMatching(Item.Item item)
        {
            return IsMatching(item.Data);
        }
        
        public bool IsMatching(ItemData itemData)
        {

            foreach (var type in allowedCategory.types)
            {
                if (itemData.itemType == type.type) return true;
                
            }
            
            return false;
        }

        
    }
}
