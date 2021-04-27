using System;

namespace TInventory
{
    [Serializable]
    public struct SavedObjectWrapper
    {
        public int id;
        public string json;

        public SavedObjectWrapper(int id, string json)
        {
            this.id = id;
            this.json = json;
        }
    }
}