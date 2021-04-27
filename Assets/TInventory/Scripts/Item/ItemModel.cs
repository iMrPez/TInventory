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

        public ItemModel(int id, bool isRotated, int count)
        {
            this.id = id;
            this.isRotated = isRotated;
            this.count = count;
        }
    }
}