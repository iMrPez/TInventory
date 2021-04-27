using System;
using TInventory.Item;
using UnityEngine;

namespace TInventory.Container
{
    public class BasicContainer : Container, ISaveable
    {
        private void Start()
        {
            Load();
        }

        private void OnDisable()
        {
            Save();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Save()
        {
            ObjectHandler.Save(this, containerId);
        }

        public void Load()
        {
            if (containerId == 0) return;
            
            var model = ObjectHandler.Load(containerId);

            if (model is null) return;
            
            LoadModel(model);
        }
    }
}
