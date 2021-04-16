using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace TInventory.Item
{
    public class ItemFactory : MonoBehaviour
    {
        public static ItemFactory Instance;
        
        public List<ItemPrefab> itemPrefabs = new List<ItemPrefab>();
        
        public List<ItemData> items;
        

        private void Awake()
        {
            Instance = this;
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
        /// Create items based on id.
        /// </summary>
        /// <param name="prefabType">Type of prefab</param>
        /// <param name="id">Id</param>
        /// <param name="container">Container</param>
        /// <returns>Returns initialized item</returns>
        public Item CreateItem(int id)
        {
            var itemData = GetItemById(id);

            return CreateItem(itemData);
        }

        /// <summary>
        /// Get prefab that match type
        /// </summary>
        /// <param name="prefabType"></param>
        /// <returns>Prefab</returns>
        public GameObject GetItemPrefab(ItemPrefab.ItemPrefabType prefabType) =>
            itemPrefabs.First(i => i.type == prefabType).prefab;
        
        
        /// <summary>
        /// Creates item based on itemData
        /// </summary>
        /// <param name="itemData">Item Data</param>
        /// <returns>Item</returns>
        public Item CreateItem(ItemData itemData)
        {
            if (itemData != null)
            {
                Debug.Log($"Item({itemData.id}) Created - {itemData}", itemData);
                
                var item = Instantiate(GetItemPrefab(itemData.itemPrefabType)).GetComponent<BasicItem>();
            
                item.Initialize(itemData, null);

                item.SetCount(1);
                
                return item;
            }

            Debug.LogError($"Error Creating Item with ID({itemData.id}!", this);

            return null;
        }

        /// <summary>
        /// Get list of items that match filter with their rarity according to the filters category
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Item and its rarity</returns>
        public static IEnumerable<(ItemData item, float rarity)> GetFilteredItemList(Filter.Filter filter)
        {
            foreach (var item in Instance.items)
            {
                var rarity = filter.allowedCategory.GetRarity(item);
                
                if (rarity > 0)
                {
                    yield return (item, rarity);
                }
            }
        }

        
        /// <summary>
        /// Gets random item from supplied list based on its rarity
        /// </summary>
        /// <param name="items">Items and their rarity</param>
        /// <returns>Random Item</returns>
        public static ItemData GetRandomItemFromList(List<(ItemData item, float rarity)> items)
        {
            var totalRarity = items.Sum(x => x.rarity);
            
            var randomNumber = 1 + Math.Round(new Random(DateTime.Now.Millisecond).NextDouble() * (totalRarity - 1));

            var item = items.OrderBy(x => x.item.id).Select(x => new
            {
                x.item,
                
                MinRarity = items.Where(y => y.item.id <= x.item.id).Sum(y => y.rarity) - x.rarity + 1,
                MaxRarity = items.Where(y => y.item.id <= x.item.id).Sum(y => y.rarity)
            }).Single(x => x.MinRarity <= randomNumber && x.MaxRarity >= randomNumber);

            return item.item;
        }
    }
}