using System;
using UnityEngine;

namespace TInventory.Item
{
    [Serializable]
    public struct ItemModel
    {
        public int id;

        public bool isRotated;

        public int count;

        public int containerGroupId;
        public Vector2 slot;

        public ItemModel(int id, bool isRotated, int count, int containerGroupId, Vector2 slot)
        {
            this.id = id;
            this.isRotated = isRotated;
            this.count = count;
            this.containerGroupId = containerGroupId;
            this.slot = slot;
        }
    }
}