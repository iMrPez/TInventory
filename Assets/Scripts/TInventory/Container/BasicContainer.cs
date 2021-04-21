using System;
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
            var model = ObjectHandler.Load<ContainerModel>(containerId);

            if (model is null) return;
            
            LoadModel(model);
        }
    }
}
