using System;
using UnityEngine;

namespace TInventory
{
    [Serializable]
    public struct ItemPrefab
    {
        public enum ItemPrefabType
        {
            Basic = 0,
            Container = 1
        }

        public ItemPrefabType type;
        public GameObject prefab;

        public ItemPrefab(ItemPrefabType type, GameObject prefab)
        {
            this.type = type;
            this.prefab = prefab;
        }
    }
}
