using System;
using UnityEngine;

namespace TInventory.Item
{
    [Serializable]
    public struct ItemModelWrapper
    {
        public int itemID;
        public string model;
        public int containerGroupId;
        public Vector2 slot;

        public ItemModelWrapper(int itemID, object model, int containerGroupId, Vector2 slot)
        {
            this.itemID = itemID;
            this.model = JsonUtility.ToJson(model);
            this.containerGroupId = containerGroupId;
            this.slot = slot;
        }
    }
}