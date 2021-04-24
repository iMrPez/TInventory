using System.IO;
using UnityEngine;

namespace TInventory
{
    public static class ObjectHandler
    {
        public static bool Save(IManageable obj, int id)
        {
            if (id == 0) return false;
            
            object model = obj.GetModel();

            string json = JsonUtility.ToJson(model);
            
            var savedObject = new SavedObjectWrapper(id, json);

            string savedJson = JsonUtility.ToJson(savedObject);

            try
            {
                File.WriteAllText(Application.persistentDataPath + $"//{id}.txt", savedJson);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Load(int id)
        {
            try
            {
                var json = File.ReadAllText(Application.persistentDataPath + $"//{id}.txt");

                var loadedObject = JsonUtility.FromJson<SavedObjectWrapper>(json);
                return loadedObject.json;
            }
            catch
            {
                Debug.LogWarning("No file found!");
                return null;
            }

        }
    }
}