using System;
using UnityEngine;

namespace TInventory.Item
{
    [CreateAssetMenu(fileName = "Item Type", menuName = "Inventory/Filter/Item Type")]
    public class ItemType : ScriptableObject
    {
        public string typeName;
    }
}
