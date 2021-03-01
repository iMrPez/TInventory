using System.Linq;
using Inventory.Item;
using TInventory.Container;
using UnityEngine;

namespace TInventory.Item
{
    public class ItemFactory : MonoBehaviour
    {
        /// <summary>
        /// Item factory singleton instance.
        /// </summary>
        public static ItemFactory instance;

        // TODO REDO prefab holding and instantiation.
        /// <summary>
        /// Basic item prefab.
        /// </summary>
        [SerializeField]
        private GameObject basicItemPrefab;
        
        /// <summary>
        /// Basic item data.
        /// </summary>
        [SerializeField]
        private ItemData[] items;
        

        private void Awake()
        {
            instance = this;
        }
        
        /// <summary>
        /// Retrieve item by id.
        /// </summary>
        /// <param name="id">Id of item to lookup</param>
        /// <returns>Returns item if an item with the id is found./</returns>
        public ItemData GetItemById(int id)
        {
            try
            {
                return items.First(i => i.id == id);
            }
            catch
            {
                Debug.LogError($"No item with ID({id} found!", this);
                throw;
            }
        }


        /// <summary>
        /// Create basic items based on id.
        /// </summary>
        /// <param name="id">Id of item to lookup</param>
        /// <param name="container">Container the item will be contained in</param>
        /// <returns>Returns initialized item</returns>
        public BasicItem CreateBasicItem(int id)
        {
            var itemData = GetItemById(id);
            
            if (itemData != null)
            {
                Debug.Log($"Item({id}) Created - {itemData}", itemData);
                
                var item = Instantiate(basicItemPrefab).GetComponent<BasicItem>();
            
                item.Initialize(itemData, null);

                item.SetCount(3);
                
                return item;
            }

            Debug.LogError($"Error Creating Item with ID({id}!", this);

            return null;
        }

    }
}