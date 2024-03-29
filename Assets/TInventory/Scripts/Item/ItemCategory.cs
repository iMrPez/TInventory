using System;
using System.Collections.Generic;
using UnityEngine;

namespace TInventory.Item
{
    
    [CreateAssetMenu(fileName = "Item Category", menuName = "TInventory/Filter/Item Category")]
    public class ItemCategory : ScriptableObject
    {
        public List<TypeCategory> types;
        
        /// <summary>
        /// Get rarity of item
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns>Rarity</returns>
        public float GetRarity(ItemData itemData)
        {
            foreach (var type in types)
            {
                if (type.type == itemData.itemType) return type.rarity;
            }

            return 0;
        }
    }

    [Serializable]
    public struct TypeCategory
    {
        public ItemType type;
        public float rarity;
    }
}
